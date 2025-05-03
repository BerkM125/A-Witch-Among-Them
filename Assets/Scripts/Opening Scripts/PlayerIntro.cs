using UnityEngine;
using System.Collections;

public class PlayerIntro : MonoBehaviour
{
    public float speed = 0.2f; // Slow speed for downward movement
    private Rigidbody2D rb;
    private float moveDuration = 1f; // Duration of movement in seconds
    private float elapsedTime = 0f;
    public DialogueBoxController dialogueBoxController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Ensure no gravity affects the player
        StartCoroutine(MoveDown()); // Start the downward movement coroutine
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "SteppingStone")
        {
            StartCoroutine(OpeningSpeech()); // Start the opening speech coroutine
        }
    }
    
    IEnumerator OpeningSpeech()
    {
        dialogueBoxController.ShowDialogue("character_player", @"Hear me, people of New Athens! I am a witch hunter.
                                                                My job is to expose witches in this time of great suffering and cure towns
                                                                from suffering");
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before continuing
    }
    IEnumerator MoveDown()
    {
        // Move downwards for a set duration
        float startTime = Time.time;
        while (Time.time < startTime + moveDuration)
        {
            rb.MovePosition(rb.position + Vector2.down * speed * Time.deltaTime);
            yield return null;
        }
    }
}
