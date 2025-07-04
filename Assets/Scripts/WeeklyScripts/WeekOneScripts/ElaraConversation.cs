using UnityEngine;
using System.Collections;
using LevelManagerNamespace;

public class ElaraConversation : MonoBehaviour
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
        if (LevelManager.currentLevel != 1) return;

        if (other.gameObject.name == "Player" && !om.objectives[1].fulfilled)
        {
            // Start the conversation with Elara
            StartCoroutine(BarDialogueSequence());
            // Progress the objective
            om.ProgressObjective();
        }
    }

    private IEnumerator BarDialogueSequence()
    {
        dialogueBox.AddDialogue("character_accused",
            "Oh hello traveler! What brings you to the bar?",
            "Since this is a playtest, I'm just gonna take over this conversation for now.",
            "Anyways, grab a drink if you'd like. I sure have been DANCING WITH THE DEVIL coming here too often recently, heh.",
            "Yeah... ever since my husband, REVEREND PARRISH, passed I've been a little sad... Picked up a few BAD HABITS and STRANGE HOBBIES, too.",
            "Stuff like KNITTING POPPETS, SWEEPING THE FLOOR WITH A BROOM, and even... uh... you know what? Never mind that last one. It's a little too personal.",
            "Ah screw it. I've been reading more! Yes I know that's taboo here... but THIS BOOK IS VERY INTERESTING... trust me.",
            "I think it has something to do with the WITCHES in this town. Hey, you said you're hunting them? Good luck with that good sir!"
        );

        dialogueBox.AddDialogue("character_player", "Thanks Elara! I'll be looking carefully... you've been such a great friend so far.");

        StartCoroutine(dialogueBox.TypeDialogue());

        om.ProgressObjective();

        yield return new WaitForSeconds(1f);
    }
}
