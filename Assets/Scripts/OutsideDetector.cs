using UnityEngine;
using UnityEngine.UI;

public class OutsideDetector : MonoBehaviour
{
    public GameObject objectiveHandler;
    public DialogueBoxController dialogueBox;
    public Image backdrop;
    private ObjectiveManager om;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        om = objectiveHandler.GetComponent<ObjectiveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Upon collision with the player, progress objective
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision occurring with " + other.gameObject.name);
        if (other.gameObject.name == "Player" && om.objectives[1].fulfilled && !om.objectives[2].fulfilled)
        {
            // Progress the objective
            om.ProgressObjective();
            // Hazen the sky
            backdrop.color = new Color(0.3886792f, 0.1338908f, 0.05500172f, 0.4f);

            Debug.Log("Should be heading home...");
            // Show dialogue
            dialogueBox.AddDialogue("character_player", "I should head home... it's getting quite late.");
            StartCoroutine(dialogueBox.ShowDialogue()); // Show the dialogue and change scene
            
        }

        if (other.gameObject.name == "Player" && om.objectives[5].fulfilled && !om.objectives[6].fulfilled)
        {
            // Progress the objective
            om.ProgressObjective();

            // Show dialogue
            dialogueBox.AddDialogue("character_player", "Better head home before the sun comes up!");
            StartCoroutine(dialogueBox.ShowDialogue()); // Show the dialogue and change scene
        }
    }
}
