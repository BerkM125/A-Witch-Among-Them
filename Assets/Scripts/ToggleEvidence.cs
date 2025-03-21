using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;

public class ToggleEvidence : MonoBehaviour
{
    public GameObject evidenceList;
    public GameObject evidenceTextBox;
    bool visible = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipEvidence () {
        evidenceList.SetActive(!visible);
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
