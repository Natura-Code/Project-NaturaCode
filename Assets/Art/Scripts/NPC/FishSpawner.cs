using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("Fish Settings")]
    public GameObject fishPrefab; // Prefab ikan yang akan di-spawn
    public Vector2 spawnAreaMin; // Titik minimal area spawn (misalnya, kiri bawah)
    public Vector2 spawnAreaMax; // Titik maksimal area spawn (misalnya, kanan atas)
    public float spawnInterval = 5f; // Interval waktu untuk respawn ikan

    private void Start()
    {
        // Mulai coroutine untuk spawn ikan setiap interval
        StartCoroutine(SpawnFishAtIntervals());
    }

    // Coroutine untuk spawn ikan secara berkala
    private IEnumerator SpawnFishAtIntervals()
    {
        while (true)
        {
            SpawnFish(); // Spawn ikan baru
            yield return new WaitForSeconds(spawnInterval); // Tunggu selama interval yang ditentukan
        }
    }

    // Fungsi untuk spawn ikan
    public void SpawnFish()
    {
        // Tentukan posisi spawn acak dalam area spawn
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Spawn ikan pada posisi acak
        Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
    }
}
