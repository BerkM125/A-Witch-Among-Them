using TMPro;
using UnityEngine;
using CampaignObjects;
using UnityEngine.SceneManagement;
using LevelManagerNamespace;

public class ObjectiveManager : MonoBehaviour
{
    public int currentObjectiveIndex = -1; // Index of the current objective
    public GameObject goalOrb;
    public TextMeshProUGUI objectiveLabel;
    public Objective[] objectives;
    public bool objectivesInitialized = false; // Flag to check if objectives are initialized
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ObjectiveManager started");
        Debug.Log("Current Level: " + LevelManager.currentLevel);
        
        LevelManager.InitializeManagerLevels(); // Initialize the manager levels
        LevelManager.LoadLevel(LevelManager.currentLevel); // Load the current level
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProgressObjective()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Get the name of the scene
        string sceneName = currentScene.name;

        if(sceneName != "New Athens Town") {
            return;
        }

        if(currentObjectiveIndex >= 0) {
            objectives[currentObjectiveIndex].fulfilled = true; // Mark the current objective as fulfilled
        }
        currentObjectiveIndex += 1; // Increment the current objective index

        if(objectivesInitialized) {
            if(currentObjectiveIndex < objectives.Length) {
                goalOrb.transform.position = objectives[currentObjectiveIndex].position; // Move the goal orb to the new objective position
                objectiveLabel.text = objectives[currentObjectiveIndex].name; // Update the objective label with the new objective description
            }
        }
    }
}
