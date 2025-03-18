using UnityEngine;
using TMPro;
using System;

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
            judgeController.SetCurrentContext("accusedContext");
            judgeController.LoadJudgeInstructions(Instructions.JudgeStatement.DELIVER_AS_DEFENDANT);
            SendPlayerMessage(text);
            canInteract = false;
            inputFieldPrefab.SetActive(false);
        }
    }
}
