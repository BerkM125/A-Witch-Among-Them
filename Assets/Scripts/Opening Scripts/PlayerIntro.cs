using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerIntro : MonoBehaviour
{
    public float speed; // Slow speed for downward movement
    private Rigidbody2D rb;
    private float moveDuration = 1.0f; // Duration of movement in seconds
    private float elapsedTime = 0f;
    private Vector3 witchOffset = new Vector3(1.5f, 0.3f, 0.3f); // Offset for the witch's position
    public GameObject witchCorpse;
    public DialogueBoxController dialogueBoxController;
    public Animator animator;
    public Image backdrop;

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
        Debug.Log("Collision detected with: " + other.gameObject.name); // Debug log for collision detection
        if (other.gameObject.name == "character_accused")
        {
            StartCoroutine(OpeningSpeech()); // Start the opening speech coroutine
        }
    }
    
    IEnumerator OpeningSpeech()
    {   
        // Initiate throwing animation
        animator.SetBool("IsThrowing", true);
        yield return new WaitForSeconds(4f);
        animator.SetBool("IsThrowing", false); 

        // Reveal the witch, drop them
        witchCorpse.transform.position = gameObject.transform.position + witchOffset;
        witchCorpse.SetActive(true);
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before continuing

        // Begin monologue
        dialogueBoxController.ShowDialogue("character_player", "Hear me, people of New Athens! I am a witch hunter.");
        yield return new WaitForSeconds(5f); // Wait for 2 seconds before continuing

        dialogueBoxController.ShowDialogue("character_accused", "Traveler... who is that blond haired lady you have just dropped into our town square?");
        yield return new WaitForSeconds(6f); // Wait for 2 seconds before continuing

        dialogueBoxController.ShowDialogue("character_player", "From the last town I came from... this is a real witch.");
        yield return new WaitForSeconds(3f); // Wait for 2 seconds before continuing
        
        dialogueBoxController.ShowDialogue("character_accused", "A witch? You mean to say that this lady is a witch? Good lord...");
        yield return new WaitForSeconds(6f); // Wait for 2 seconds before continuing
        
        dialogueBoxController.ShowDialogue("character_player", "Indeed. People of New Athens, I am a witch hunter. My job is to root out the witches that plague our towns and cure our people from suffering!");
        yield return new WaitForSeconds(7f); // Wait for 2 seconds before continuing
        
        dialogueBoxController.ShowDialogue("character_accused", "Welcome witch hunter! We hope to learn a lot from your ways.");
        yield return new WaitForSeconds(3f); // Wait for 2 seconds before continuing
    
        // Fade out the backdrop
        gameObject.GetComponent<PlayerController>().enabled = true;
        
        StartCoroutine(backdrop.GetComponent<BackdropScript>().FadeToNewScene());    }

    IEnumerator MoveDown()
    {
        //Move downwards for a set duration
        float startTime = Time.time;
        while (Time.time < startTime + moveDuration)
        {
            rb.MovePosition(rb.position + Vector2.down * speed * Time.deltaTime);
            yield return null;
        }
    }
}
