using UnityEngine;

public class TalkInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 2f; // Radius to detect NPCs
    private GameObject nearestNPC;
    private bool canInteract = false;

    public GameObject interactBox;
    public DialogueBoxController dialogueBoxController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure we have a trigger collider for detection
        CircleCollider2D triggerCollider = gameObject.AddComponent<CircleCollider2D>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = interactionRadius;
        // dialogueBoxController = FindObjectOfType<DialogueBoxController>();
    }

    // Update is called once per frame
    void Update()
    {
        interactBox.SetActive(canInteract);
        // Check for interaction input (E key)
        if (Input.GetKeyDown(KeyCode.E) && canInteract && nearestNPC != null)
        {
            // TODO: Implement NPC interaction logic here
            Debug.Log("Interacting with NPC!");
<<<<<<< HEAD
            DialogueController.instance.NewDialogueInstance("Why, hello there! You must be the new witch hunter in town!", nearestNPC.gameObject.name);
=======
            // DialogueController.instance.NewDialogueInstance("Hello there!", "character_nun");
            dialogueBoxController.ShowDialogue("character_accused", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.");
>>>>>>> 32709939f82c86e23c401b243ca8eef285709bf2
            canInteract = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            nearestNPC = other.gameObject;
            canInteract = true;

            // Make the NPC face the player
            Vector3 direction = transform.position - nearestNPC.transform.position;
            SpriteRenderer spriteRenderer = nearestNPC.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            nearestNPC = null;
            canInteract = false;
        }
    }
}
