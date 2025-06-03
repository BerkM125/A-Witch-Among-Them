using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using LevelManagerNamespace;

public class ClueCollector : MonoBehaviour
{
    public GameObject interactBox;
    bool clueInRange = false;
    public GameObject objectiveHandler;
    private ObjectiveManager om;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        om = objectiveHandler.GetComponent<ObjectiveManager>();
        string filePath = Path.Combine(Application.dataPath, "Scripts/ModelInterface/judge_instructions.json");
        if (File.Exists(filePath))
        {
            try
            {     
                string jsonContent = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(jsonContent);
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && clueInRange) {
            AppendToEvidence(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!om.objectives[5].fulfilled)
            {
                om.ProgressObjective();
            }

            interactBox.SetActive(true);
            clueInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactBox.SetActive(false);
            clueInRange = false;
        }
    }

    public void AppendToEvidence (GameObject evidenceObject) {
        // Add the evidence object to the list of evidence in judge_instructions.json
        string filePath = Path.Combine(Application.dataPath, "Scripts/ModelInterface/judge_instructions.json");
        if (File.Exists(filePath))
        {
            try
            {     
                string jsonContent = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(jsonContent);

                if (jsonObject["prototype"]["evidence"] == null)
                {
                    jsonObject["prototype"]["evidence"] = new JArray();
                }

                JArray evidenceArray = (JArray)jsonObject["prototype"]["evidence"];
                evidenceArray.Add($"{LevelManager.levelTarget} owns a {evidenceObject.name}...");

                jsonObject["prototype"]["evidence"] = evidenceArray;
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
