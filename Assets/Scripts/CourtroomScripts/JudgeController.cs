using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using LevelManagerNamespace;
using ModelBridge;
using Instructions;

public class JudgeController : MonoBehaviour
{
    // Public variables for Unity Editor
    public DialogueBoxController dialogueBoxController;
    public GameObject playerPrefab;

    // Will increase with every level.
    public int interactionLimit = 2;

    // Change this based on which character is on trial.
    public string accusedCharacter = "character_accused";
    
    [SerializeField]
    private GameObject player; // Player gameobject in the courtroom.
    
    [SerializeField]
    private GameObject[] accusedCharacters; // Load with ALL possible accused characters on the stand.

    // Private variables for internal use
    PlayerDialogue playerDialogue;
    string judgeInstructions;
    string judgePrompt;
    string currentResponse;
    string currentSpeaker = "judgeContext";
    string currentContext = "judgeContext";
    bool accusedResponded = false;
    int interactionCount = 0;

    // Maps to make speaker and dialogue retrieval easier
    Dictionary<string, string> contextSpeakerMap = new Dictionary<string, string>();
    Dictionary<string, string> contextDialogueMap = new Dictionary<string, string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Establish the accused character and disable the others.
        accusedCharacter = LevelManager.managerLevels[LevelManager.currentLevel - 1].levelTargetID;
        foreach (GameObject character in accusedCharacters) {
            if (character.name == accusedCharacter) {
                character.SetActive(true);
            } else {
                character.SetActive(false);
            }
        }

        // Set interaction limit from level manager
        interactionLimit = LevelManager.managerLevels[LevelManager.currentLevel - 1].interactionLimit;

        // Instantiate player prefabs at the specified position 
        Instantiate(playerPrefab, new Vector3(5.01000023f,-0.699999988f,0f), Quaternion.identity);

        // CLEAR FILE BEFORE ANYTHING
        ClearFile();

        // Initialize context speaker and dialogue maps
        contextSpeakerMap["judgeContext"] = "You, the judge, said";
        contextSpeakerMap["accusedContext"] = "The accused person said";
        contextSpeakerMap["playerContext"] = "The player said";

        contextDialogueMap["judgeContext"] = "character_judge";
        contextDialogueMap["accusedContext"] = accusedCharacter;
        contextDialogueMap["playerContext"] = "character_player";

        // Initialize dialogue sequence and find player object
        player = GameObject.Find("PlayerCourt");
        playerDialogue = player.GetComponent<PlayerDialogue>();

        // Dialogue sequence coroutines must END before another is triggered to avoid feeding the
        // dialogue buffers with multiple dialogues at once through multiple coroutines.
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

    // Set the current context and speaker based on the context
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

            // Determine the type of instruction and set the appropriate variables
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

            // Read the JSON file and parse it
            string jsonContent = File.ReadAllText(filePath);
            JObject jsonObject = JObject.Parse(jsonContent);

            // Prepare the judge instructions and prompt based on the context type
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

            // Add to the judge's context or its "memory"
            judgeInstructions += jsonObject["prototype"][judgementType].ToString();

            // Prepare the judge's context and memory
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
                    LevelManager.playerLost = true;
                    dialogueBoxController.AddDialogue("character_judge", "You have lost loser! The accused is not guilty. AT THIS POINT, CHANGE THE SCENE TO EXECUTION OR YOUR EXECUTION.");
                } else {
                    Debug.Log("YOU HAVE WON!");
                    LevelManager.playerLost = false;
                    dialogueBoxController.AddDialogue("character_judge", "You have won! The accused is guilty. AT THIS POINT, CHANGE THE SCENE TO EXECUTION OR YOUR EXECUTION.");
                }

                // Type out the dialogue!
                StartCoroutine(dialogueBoxController.ShowDialogueExtended((string arg) => {
                    // Change scene to New Athens Town and set currentLevel to 2
                    LevelManager.currentLevel += 1;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ExecutionGround");
                }));

                // Change scene to 
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
