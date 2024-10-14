using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string characterName; // Nama karakter yang berbicara
    [TextArea(3, 10)]
    public string dialogueText; // Teks dialog yang akan ditampilkan

    public DialogueChoice[] choices; // Array pilihan yang tersedia untuk pemain
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText; // Teks pilihan yang ditampilkan kepada pemain
    public int nextDialogueID; // ID dialog berikutnya, menghindari referensi siklus
}


