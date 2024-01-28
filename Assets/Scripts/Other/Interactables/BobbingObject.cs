using UnityEngine;

public class BobbingObject : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.5f; // Height of the bob
    [SerializeField]
    private float frequency = 1f; // Speed of the bob

    private Vector3 startPos;

    void Start()
    {
        // Store the starting position of the object
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);
        
        // Set the position of the object
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
