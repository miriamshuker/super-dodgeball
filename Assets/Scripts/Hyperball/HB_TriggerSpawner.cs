using System.Collections.Generic;
using UnityEngine;

public class HB_TriggerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public GameObject hyperTriggerPrefab;
    public float spawnCountdown = 10f;
    public bool hyperInPlay;

    void Start()
    {
        spawnPoints = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
            spawnPoints.Add(child.gameObject);

        hyperInPlay = false;
    }

    void Update()
    {
        if (!hyperInPlay)
        {
            SpawnTimer();
        }
    }

    private void SpawnTimer()
    {
        if (spawnCountdown > 0f)
        {
            spawnCountdown -= Time.deltaTime;
        }
        else
        {
            SpawnHyper();
            hyperInPlay = true;
            spawnCountdown = 10f;
            Debug.Log("hyper spawned");
        }
    }

    private void SpawnHyper()
    {
        GameObject selectedSpawnpoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector2 hyperSpawn = new Vector2(selectedSpawnpoint.transform.position.x + (Random.Range(-15f, 15f)), selectedSpawnpoint.transform.position.y + (Random.Range(-15f, 15f)));
        GameObject hyperTriggerInst = Instantiate(hyperTriggerPrefab, hyperSpawn, selectedSpawnpoint.transform.rotation);
    }
}
