using UnityEngine;

public class BobClues : MonoBehaviour
{
    public float radius = 2f;  // Radius of the circle
    private float angle = 0f;  // Current angle
    private Vector3 centerPosition;  // Center point of the circle
    private float speed = 1f;  // Speed of rotation in radians per second

    void Start()
    {
        centerPosition = transform.position;  // Store the initial position as center
    }

    void Update()
    {
        // Calculate new position using parametric equation of a circle
        float x = centerPosition.x + radius * Mathf.Cos(angle);
        float y = centerPosition.y + radius * Mathf.Sin(angle);
        
        // Update position
        transform.position = new Vector3(x, y, centerPosition.z);
        
        // Increment angle (multiply by Time.deltaTime for frame-rate independence)
        angle += speed * Time.deltaTime;
    }
}
