using UnityEngine;

public class FishData : MonoBehaviour
{
    public FishAI fishInfo; // Referensi ke ScriptableObject Fish

    void Start()
    {
        if (fishInfo == null)
        {
            Debug.LogWarning($"{name} tidak memiliki Fish ScriptableObject yang terhubung!");
        }
    }
}
