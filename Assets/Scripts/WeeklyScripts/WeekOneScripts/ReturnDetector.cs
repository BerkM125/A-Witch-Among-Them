using UnityEngine;
using UnityEngine.UI;
using LevelManagerNamespace;

public class ReturnDetector : MonoBehaviour
{
    public GameObject objectiveHandler;
    public Image backdrop;
    public DialogueBoxController dialogueBox;
    public GameObject cluesHolder;

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

    private void LockUnlockPlayer(GameObject other)
    {
        other.gameObject.GetComponent<PlayerController>().playerLocked = !other.gameObject.GetComponent<PlayerController>().playerLocked;
    }
    // Upon collision with the player, progress objective
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LevelManager.currentLevel > 4) return;

        cluesHolder = (LevelManager.managerLevels[LevelManager.currentLevel-1].cluesHolder);
        Debug.Log("Clues holder: " + cluesHolder.name);

        Debug.Log("collision occurring with " + other.gameObject.name);
        if (other.gameObject.name == "Player" && om.objectives[2].fulfilled && !om.objectives[3].fulfilled)
        {
            // Progress the objective
            om.ProgressObjective();
            // Show dialogue
            dialogueBox.AddDialogue("character_player", "Time to go to 'sleep'... heheheheh.");
            StartCoroutine(dialogueBox.TypeDialogue()); // Show the dialogue and change scene

            // Night time sky
            StartCoroutine(backdrop.GetComponent<BackdropScript>().FadeToNightTime());
            om.ProgressObjective();

            // Activate the evidence! Hide relevant characters
            cluesHolder.SetActive(true);
            GameObject.Find(LevelManager.managerLevels[LevelManager.currentLevel-1].levelTargetID).SetActive(false);
        }

        if (other.gameObject.name == "Player" && om.objectives[6].fulfilled && !om.objectives[7].fulfilled)
        {
            // Progress the objective
            om.ProgressObjective();
            // Show dialogue
            dialogueBox.AddDialogue("character_player", "Better prepare for a great day of WITCHING tomorrow!");
            StartCoroutine(dialogueBox.TypeDialogue()); // Show the dialogue and change scene

            // Day time sky
            StartCoroutine(backdrop.GetComponent<BackdropScript>().FadeToDayTime());
            om.ProgressObjective();

            // Revert all changes
            cluesHolder.SetActive(false);

            // THIS WONT WORK becuase player is already inactive!
            //GameObject.Find(LevelManager.managerLevels[LevelManager.currentLevel-1].levelTargetID).SetActive(true);
        }
    }
}
