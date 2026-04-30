using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodgeballSpawnerScript : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public GameObject dodgeballPrefab;
    private float timeSinceLastSpawn = 0f;
    public float spawnInterval =  5f;
    private int dodgeballsSpawned = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimer();
    }

    
    private void SpawnTimer()
    {
        if (timeSinceLastSpawn < spawnInterval)
        {
            timeSinceLastSpawn += Time.deltaTime;
        }
        else
        {
            SpawnDodgeball();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnDodgeball()
    {
        GameObject selectedSpawnpoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector2 dodgeballSpawn = new Vector2(selectedSpawnpoint.transform.position.x + (Random.Range(-15f, 15f)),selectedSpawnpoint.transform.position.y + (Random.Range(-15f, 15f)));
        GameObject dodgeballInst = Instantiate(dodgeballPrefab, dodgeballSpawn, selectedSpawnpoint.transform.rotation);
        dodgeballInst.GetComponent<DodgeballScript>()._isLive = false;
        
        dodgeballsSpawned += 1;
        if(dodgeballsSpawned > 10)
        {
            spawnInterval = 4f;
        } else if(dodgeballsSpawned > 20)
        {
            spawnInterval = 3f;
        } else if(dodgeballsSpawned > 30)
        {
            spawnInterval = 2f;
        }
    }



}
