using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class DialogueResponse
{
    public string responseText; // Teks respons
    public DialogueNode nextNode; // Node dialog berikutnya

    // Variabel untuk perpindahan scene
    public bool triggersSceneChange; // Menentukan apakah respons ini memicu perpindahan scene
    public string sceneToLoad; // Nama scene yang akan di-load jika triggersSceneChange == true
}

