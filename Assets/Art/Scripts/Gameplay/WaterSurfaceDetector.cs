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
    private GoldManager goldManager;
    [SerializeField] private UnityEngine.UI.Slider oxygenSlider;

    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TextMeshProUGUI countdownText; 
    [SerializeField] private Transform startingPoint; 
    private bool isOxygenDepleted = false; 
    private Coroutine oxygenDepletedCoroutine; 

    void Start()
    {
        currentOxygen = maxOxygen;
        UpdateOxygenSlider();
        goldManager = FindObjectOfType<GoldManager>();
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
        if (goldManager != null && goldManager.SpendGold(goldCostForUpgrade))
        {
            maxOxygen += 10f;
            currentOxygen = maxOxygen;
            UpdateOxygenSlider();
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
            Debug.Log("Tidak cukup emas untuk peningkatan!");
            StartCoroutine(ShowWarningTemporary("Gold tidak cukup!", 2f));
        }
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

        if (goldManager != null)
        {
            int currentGold = goldManager.GetGold(); 
            if (currentGold >= 100)
            {
                goldManager.ChangeGold(-100); 
                Debug.Log("Kembali ke titik awal. Emas berkurang 100.");
            }
            else
            {
                goldManager.ChangeGold(-currentGold); 
                Debug.Log("Kembali ke titik awal. Semua emas habis.");
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
