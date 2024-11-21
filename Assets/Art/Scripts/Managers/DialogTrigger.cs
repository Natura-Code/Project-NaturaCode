using System.Collections;
using System.Collections.Generic;
using cherrydev;
using Unity.VisualScripting;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour; // Komponen DialogBehaviour
    [SerializeField] private DialogNodeGraph dialogGraph; // Graph dialog yang digunakan

    private void Update()
    {
        // Periksa apakah tombol E ditekan
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartDialog();
        }
    }

    private void StartDialog()
    {
        if (dialogBehaviour != null && dialogGraph != null)
        {
            dialogBehaviour.StartDialog(dialogNodeGraph: dialogGraph);
            Debug.Log("Dialog dimulai."); // Log untuk debugging
        }
        else
        {
            Debug.LogWarning("DialogBehaviour atau DialogGraph belum dihubungkan di Inspector!");
        }
    }
}

