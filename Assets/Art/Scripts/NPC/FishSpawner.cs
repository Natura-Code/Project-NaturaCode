using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    public GameObject[] fishPrefabs; // Array prefab ikan

    public int fishCount = 10; // Jumlah ikan yang akan di-spawn
    public BoxCollider2D spawnArea; // Area spawn

    void Start()
    {
        for (int i = 0; i < fishCount; i++)
        {
            SpawnFish();
        }
    }


    void SpawnFish()
    {
        if (spawnArea == null || fishPrefabs.Length == 0) return; // Cek jika tidak ada area atau prefab

        // Tentukan posisi spawn secara acak dalam area
        Bounds bounds = spawnArea.bounds;
        Vector2 spawnPosition = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );

        // Pilih salah satu fishPrefab secara acak
        GameObject randomFishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];

        // Spawn ikan
        Instantiate(randomFishPrefab, spawnPosition, Quaternion.identity);
    }


    void OnDrawGizmosSelected()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
    }
}
