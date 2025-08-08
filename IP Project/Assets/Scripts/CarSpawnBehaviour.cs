using System.Threading;
using UnityEngine;

public class CarSpawnBehaviour : MonoBehaviour
{
    public GameObject carPrefab1; // Prefab of the car to spawn
    public GameObject carPrefab2; // Prefab of the car to spawn
    public GameObject carPrefab3; // Prefab of the car to spawn
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

        // Randomly select one of the car prefabs to spawn
        GameObject selectedCarPrefab = carPrefab1;
        int randomCar = Random.Range(1, 4);
        switch (randomCar)
        {
            case 1:
                selectedCarPrefab = carPrefab1;
                break;
            case 2:
                selectedCarPrefab = carPrefab2;
                break;
            case 3:
                selectedCarPrefab = carPrefab3;
                break;
        }
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomSpawnIndex];
        Debug.Log("Car spawned");
        Instantiate(selectedCarPrefab, selectedSpawnPoint.position, transform.rotation);
    }
}
