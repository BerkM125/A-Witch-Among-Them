using TMPro;
using UnityEngine;
using CampaignObjects;
using UnityEngine.SceneManagement;
public class ObjectiveManager : MonoBehaviour
{
    public int currentObjectiveIndex = -1; // Index of the current objective
    public GameObject goalOrb;
    public TextMeshProUGUI objectiveLabel;
    public Objective[] objectives = new Objective[9]; // Array of objectives
    private bool objectivesInitialized = false; // Flag to check if objectives are initialized
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectives[0] = new Objective("Find Elara Vex in the bar. Talk to her, collect clues...", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
        objectives[1] = new Objective("Talk to Elara.", new Vector3(147.130005f,38.5900002f,0f));
        objectives[2] = new Objective("Go outside.", new Vector3(147.130005f,38.5900002f,0f));
        objectives[3] = new Objective("Go back home.", new Vector3(57.1100006f,40.9399986f,0f));
        objectives[4] = new Objective("Go to 'bed'.", new Vector3(57.1100006f,40.9399986f,0f));
        objectives[5] = new Objective("Sneak into Elara Vex's house.", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
        objectives[6] = new Objective("Collect suspicious items... in a timely manner!", new Vector3(147.130005f,38.5900002f,0f));
        objectives[7] = new Objective("Run back home.", new Vector3(57.1100006f,40.9399986f,0f));
        objectives[8] = new Objective("GO TO COURT!", new Vector3(102.980003f,56.1100006f,-0.00902594253f));
        objectivesInitialized = true;
        objectiveLabel.text = objectives[0].name; // Update the objective label with the new objective description
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

        if(sceneName != "New Salem Town") {
            return;
        }

        if(currentObjectiveIndex >= 0) {
            objectives[currentObjectiveIndex].fulfilled = true; // Mark the current objective as fulfilled
        }
        currentObjectiveIndex += 1; // Increment the current objective index

        if(currentObjectiveIndex < 9 && objectivesInitialized) {
            goalOrb.transform.position = objectives[currentObjectiveIndex].position; // Move the goal orb to the new objective position
            objectiveLabel.text = objectives[currentObjectiveIndex].name; // Update the objective label with the new objective description
        }
    }
}
