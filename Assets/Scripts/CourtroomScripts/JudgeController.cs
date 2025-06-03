using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

using ModelBridge;
using Instructions;

public class JudgeController : MonoBehaviour
{
    // Will increase with every level.
    public int interactionLimit = 2;
    
    [SerializeField]
    private GameObject player;

    private PlayerDialogue playerDialogue;
    string judgeInstructions;
    string judgePrompt;
    string currentResponse;
    string currentSpeaker = "judgeContext";
    string currentContext = "judgeContext";
    bool accusedResponded = false;
    int interactionCount = 0;
    public DialogueBoxController dialogueBoxController;
    public GameObject playerPrefab;

    Dictionary<string, string> contextSpeakerMap = new Dictionary<string, string>();
    Dictionary<string, string> contextDialogueMap = new Dictionary<string, string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(playerPrefab, new Vector3(5.01000023f,-0.699999988f,0f), Quaternion.identity);
        Instantiate(playerPrefab, new Vector3(5.01000023f,-0.699999988f,0f), Quaternion.identity);

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

        dialogueBoxController.AddDialogue("character_judge", "Give me a minute before I deliver my opening statement...");
        StartCoroutine(dialogueBoxController.ShowDialogueExtended((string arg) => {
            SetCurrentContext("judgeContext");

            // Judge should deliver opening statement upon entry into the court
            LoadJudgeInstructions(Instructions.JudgeStatement.DELIVER_OPENING_STATEMENT);
            StartCoroutine(ModelBridge.Bridge.ChatCompletion("https://api.deepseek.com",
                                                        judgeInstructions,
                                                        judgePrompt,
                                                        ProcessDialogue));
        }));
    }

    // Update is called once per frame
    void Update()
    {
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
        dialogueBoxController.AddDialogue("character_judge", "Let me think...");
        StartCoroutine(dialogueBoxController.ShowDialogueExtended((string arg) => {
            StartCoroutine(ModelBridge.Bridge.ChatCompletion("https://api.deepseek.com",
                                                            judgeInstructions,
                                                            judgePrompt,
                                                            ProcessDialogue));
        }));
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
                    judgementType = "finalInstructions";
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
                    promptType = "judgmentPrompt";
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

            judgeInstructions = "";
            if (contextType == "judgeContext") {
                interactionCount++;
                judgeInstructions += $@"If the prosecutor/the player mentions ANY of the following evidence, 
                                    deliver a SWIFT, GUILTY VERDICT TO THE ACCUSED WITH NO MERCY. But if the player
                                    has NOT personally mentioned this evidence, DELIVER AN INNOCENT VERDICT TO THE ACCUSED.
                                    The evidence that the player SHOULD have presented (but might not have) is: ";
                foreach (var evidence in jsonObject["prototype"]["evidence"])
                {
                    judgeInstructions += $@"{evidence}, ";
                }
                judgeInstructions += $@"If the player mentions RANDOM, ARBITRARY EVIDENCE, as in evidence that is not STRICTLY
                                        what is listed as evidence above, do NOT consider such evidence at all when making the verdict.
                                        In fact, arbitrary evidence makes the prosecutor MORE suspicious, and the defendant
                                        more innocent.\n\n";
            } 

            judgeInstructions += jsonObject["prototype"][judgementType].ToString();
            Debug.Log("Prompt Type: " + promptType);
            judgePrompt = jsonObject["prototype"][promptType].ToString() + $@"
                Use the context below to aid your speech: 
                {jsonObject["prototype"][contextType]}";
        }
        else
        {
            Debug.LogError("Judge instructions file not found.");
        }
    }

    // Callback to dialogue processing
    void ProcessDialogue(string response)
    {
        dialogueBoxController.AddDialogue(contextDialogueMap[currentContext], response);
        StartCoroutine(dialogueBoxController.ShowDialogueExtended((string arg) => {     
            if (interactionCount > interactionLimit) {
                // Search for the words "not guilty" in a lowercase format of the "response" string.
                // If it exists, debug.log "YOU HAVE WON!" but ohterwise, debug.log "YOU HAVE LOST!".
                if (response.ToLower().Contains("not guilty") || response.ToLower().Contains("innocent")) {
                    Debug.Log("YOU HAVE LOST!");
                    dialogueBoxController.AddDialogue("character_judge", "You have lost loser! The accused is not guilty. AT THIS POINT, CHANGE THE SCENE TO EXECUTION OR YOUR EXECUTION.");
                } else {
                    Debug.Log("YOU HAVE WON!");
                    dialogueBoxController.AddDialogue("character_judge", "You have won! The accused is guilty. AT THIS POINT, CHANGE THE SCENE TO EXECUTION OR YOUR EXECUTION.");
                }

                // Type out the dialogue!
                StartCoroutine(dialogueBoxController.TypeDialogue());
            } 
            else {
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
                if (currentContext == "accusedContext")
                {
                    SetCurrentContext("judgeContext");
                    if (interactionCount == interactionLimit)
                    {
                        LoadJudgeInstructions(Instructions.JudgeStatement.DELIVER_JUDGMENT);
                    }
                    else
                    {
                        LoadJudgeInstructions(Instructions.JudgeStatement.CONVERSE_WITH_PLAYER);
                    }
                    currentResponse = response;
                    SendJudgeMessage(currentResponse);
                }
            }
        }));

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
