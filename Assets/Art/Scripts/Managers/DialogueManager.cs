using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI characterNameText; // UI Text untuk menampilkan nama karakter
    public TextMeshProUGUI dialogueText; // UI Text untuk menampilkan teks dialog
    public Button[] choiceButtons; // Array button untuk pilihan-pilihan dialog
    public Dialogue[] allDialogues; // Semua dialog yang tersedia, disimpan di sini

    private Dialogue currentDialogue;

    // Memulai dialog pertama
    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        ShowDialogue();
    }

    // Menampilkan dialog dan pilihan
    public void ShowDialogue()
    {
        characterNameText.text = currentDialogue.characterName;
        dialogueText.text = currentDialogue.dialogueText;

        // Menampilkan pilihan
        for (int i = 0; i < currentDialogue.choices.Length; i++)
        {
            choiceButtons[i].GetComponentInChildren<Text>().text = currentDialogue.choices[i].choiceText;
            choiceButtons[i].gameObject.SetActive(true); // Menampilkan button
            int choiceIndex = i; // Menyimpan indeks pilihan
            choiceButtons[i].onClick.AddListener(() => ChooseOption(choiceIndex)); // Menambahkan event listener
        }
    }

    // Ketika pemain memilih pilihan
    public void ChooseOption(int choiceIndex)
    {
        int nextDialogueID = currentDialogue.choices[choiceIndex].nextDialogueID;
        Dialogue nextDialogue = GetDialogueByID(nextDialogueID); // Mendapatkan dialog berdasarkan ID
        if (nextDialogue != null)
        {
            StartDialogue(nextDialogue); // Memulai dialog berikutnya
        }
        else
        {
            EndDialogue(); // Mengakhiri dialog jika tidak ada dialog berikutnya
        }
    }

    // Mendapatkan dialog berdasarkan ID
    private Dialogue GetDialogueByID(int id)
    {
        foreach (Dialogue dialogue in allDialogues)
        {
            if (dialogue.GetInstanceID() == id)
            {
                return dialogue;
            }
        }
        return null;
    }

    // Mengakhiri dialog
    private void EndDialogue()
    {
        dialogueText.text = "";
        characterNameText.text = "";
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false); // Sembunyikan semua pilihan
        }
    }
}