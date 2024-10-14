using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string npcName; // Nama NPC
    [TextArea(3, 10)]
    public string[] sentences; // Kalimat-kalimat dalam dialog
}
