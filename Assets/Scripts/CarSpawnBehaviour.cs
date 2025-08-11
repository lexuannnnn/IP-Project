using System.Threading;
using UnityEngine;

public class CarSpawnBehaviour : MonoBehaviour
{
    public GameObject[] carPrefabs; // Prefabs of the car to spawn
    public Transform[] spawnPoints; // Transform containing spawn points
    public float minSpawnInterval = 2f; // Minimum time interval between spawns
    public float maxSpawnInterval = 5f; // Maximum time interval between spawns
    public float spawnInterval; // Current spawn interval
    private float timer = 0f; // Time when the next car will be spawned
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnCar(); // Spawn the first car immediately
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnInterval)
        {
            timer += Time.deltaTime; // Increment the timer by the time passed since last frame
            return; // Exit if the interval has not yet passed
        }
        else
        {
            spawnCar();
            timer = 0f; // Reset the timer after spawning a car
        }
    }
    
    void spawnCar()
    {
        float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        spawnInterval = randomInterval;

        // Randomly select a car prefab to spawn
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomSpawnIndex];
        int carIndex = Random.Range(0, carPrefabs.Length);
        Debug.Log("Car spawned");
        Instantiate(carPrefabs[carIndex], selectedSpawnPoint.position, transform.rotation);
    }
}
