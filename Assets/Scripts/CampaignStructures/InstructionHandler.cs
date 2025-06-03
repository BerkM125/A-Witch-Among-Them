using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace InstructionSuite {
    public static class InstructionHandler {
        // Makes a temporary black screen with instructions pop up that can be dismissed by clicking
        public static void CreateInstruction(string instructionText) {
            // Create a new GameObject for the instruction panel
            GameObject instructionPanel = new GameObject("InstructionPanel");
            Canvas canvas = instructionPanel.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Create a panel to hold the instruction text
            GameObject panel = new GameObject("Panel");
            RectTransform panelRect = panel.AddComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(600, 400);
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f); // Center pivot
            panelRect.anchoredPosition = Vector2.zero; // Centered in parent
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.8f); // Semi-transparent black

            // Create a Text component for the instruction text
            GameObject textObject = new GameObject("InstructionText");
            RectTransform textRect = textObject.AddComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(580, 380);
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.pivot = new Vector2(0.5f, 0.5f); // Center pivot
            textRect.anchoredPosition = Vector2.zero; // Centered in parent
            Text instructionTextComponent = textObject.AddComponent<Text>();
            instructionTextComponent.text = instructionText;
            instructionTextComponent.fontSize = 24;
            instructionTextComponent.alignment = TextAnchor.MiddleCenter;
            instructionTextComponent.color = Color.white;

            // Set the parent-child relationship
            textObject.transform.SetParent(panel.transform, false);
            panel.transform.SetParent(instructionPanel.transform, false);

            // Add a button to dismiss the instructions
            Button dismissButton = panel.AddComponent<Button>();
            dismissButton.onClick.AddListener(() => Object.Destroy(instructionPanel));

            // Set the parent of the instruction panel to the main canvas
            instructionPanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
    }
}