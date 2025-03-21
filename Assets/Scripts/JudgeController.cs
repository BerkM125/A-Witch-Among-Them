using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

using ModelBridge;
using Instructions;

public class JudgeController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private PlayerDialogue playerDialogue;
    string judgeInstructions;
    string judgePrompt;
    string currentResponse;
    string currentSpeaker = "judgeContext";
    string currentContext = "judgeContext";
    bool accusedResponded = false;
    public DialogueBoxController dialogueBoxController;

    Dictionary<string, string> contextSpeakerMap = new Dictionary<string, string>();
    Dictionary<string, string> contextDialogueMap = new Dictionary<string, string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // CLEAR FILE BEFORE ANYTHING
        ClearFile();

        contextSpeakerMap["judgeContext"] = "You, the judge, said";
        contextSpeakerMap["accusedContext"] = "The accused person said";
        contextSpeakerMap["playerContext"] = "The player said";

        contextDialogueMap["judgeContext"] = "character_judge";
        contextDialogueMap["accusedContext"] = "character_accused";
        contextDialogueMap["playerContext"] = "character_player";

        player = GameObject.Find("PlayerCourt");
        playerDialogue = player.GetComponent<PlayerDialogue>();

        dialogueBoxController.ShowDialogue("character_judge", "Give me a minute before I deliver my opening statement...");
        SetCurrentContext("judgeContext");

        // Judge should deliver opening statement upon entry into the court
        LoadJudgeInstructions(Instructions.JudgeStatement.DELIVER_OPENING_STATEMENT);
        StartCoroutine(ModelBridge.Bridge.ChatCompletion("https://api.deepseek.com",
                                                    judgeInstructions,
                                                    judgePrompt,
                                                    ProcessDialogue));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && accusedResponded)
        {
            SendJudgeMessage(currentResponse);
            accusedResponded = false;
        }
    }

    public void SetCurrentContext(string context)
    {
        Debug.Log("Was called");
        currentContext = context;
        currentSpeaker = contextSpeakerMap[context];
    }

    // Send message to the Judge
    public void SendJudgeMessage(string message)
    {
        judgePrompt = message;
        dialogueBoxController.ShowDialogue("character_judge", "Let me think...");
        StartCoroutine(ModelBridge.Bridge.ChatCompletion("https://api.deepseek.com",
                                                    judgeInstructions,
                                                    judgePrompt,
                                                    ProcessDialogue));
    }


    // Load judge instructions and prompt from JSON file
    public void LoadJudgeInstructions(int instructionType)
    {
        string filePath = Path.Combine(Application.dataPath, "Scripts/ModelInterface/judge_instructions.json");
        if (File.Exists(filePath))
        {
            string judgementType = "";
            string promptType = "";
            string contextType = "";

            switch (instructionType) {
                case Instructions.JudgeStatement.DELIVER_JUDGMENT:
                    judgementType = "judgmentInstructions";
                    promptType = "judgmentPrompt";
                    contextType = "judgeContext";
                    break;
                case Instructions.JudgeStatement.DELIVER_OPENING_STATEMENT:
                    judgementType = "openingInstructions";
                    promptType = "openingPrompt";
                    contextType = "judgeContext";
                    break;
                case Instructions.JudgeStatement.DELIVER_FINAL_INSTRUCTIONS:
                    judgementType = "finalInstructions";
                    promptType = "finalPrompt";
                    contextType = "judgeContext";
                    break;
                case Instructions.JudgeStatement.DELIVER_AS_DEFENDANT:
                    judgementType = "accusedInstructions";
                    promptType = "accusedPrompt";
                    contextType = "accusedContext";
                    break;
                case Instructions.JudgeStatement.CONVERSE_WITH_PLAYER:
                    judgementType = "conversePlayerInstructions";
                    promptType = "conversePlayerPrompt";
                    contextType = "judgeContext";
                    break;
                case Instructions.JudgeStatement.CONVERSE_WITH_DEFENDANT:
                    judgementType = "converseDefendantInstructions";
                    promptType = "converseDefendantPrompt";
                    contextType = "judgeContext";
                    break;
            }

            string jsonContent = File.ReadAllText(filePath);
            JObject jsonObject = JObject.Parse(jsonContent);

            judgeInstructions = jsonObject["prototype"][judgementType].ToString();
            judgePrompt = jsonObject["prototype"][promptType].ToString() + $@"
                Use the context below to aid your speech: 
                {jsonObject["prototype"][contextType]}";

            if (contextType == "judgeContext") {
                judgePrompt += $@"If the prosecutor/the player mentions ANY of the following evidence, 
                                    deliver a SWIFT, GUILTY VERDICT TO THE ACCUSED WITH NO MERCY: ";
                foreach (var evidence in jsonObject["prototype"]["evidence"])
                {
                    judgePrompt += $@"{evidence}, ";
                }
            }   
        }
        else
        {
            Debug.LogError("Judge instructions file not found.");
        }
    }

    // Callback to dialogue processing
    void ProcessDialogue(string response)
    {
        dialogueBoxController.ShowDialogue(contextDialogueMap[currentContext], response);
        playerDialogue.EnableChat();

        // Open the judge_instructions.json file and prepare it for writing
        string filePath = Path.Combine(Application.dataPath, "Scripts/ModelInterface/judge_instructions.json");
        if (File.Exists(filePath))
        {
            try
            {     
                string jsonContent = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(jsonContent);
                jsonObject["prototype"]["judgeContext"] += $@"\n{contextSpeakerMap[currentContext]} said: {response}, ";
                jsonObject["prototype"]["accusedContext"] += $@"\n{contextSpeakerMap[currentContext]} said: {response}, ";

                string jsonFileContent = jsonObject.ToString();
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.Write(jsonFileContent);
                }
            }
            catch (IOException ex)
            {
                Debug.LogError("Error opening judge_instructions.json for writing: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Judge instructions file not found.");
        }

        // Make sure that if the accused is currently delivering speech, it is relayed to the judge.
        if(currentContext == "accusedContext")
        {
            SetCurrentContext("judgeContext");
            LoadJudgeInstructions(Instructions.JudgeStatement.CONVERSE_WITH_PLAYER);
            currentResponse = response;
            accusedResponded = true;
        }
    }


    // Init clear file
    void ClearFile()
    {
        string filePath = Path.Combine(Application.dataPath, "Scripts/ModelInterface/judge_instructions.json");
        if (File.Exists(filePath))
        {
            try
            {     
                string jsonContent = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(jsonContent);
                jsonObject["prototype"]["judgeContext"] = "";
                jsonObject["prototype"]["accusedContext"] = "";
                jsonObject["prototype"]["playerContext"] = "";
                jsonObject["prototype"]["evidence"] = new JArray();

                string jsonFileContent = jsonObject.ToString();
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.Write(jsonFileContent);
                }
            }
            catch (IOException ex)
            {
                Debug.LogError("Error opening judge_instructions.json for writing: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Judge instructions file not found.");
        }
    }
}
