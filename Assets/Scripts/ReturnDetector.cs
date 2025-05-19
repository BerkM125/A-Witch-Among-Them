using UnityEngine;
using UnityEngine.UI;

public class ReturnDetector : MonoBehaviour
{
    public GameObject objectiveHandler;
    public Image backdrop;
    public DialogueBoxController dialogueBox;
    public GameObject cluesHolder;
    public GameObject elara;

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
        if (other.gameObject.name == "Player" && om.objectives[2].fulfilled && !om.objectives[3].fulfilled)
        {
            // Progress the objective
            om.ProgressObjective();
            // Show dialogue
            dialogueBox.ShowDialogue("character_player", "Time to go to 'sleep'... heheheheh.");

            // Night time sky
            StartCoroutine(backdrop.GetComponent<BackdropScript>().FadeToNightTime());
            om.ProgressObjective();

            // Activate the evidence! Hide Elara
            cluesHolder.SetActive(true);
            elara.SetActive(false);
        }

        if (other.gameObject.name == "Player" && om.objectives[6].fulfilled && !om.objectives[7].fulfilled)
        {
            // Progress the objective
            om.ProgressObjective();
            // Show dialogue
            dialogueBox.ShowDialogue("character_player", "Better prepare for a great day of WITCHING tomorrow!");

            // Day time sky
            StartCoroutine(backdrop.GetComponent<BackdropScript>().FadeToDayTime());
            om.ProgressObjective();

            // Revert all changes
            cluesHolder.SetActive(false);
            elara.SetActive(true);
        }
    }
}
