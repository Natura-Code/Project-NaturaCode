using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox; // Dialog UI panel
    public TextMeshProUGUI nameText; // Character name text
    public TextMeshProUGUI dialogText; // Dialog line text
    public Image iconImage; // Character icon image

    private Queue<string> sentences; // Queue for storing dialog lines
    private bool isTyping; // To check if a text animation is in progress
    private Coroutine typingCoroutine; // To manage typing coroutine

    private bool isInDialog; // Track if dialog is active
    public Action onDialogEnd; // Callback for custom actions

    private void Start()
    {
        sentences = new Queue<string>();
        dialogBox.SetActive(false); // Hide dialog box initially
    }

    // Start the dialog by receiving data
    public void StartDialog(DialogData dialogData, Action callback = null)
    {
        dialogBox.SetActive(true);
        nameText.text = dialogData.characterName;
        iconImage.sprite = dialogData.characterIcon;
        isInDialog = true;

        sentences.Clear();
        foreach (string sentence in dialogData.dialogLines)
        {
            sentences.Enqueue(sentence);
        }

        onDialogEnd = callback; // Assign the callback
        DisplayNextSentence();
    }

    // Display the next sentence in the queue
    public void DisplayNextSentence()
    {
        if (!isInDialog) return;

        if (isTyping) // Skip current animation if player proceeds
        {
            StopCoroutine(typingCoroutine);
            dialogText.text = sentences.Peek();
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string sentence = sentences.Dequeue();
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    // Animate text typing
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.02f); // Typing speed
        }
        isTyping = false;
    }

    // End the dialog
    public void EndDialog()
    {
        if (dialogBox != null) // Periksa apakah dialogBox masih ada
        {
            dialogBox.SetActive(false);
        }

        isInDialog = false;

        // Call the callback function if it exists
        onDialogEnd?.Invoke();
    }

}