using TMPro;
using UnityEngine;
using CampaignObjects;
using UnityEngine.SceneManagement;
using LevelManagerNamespace;
using InstructionSuite;

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

        InstructionHandler.InitializeInstructions(); // Initialize the instruction handler

        if (LevelManager.currentLevel == 1) {
            InstructionHandler.CreateInstruction($@"Pssst... you don't need to know who I am. Just know that our interests are currently aligned... across worlds. Your job in this town is to accuse every single important official of witchcraft and get them executed in a court of law. The due process of witch trial shall keep your true identity, a REAL WITCH, hidden from the town of New Athens. Good luck with your first target... she should be a real easy one...");    
        }
        else if (LevelManager.currentLevel == 2) {
            InstructionHandler.CreateInstruction($@"Good... \n\nVery, very good...");    
        }
        else if (LevelManager.currentLevel == 3) {
            InstructionHandler.CreateInstruction($@"Good... \n\nVery, very good...");    
        }
        else if (LevelManager.currentLevel == 4) {
            InstructionHandler.CreateInstruction($@"Good... \n\nVery, very good...");    
        }

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
