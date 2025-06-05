using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace InstructionSuite {
    public static class InstructionHandler {
        public static GameObject globalInstructionPanel;
        public static GameObject globalSubPanel;
        public static TextMeshProUGUI globalText;

        // Initialize the global variables, call this function before the panels are disabled though.
        public static void InitializeInstructions() {
            // Initialize the instruction panel here
            globalInstructionPanel = GameObject.Find("InstructionPanel");
            globalSubPanel = GameObject.Find("InstructionSubPanel");
            globalText = GameObject.Find("InstructionText").GetComponent<TextMeshProUGUI>();
        }

        // Makes a temporary black screen with instructions pop up that can be dismissed by clicking
        public static void CreateInstruction(string instructionText) {
            globalInstructionPanel.SetActive(true);
            globalSubPanel.SetActive(true);
            
            // Use the global panels and text to achieve desired effect
            if (globalInstructionPanel == null || globalSubPanel == null || globalText == null) {
                Debug.LogError("Instruction panel or sub-panel or text not found in the scene.");
                return;
            }

            globalInstructionPanel.SetActive(true);
            globalSubPanel.SetActive(true);
            globalText.text = instructionText;

        }
    }
}