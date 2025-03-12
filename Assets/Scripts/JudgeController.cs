using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;

using ModelBridge;
using Instructions;

public class JudgeController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private PlayerDialogue playerDialogue;
    string judgeInstructions;
    string judgePrompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        playerDialogue = player.GetComponent<PlayerDialogue>();

        DialogueController.instance.NewDialogueInstance("Give me a minute before I give my opening statement...", "character_judge");

        // Judge should deliver opening statement upon entry into the court
        LoadJudgeInstructions(Instructions.JudgeStatement.DELIVER_OPENING_STATEMENT);
        StartCoroutine(ModelBridge.Bridge.ChatCompletion("http://127.0.0.1:8080",
                                                    judgeInstructions,
                                                    judgePrompt,
                                                    ProcessDialogue));

        // Prime judge for actual judging, afterward
        LoadJudgeInstructions(Instructions.JudgeStatement.DELIVER_JUDGMENT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Send message to the Judge
    public void SendJudgeMessage(string message)
    {
        judgePrompt = message;
        DialogueController.instance.NewDialogueInstance("Let me think...", "character_judge");
        StartCoroutine(ModelBridge.Bridge.ChatCompletion("http://127.0.0.1:8080",
                                                    judgeInstructions,
                                                    judgePrompt,
                                                    ProcessDialogue));
    }


    // Load judge instructions and prompt from JSON file
    void LoadJudgeInstructions(int instructionType)
    {
        string filePath = Path.Combine(Application.dataPath, "Scripts/ModelInterface/judge_instructions.json");
        if (File.Exists(filePath))
        {
            string judgementType = "";
            string promptType = "";

            switch (instructionType) {
                case Instructions.JudgeStatement.DELIVER_JUDGMENT:
                    judgementType = "judgmentInstructions";
                    promptType = "judgmentPrompt";
                    break;
                case Instructions.JudgeStatement.DELIVER_OPENING_STATEMENT:
                    judgementType = "openingInstructions";
                    promptType = "openingPrompt";
                    break;
            }

            string jsonContent = File.ReadAllText(filePath);
            JObject jsonObject = JObject.Parse(jsonContent);

            judgeInstructions = jsonObject["prototype"][judgementType].ToString();
            judgePrompt = jsonObject["prototype"][promptType].ToString();
        }
        else
        {
            Debug.LogError("Judge instructions file not found.");
        }
    }

    // Callback to dialogue processing
    void ProcessDialogue(string response)
    {
        DialogueController.instance.NewDialogueInstance(response, "character_judge");
        playerDialogue.EnableChat();
    }
}
