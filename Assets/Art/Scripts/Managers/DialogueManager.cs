using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI npcNameText; // Komponen UI untuk menampilkan teks dialog
    public TextMeshProUGUI dialogueText; // Komponen UI untuk menampilkan teks dialog
    public GameObject dialoguePanel; // Panel untuk dialog UI

    private Queue<string> sentences; // Queue untuk menyimpan kalimat-kalimat dialog

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false); // Panel dialog tidak muncul di awal
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Tekan Space untuk lanjut ke kalimat berikutnya
        {
            DisplayNextSentence();
        }
    }


    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true); // Tampilkan panel dialog
        npcNameText.text = dialogue.npcName;
        sentences.Clear(); // Bersihkan queue

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence); // Tambahkan kalimat ke dalam queue
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue(); // Akhiri dialog jika tidak ada kalimat tersisa
            return;
        }

        string sentence = sentences.Dequeue(); // Ambil kalimat pertama dari queue
        dialogueText.text = sentence; // Tampilkan kalimat di UI
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Sembunyikan panel dialog
    }
}
