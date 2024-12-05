using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogTrigger : MonoBehaviour
{
    public DialogData dialogData; // Dialog data for this trigger
    private DialogManager dialogManager; // Reference to DialogManager

    public bool shouldGrantItem; // Example action: grant item
    public bool shouldChangeScene; // Example action: change scene
    public string targetScene; // Target scene name for scene change

    private void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dialogData != null)
            {
                dialogManager.StartDialog(dialogData, OnDialogComplete);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogManager.EndDialog();
        }
    }

    private void OnDialogComplete()
    {
        if (shouldGrantItem)
        {
            GrantItem();
        }

        if (shouldChangeScene && !string.IsNullOrEmpty(targetScene))
        {
            ChangeScene(targetScene);
        }
    }

    private void GrantItem()
    {
        Debug.Log("Player received an item!");
        // Tambahkan logika pemberian item di sini (misalnya tambahkan ke inventory)
    }

    private void ChangeScene(string sceneName)
    {
        Debug.Log("Changing scene to: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
