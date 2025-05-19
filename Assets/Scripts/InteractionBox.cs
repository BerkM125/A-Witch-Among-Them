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

    private void OnTriggerEnter2D(Collider2D other) {
            nearestNPC = other.gameObject;
            canInteract = true;
            dialogueText.text = message;
            interactBox.SetActive(canInteract);
            Debug.Log("Opening door!!!");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
            nearestNPC = null;
            canInteract = false;
            dialogueText.text = "";
            // Do this manually, putting in update is inefficient and causes errors.
            interactBox.SetActive(canInteract);
            Debug.Log("Closing door!!!");
    }
}
