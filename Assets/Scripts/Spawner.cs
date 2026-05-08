using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawner;
    public bool stopSpawn = false;
    public float spawnTime;
    public float spawnDelay;
    
    private GameObject currentSpawnedObject;
    private float timer = 0f;

    void Start()
    {
        timer = spawnTime;
    }

    void Update()
    {
        if (stopSpawn) return;

        if (currentSpawnedObject == null || !currentSpawnedObject.activeInHierarchy)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SpawnObject();
                timer = spawnDelay;
            }
        }
    }

    public void SpawnObject()
    {
        if (currentSpawnedObject == null)
        {
            currentSpawnedObject = Instantiate(spawner, transform.position, transform.rotation);
        }
        else
        {
            currentSpawnedObject.transform.position = transform.position;
            currentSpawnedObject.transform.rotation = transform.rotation;
            currentSpawnedObject.SetActive(true);
        }
    }
}
