using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using System;

public class ToggleEvidence : MonoBehaviour
{
    public GameObject evidenceList;
    public GameObject evidenceTextBox;
    bool visible = false;
    bool glowState = true;
    bool growing = true;
    float scaleSpeed = 2.0f;
    Vector2 shrinkTarget = new Vector2(130f, 93f);
    Vector2 growTarget = new Vector2(150f, 102f);
    Vector2 originalSize = new Vector2(140f, 97f);
    RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (glowState)
        {
            Vector2 targetSize = growing ? growTarget : shrinkTarget;
            rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, targetSize, Time.deltaTime * scaleSpeed);
            
            if ((rectTransform.rect.width-targetSize.x) <= 1f && Math.Abs(rectTransform.rect.height-targetSize.y) <= 1f)
            {
                Debug.Log("Reached target size: " + targetSize);
                growing = !growing;
            }
        }
    }

    public void FlipEvidence () {
        rectTransform.sizeDelta = originalSize; // Reset size to original before toggling visibility
        glowState = false; // Stop glowing when toggling visibility
        
        // Toggle the visibility of the evidence list
        evidenceList.SetActive(!visible);
        Debug.Log("Visible? " + visible);

        visible = !visible;

        if(visible) {
            // Populate the textbox given in evidenceTextBox with al the evidence in judgeInstructions.json
            string filePath = Path.Combine(Application.dataPath, "Scripts/ModelInterface/judge_instructions.json");
            if (File.Exists(filePath))
            {
                try
                {     
                    string jsonContent = File.ReadAllText(filePath);
                    JObject jsonObject = JObject.Parse(jsonContent);

                    // Append all evidence into a line-by-line simple string 
                    string evidence = "";
                    foreach (var item in jsonObject["prototype"]["evidence"])
                    {
                        evidence += item + "\n";
                    }
                    evidenceTextBox.GetComponent<TextMeshProUGUI>().text = evidence;
                }
                catch (IOException ex)
                {
                    Debug.LogError("Error opening judge_instructions.json for reading: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("Judge instructions file not found.");
            }
        }
    }
}
