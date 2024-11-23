using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog Data")]
public class DialogData : ScriptableObject
{
    public string characterName; // Character's name
    public Sprite characterIcon; // Character's icon
    [TextArea(3, 10)]
    public List<string> dialogLines; // Lines of dialog
}