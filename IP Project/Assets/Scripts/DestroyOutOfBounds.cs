using System.Runtime.InteropServices;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    [SerializeField]
    private float topBound = 250f; // Upper boundary
    [SerializeField]
    private float lowerBound = -250f; // Lower boundary

    void Update()
    {
        if (transform.position.x > topBound)
        {
            Debug.Log("Object destroyed: " + gameObject.name);
            Destroy(gameObject);
        }
        else if (transform.position.x < lowerBound)
        {
            Debug.Log("Object destroyed: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
