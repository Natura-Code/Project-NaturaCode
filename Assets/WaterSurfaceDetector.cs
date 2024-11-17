using UnityEngine;

public class WaterSurfaceDetector : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 10f;
    [SerializeField] private float currentOxygen;
    [SerializeField] private float oxygenRate = 1f;
    private bool isUnderwater = false;

    public UnityEngine.UI.Slider oxygenSlider;

    void Start()
    {
        currentOxygen = maxOxygen;
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = currentOxygen;
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
            }
        }

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
}
