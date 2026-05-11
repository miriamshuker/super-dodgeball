using System.Collections.Generic;
using UnityEngine;

public class HB_TriggerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public GameObject hyperTriggerPrefab;
    public float spawnCountdown = 10f;
    public bool hyperInPlay;

    [SerializeField] private float minPlayerDistance = 3f;
    [SerializeField] private int maxSpawnAttempts = 10;
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        spawnPoints = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
            spawnPoints.Add(child.gameObject);

        hyperInPlay = false;

        // Grab all players once at start
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            players.Add(p);
    }

    void Update()
    {
        if (!hyperInPlay)
            SpawnTimer();
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
        }
    }

    private void SpawnHyper()
    {
        GameObject selectedSpawnpoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector2 candidate = new Vector2(
                selectedSpawnpoint.transform.position.x + Random.Range(-15f, 15f),
                selectedSpawnpoint.transform.position.y + Random.Range(-15f, 15f)
            );

            if (!TooCloseToPlayer(candidate))
            {
                Instantiate(hyperTriggerPrefab, candidate, selectedSpawnpoint.transform.rotation);
                //Debug.Log($"Hyper spawned on attempt {i + 1}");
                return;
            }
        }

        Vector2 fallback = new Vector2(
            selectedSpawnpoint.transform.position.x + Random.Range(-15f, 15f),
            selectedSpawnpoint.transform.position.y + Random.Range(-15f, 15f)
        );
        Instantiate(hyperTriggerPrefab, fallback, selectedSpawnpoint.transform.rotation);
    }

    private bool TooCloseToPlayer(Vector2 position)
    {
        foreach (GameObject player in players)
        {
            if (player == null) continue;
            if (Vector2.Distance(position, player.transform.position) < minPlayerDistance)
                return true;
        }
        return false;
    }
}