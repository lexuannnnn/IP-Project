using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f; // Speed of the car
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
