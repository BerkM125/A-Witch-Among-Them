using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InstructionSuite;

public class EndingScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstructionHandler.InitializeInstructions(); // Initialize the instruction handler
        InstructionHandler.globalInstructionPanel.SetActive(false); // Hide the instruction panel at the start
        StartCoroutine(InitiateEndingSequence()); // Start the ending sequence
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InitiateEndingSequence()
    {
       yield return new WaitForSeconds(10f);
       InstructionHandler.CreateInstruction("The inhabitants of New Athens mysteriously disappeared " +
                                            "after the witch trials caused a breakdown in the order of their society. " +
                                            "The town was left in ruins, with only the echoes of the past remaining. " +
                                            "\n\nAnd you?" +
                                            "\n\nYou moved onto the next town, holding the body of Judge Silas Harrow. " +
                                            "\n\nJust another traveler again, seeking to help towns in need of a witch hunter like yourself.");
    }
}
