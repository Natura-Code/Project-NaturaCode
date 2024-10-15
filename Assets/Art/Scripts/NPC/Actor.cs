using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public string Name;
    public Dialogue Dialogue;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpeakTo();
        }
    }

    // Trigger dialogue for this actor
    public void SpeakTo()
    {
        DialogueManager.Instance.StartDialogue(Name, Dialogue.RootNode);
    }
}

