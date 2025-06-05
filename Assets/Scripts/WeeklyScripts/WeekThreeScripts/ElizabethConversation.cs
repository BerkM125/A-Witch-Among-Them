using UnityEngine;
using System.Collections;
using LevelManagerNamespace;

public class ElizabethConversation : MonoBehaviour
{
    public GameObject ObjectiveHandler;
    public DialogueBoxController dialogueBox;
    private ObjectiveManager om;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        om = ObjectiveHandler.GetComponent<ObjectiveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Upon collision with the player, start conversation and ProgressObjective
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Ensure week is still at ONE
        if (LevelManager.currentLevel != 3) return;

        if (other.gameObject.name == "Player" && !om.objectives[1].fulfilled)
        {
            // Start the conversation with George Heff
            StartCoroutine(TeacherDialogueScene());
            // Progress the objective
            om.ProgressObjective();
        }
    }

    private IEnumerator TeacherDialogueScene()
    {
        dialogueBox.AddDialogue("elizabeth_wallowmore",
            "Traveler... is that you? Why have you shown up to the doorstep of my sacred school?"
        );

        dialogueBox.AddDialogue("character_player", "Gee Ms. Wallowmore, I think we've gotten off on the wrong foot!");

        dialogueBox.AddDialogue("elizabeth_wallowmore",
            "I think not. I have watched you in court. It frightens me how easily you manipulate the law to accuse our townspeople of witchcraft. Even if they are witches, I believe due process should not be infringed to achieve such means, don't you?"
        );      

        dialogueBox.AddDialogue("character_player", "My job is to hunt witches. There is no more important task than that. Due process IS happening, and justice IS being served.");

        dialogueBox.AddDialogue("elizabeth_wallowmore",
            "I teach justice from history all the time, traveler. Even my students can understand from the HISTORY BOOKS in their class that due process is necessary."
        ); 
        dialogueBox.AddDialogue("elizabeth_wallowmore", "Honestly, I'm done speaking with you. Your soul is about as rotten as THE APPLE THAT HAS BEEN SITTING ON MY DESK FOR THE PAST WEEK. I do not wish you luck with your quest.");
        
        StartCoroutine(dialogueBox.TypeDialogue());

        om.ProgressObjective();

        yield return new WaitForSeconds(1f);
    }
}
