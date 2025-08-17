using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f; // Speed of the car

    [SerializeField]
    private float topBound = 210f;
    [SerializeField]
    private float lowerBound = -210f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > topBound)
        {
            // If the car goes beyond the top boundary, destroy it
            Destroy(gameObject);
            Debug.Log("Car deleted");
        }
        else if (transform.position.x < lowerBound)
        {
            // If the car goes below the lower boundary, destroy it
            Destroy(gameObject);
            Debug.Log("Car deleted");
        }
        // Move the car forward at a constant speed
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    // Destroy the car 
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Car collided with: " + other.name);
        if (other.CompareTag("DeadZone"))
        {
            Debug.Log("Car deleted");
            Destroy(gameObject); // Remove the car from the scene
        }
    }
}
