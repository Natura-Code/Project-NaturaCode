using TMPro;
using UnityEngine;
using System.Collections;

public class WaterSurfaceDetector : MonoBehaviour
{
    [SerializeField] private float maxOxygen;
    [SerializeField] private float currentOxygen;
    [SerializeField] private float oxygenRate = 1f;
    private bool isUnderwater = false;

    [SerializeField] private int goldCostForUpgrade;
    private GoldManager goldManager;
    [SerializeField] private UnityEngine.UI.Slider oxygenSlider;

    [SerializeField] private TextMeshProUGUI warningText;

    void Start()
    {
        currentOxygen = maxOxygen;
        UpdateOxygenSlider();
        goldManager = FindObjectOfType<GoldManager>();
    }

    void Update()
    {
        if (isUnderwater)
        {
            currentOxygen -= oxygenRate * Time.deltaTime;
            if (currentOxygen <= 0)
            {
                currentOxygen = 0;
                HandleOxygenDepleted();
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
            Debug.Log("Oksigen tekah ditingkatkan, Sekarang: " + maxOxygen);
            StartCoroutine(ShowWarningTemporary("Pembelian Berhasil Oksigen = " + maxOxygen, 2f));
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

    void HandleOxygenDepleted()
    {
        Debug.Log("Pemain kehabisan oksigen!");
    }

    private IEnumerator ShowWarningTemporary(string message, float delay)
    {
        warningText.text = message;
        yield return new WaitForSecondsRealtime(delay);
        warningText.text = "";
    }
}
