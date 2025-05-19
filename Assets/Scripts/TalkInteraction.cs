using UnityEngine;

public class TalkInteraction : MonoBehaviour
{
    // [SerializeField] private float interactionRadius = 2f; // Radius to detect NPCs
    private GameObject nearestNPC;
    private bool canInteract = false;

    public GameObject interactBox;
    public DialogueBoxController dialogueBoxController;
    private CircleCollider2D triggerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggerCollider = GetComponent<CircleCollider2D>();
        // Ensure we have a trigger collider for detection
        // CircleCollider2D triggerCollider = gameObject.AddComponent<CircleCollider2D>();
        triggerCollider.isTrigger = true;
        // triggerCollider.radius = interactionRadius;
        // dialogueBoxController = FindObjectOfType<DialogueBoxController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for interaction input (E key)
        if (Input.GetKeyDown(KeyCode.E) && canInteract && nearestNPC != null)
        {
            // TODO: Implement NPC interaction logic here
            Debug.Log("Interacting with NPC!");
            // DialogueController.instance.NewDialogueInstance("Hello there!", "character_nun");
            // dialogueBoxController.ShowDialogue("character_accused", "Why hello there! You must be the new witch hunter in town!");
            canInteract = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            nearestNPC = other.gameObject;
            canInteract = true;

            // Do this manually, putting in update is inefficient and causes errors.
            // interactBox.SetActive(canInteract);

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

            // Do this manually, putting in update is inefficient and causes errors.
            // interactBox.SetActive(canInteract);
        }
    }
}
