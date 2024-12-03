using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("Fish Settings")]
    public GameObject fishPrefab; 
    public Vector2 spawnAreaMin; 
    public Vector2 spawnAreaMax; 
    public float spawnInterval = 5f; 

    private void Start()
    {
        StartCoroutine(SpawnFishAtIntervals());
    }

    private IEnumerator SpawnFishAtIntervals()
    {
        while (true)
        {
            SpawnFish(); 
            yield return new WaitForSeconds(spawnInterval); 
        }
    }

    public void SpawnFish()
    {
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
    }
}
