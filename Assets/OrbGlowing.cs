using UnityEngine;

public class OrbGlowing : MonoBehaviour
{
    private float delay = 0.2f; // 200ms delay
    private float delayTimer = 0f;
    private bool waiting = true;
    public float glowSpeed = 1.2f; // Speed of the glow effect
    private bool growing = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer >= delay)
        {

            delayTimer = 0f;
            if(growing) {
                transform.localScale *= glowSpeed;
                if (transform.localScale.x >= 2f) {
                    growing = false;
                }
            }

            else {
                transform.localScale /= glowSpeed;
                if (transform.localScale.x <= 1.0f) {
                    growing = true;
                }
            }
        }
    }

}
