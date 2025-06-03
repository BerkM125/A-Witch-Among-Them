using UnityEngine;
using System.Collections;
using LevelManagerNamespace;

public class GeorgeConversation : MonoBehaviour
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
        if (LevelManager.currentLevel != 2) return;

        if (other.gameObject.name == "Player" && !om.objectives[1].fulfilled)
        {
            // Start the conversation with George Heff
            StartCoroutine(BlacksmithDialogueScene());
            // Progress the objective
            om.ProgressObjective();
        }
    }

    private IEnumerator BlacksmithDialogueScene()
    {
        dialogueBox.AddDialogue("george_heff",
            "AHH!!",
            "Oh, sorry... hello traveler... I hope you understand I'm quite shaken from Elara's death yesterday."
        );

        dialogueBox.AddDialogue("character_player", "Oh of course George. But she was a witch. It had to be done.");

        dialogueBox.AddDialogue("george_heff",
            "Of course, of course... but I still can't help but feel sad. She was a good friend.",
            "I know you are on a quest to find the witches, but I must ask you to be careful. They are dangerous and cunning. You must be vigilant."
        );

        dialogueBox.AddDialogue("character_player", "I will be careful, George. I promise. But I must continue my quest to protect this town.",
                                    "Hey, watcha got going on there though?");

        dialogueBox.AddDialogue("george_heff",
            "Oh, this? It's a new FORGERY of mine. It's a device that CAN DETECT MAGIC. I call it the 'Witch Detector'.",
            "I hope to use it to help you in your quest. It should be able to point you in the direction of any witches nearby."
        );

        dialogueBox.AddDialogue("character_player", "Witch Detector you say... you're an amazing blacksmith, George. I appreciate your help.",
                                    "I will use this device to find the witches and protect this town.");

        dialogueBox.AddDialogue("george_heff", "No problem, traveler. I just... I hope we don't have any more... unexpected witches, like Elara, yknow?",
                                    "But anyways, I wanted to give you something else. Here are my house keys. Hop in anytime DURING THE DAY to get MAGIC ORE to fuel this detector device, it runs out fast.");

        dialogueBox.AddDialogue("character_player", "Thanks George! I will be sure to come by and get some magic ore from you.");

        StartCoroutine(dialogueBox.TypeDialogue());

        om.ProgressObjective();

        yield return new WaitForSeconds(1f);
    }
}
