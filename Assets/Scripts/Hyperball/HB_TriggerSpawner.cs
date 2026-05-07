using System.Collections.Generic;
using UnityEngine;

public class HB_TriggerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public GameObject hyperTriggerPrefab;
    private float spawnCountdown = 10f;
    private bool hyperInPlay;

    void Start()
    {
        hyperInPlay = false;
    }


    void Update()
    {
        SpawnTimer();
    }


    private void SpawnTimer()
    {
        if (spawnCountdown > 0f && !hyperInPlay)
        {
            spawnCountdown -= Time.deltaTime;
        }
        else
        {
            SpawnHyper();
            hyperInPlay = true;
            spawnCountdown = 10f;
        }
    }

    private void SpawnHyper()
    {
        GameObject selectedSpawnpoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector2 hyperSpawn = new Vector2(selectedSpawnpoint.transform.position.x + (Random.Range(-15f, 15f)), selectedSpawnpoint.transform.position.y + (Random.Range(-15f, 15f)));
        GameObject hyperTriggerInst = Instantiate(hyperTriggerPrefab, hyperSpawn, selectedSpawnpoint.transform.rotation);
    }
}
