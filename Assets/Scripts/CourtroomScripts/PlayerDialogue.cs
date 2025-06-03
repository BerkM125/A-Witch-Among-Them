using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject judgeNPC;
    private JudgeController judgeController;
    public bool canInteract = false;
    public GameObject inputFieldPrefab;
    private TMP_InputField inputField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        judgeNPC = GameObject.Find("Judge");
        judgeController = judgeNPC.GetComponent<JudgeController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Send the message made by the player to Judge
    public void SendPlayerMessage(string message)
    {
        // Open the judge_instructions.json file and prepare it for writing
        string filePath = Path.Combine(Application.dataPath, "Scripts/ModelInterface/judge_instructions.json");
        if (File.Exists(filePath))
        {
            try
            {     
                string jsonContent = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(jsonContent);
                jsonObject["prototype"]["judgeContext"] += $@"\nThe accuser, the player, just said: {message}, ";
                jsonObject["prototype"]["accusedContext"] += $@"\nYour accuser, the player, just said: {message}, ";

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
        judgeController.SendJudgeMessage(message);
    }

    // When this is called, dialogue for player should be initiated.
    public void EnableChat()
    {
        canInteract = true;
        OpenInputField();
    }

    // Opens input field for player interaction
    private void OpenInputField()
    {
        inputFieldPrefab.SetActive(true);
        Debug.Log("should be active");
        inputField = inputFieldPrefab.GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(OnEndEdit);
        inputField.ActivateInputField();
    }

    private void OnEndEdit(string text)
    {
        // Upon entry, invoke the SendPlayerMessage event with the text input
        if (Input.GetKeyDown(KeyCode.Return) && canInteract)
        {
            Debug.Log("should be invoking..");
            judgeController.SetCurrentContext("accusedContext");
            judgeController.LoadJudgeInstructions(Instructions.JudgeStatement.DELIVER_AS_DEFENDANT);
            SendPlayerMessage(text);
            canInteract = false;
            inputFieldPrefab.SetActive(false);
        }
    }
}
