using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public string acceptedTag;

    public bool IsCorrectBin(string trashTag)
    {
        return trashTag == acceptedTag;
    }
}

