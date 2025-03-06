using UnityEngine;

public class TalkInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 2f; // Radius to detect NPCs
    private GameObject nearestNPC;
    private bool canInteract = false;

    public GameObject dialogueBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure we have a trigger collider for detection
        CircleCollider2D triggerCollider = gameObject.AddComponent<CircleCollider2D>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = interactionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for interaction input (E key)
        if (Input.GetKeyDown(KeyCode.E) && canInteract && nearestNPC != null)
        {
            // TODO: Implement NPC interaction logic here
            Debug.Log("Interacting with NPC!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        dialogueBox.SetActive(true);
        if (other.CompareTag("NPC"))
        {
            nearestNPC = other.gameObject;
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        dialogueBox.SetActive(false);
        if (other.CompareTag("NPC"))
        {
            nearestNPC = null;
            canInteract = false;
        }
    }
}
