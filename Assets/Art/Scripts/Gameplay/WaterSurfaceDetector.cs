using TMPro;
using UnityEngine;
using System.Collections;

public class WaterSurfaceDetector : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float currentOxygen;
    [SerializeField] private float oxygenRate = 1f;
    private bool isUnderwater = false;

    [SerializeField] private int goldCostForUpgrade = 50;
    private PlayerWallet playerWallet;
    [SerializeField] private UnityEngine.UI.Slider oxygenSlider;

    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Transform startingPoint;
    private bool isOxygenDepleted = false;
    private Coroutine oxygenDepletedCoroutine;

    private const string MaxOxygenKey = "MaxOxygen"; 

    void Start()
    {
        maxOxygen = PlayerPrefs.GetFloat(MaxOxygenKey, maxOxygen);
        currentOxygen = maxOxygen;
        UpdateOxygenSlider();

        playerWallet = FindObjectOfType<PlayerWallet>();
        countdownText.text = "";
    }

    void Update()
    {
        if (isUnderwater)
        {
            currentOxygen -= oxygenRate * Time.deltaTime;
            if (currentOxygen <= 0)
            {
                currentOxygen = 0;
                if (!isOxygenDepleted)
                {
                    isOxygenDepleted = true;
                    oxygenDepletedCoroutine = StartCoroutine(HandleOxygenDepletionCountdown());
                }
            }
        }
        else
        {
            if (currentOxygen < maxOxygen)
            {
                currentOxygen += oxygenRate * Time.deltaTime;
                if (currentOxygen > maxOxygen)
                {
                    currentOxygen = maxOxygen;
                }
            }

            if (isOxygenDepleted)
            {
                isOxygenDepleted = false;
                if (oxygenDepletedCoroutine != null)
                {
                    StopCoroutine(oxygenDepletedCoroutine);
                    countdownText.text = "";
                }
            }
        }

        oxygenSlider.value = currentOxygen;
    }

    public void UpgradeMaxOxygen()
    {
        if (playerWallet != null && playerWallet.SpendMoney(goldCostForUpgrade))
        {
            maxOxygen += 10f;
            currentOxygen = maxOxygen;
            UpdateOxygenSlider();

            SaveMaxOxygen();

            Debug.Log("Oksigen telah ditingkatkan, Sekarang: " + maxOxygen);
            StartCoroutine(ShowWarningTemporary("Pembelian Berhasil Oksigen = " + maxOxygen, 2f));

            if (isOxygenDepleted)
            {
                isOxygenDepleted = false;
                if (oxygenDepletedCoroutine != null)
                {
                    StopCoroutine(oxygenDepletedCoroutine);
                    countdownText.text = "";
                }
            }
        }
        else
        {
            Debug.Log("Tidak cukup uang untuk peningkatan!");
            StartCoroutine(ShowWarningTemporary("Uang tidak cukup!", 2f));
        }
    }

    private void SaveMaxOxygen()
    {
        PlayerPrefs.SetFloat(MaxOxygenKey, maxOxygen);
        PlayerPrefs.Save(); 
    }

    private void UpdateOxygenSlider()
    {
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = currentOxygen;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WaterSurface"))
        {
            isUnderwater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("WaterSurface"))
        {
            isUnderwater = false;
        }
    }

    private IEnumerator HandleOxygenDepletionCountdown()
    {
        float countdown = 5f;
        StartCoroutine(ShowWarningTemporary("Oksigen habis! Tinggalkan air atau upgrade oksigen!", countdown));

        while (countdown > 0)
        {
            countdownText.text = "Respawning: " + Mathf.CeilToInt(countdown);
            yield return new WaitForSeconds(1f);
            countdown--;

            if (!isUnderwater || !isOxygenDepleted)
            {
                countdownText.text = "";
                yield break;
            }
        }

        countdownText.text = "";
        transform.position = startingPoint.position;

        if (playerWallet != null)
        {
            int currentMoney = playerWallet.currentMoney;
            if (currentMoney >= 100)
            {
                playerWallet.SpendMoney(100);
                Debug.Log("Kembali ke titik awal. Uang berkurang 100.");
            }
            else
            {
                playerWallet.SpendMoney(currentMoney);
                Debug.Log("Kembali ke titik awal. Semua uang habis.");
            }
        }

        currentOxygen = maxOxygen;
        isOxygenDepleted = false;
        UpdateOxygenSlider();
    }

    private IEnumerator ShowWarningTemporary(string message, float delay)
    {
        warningText.text = message;
        yield return new WaitForSecondsRealtime(delay);
        warningText.text = "";
    }
}
