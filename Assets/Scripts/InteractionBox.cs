using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionBox : MonoBehaviour
{
    public GameObject interactBox;
    public string message;
    private CircleCollider2D triggerCollider;
    private TextMeshProUGUI dialogueText;
    private GameObject nearestNPC;
    private bool canInteract = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggerCollider = GetComponent<CircleCollider2D>();
        triggerCollider.isTrigger = true; 
        dialogueText = interactBox.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Door opening trigger
    private void OnTriggerEnter2D(Collider2D other) {
        nearestNPC = other.gameObject;
        canInteract = true;
        dialogueText.text = message;

        if (interactBox != null) {
            interactBox.SetActive(canInteract);
        }
    }

    // Door closing trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        nearestNPC = null;
        canInteract = false;
        dialogueText.text = "";

        if (interactBox != null) {
            interactBox.SetActive(canInteract);
        }
    }
}
