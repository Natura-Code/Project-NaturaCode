using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable objects/Item")]
public class Item : ScriptableObject
{
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only gameplay")]
    public TileBase tile;
    public int sellValue; // Nilai jual item
    public ItemType type;
    public ActionType actionType;

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;

}

public enum ItemType
{
    Weapon, // Harpoon, Tombak, Jaring
    Utility, // Oxygen Tank
    Collectible // Ikan, Sampah
}

public enum ActionType
{
    Catch, // Menangkap ikan/sampah
    Shoot, // Menggunakan Harpoon/Tombak
    RefillOxygen // Mengisi ulang oksigen
}