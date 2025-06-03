using UnityEngine;
using CampaignObjects;
using System.Collections;
using System.Collections.Generic;

namespace LevelManagerNamespace
{
    // This class manages the levels in the game, including loading levels and managing objectives.
    // It will also handle the interaction limits and door unlocking.
    public static class LevelManager
    {
        public static int currentLevel = 1;
        public static string levelTarget = "Elara Vex"; // The target character for the current level
        public static CampaignObjects.LevelContainer[] managerLevels = new CampaignObjects.LevelContainer[3]; // Assuming 3 levels for now

        public static void InitializeManagerLevels () {

        }

        public static void LoadLevelOne() {
            currentLevel = 1;
            levelTarget = "Elara Vex"; // The target character for the current level
            Objective[] objectives = new Objective[9]; 
            objectives[0] = new Objective("Find Elara Vex in the bar. Talk to her, collect clues...", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
            objectives[1] = new Objective("Talk to Elara.", new Vector3(147.130005f,38.5900002f,0f));
            objectives[2] = new Objective("Go outside.", new Vector3(147.130005f,38.5900002f,0f));
            objectives[3] = new Objective("Go back home.", new Vector3(57.1100006f,40.9399986f,0f));
            objectives[4] = new Objective("Go to 'bed'.", new Vector3(57.1100006f,40.9399986f,0f));
            objectives[5] = new Objective("Sneak into Elara Vex's house.", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
            objectives[6] = new Objective("Collect suspicious items... in a timely manner!", new Vector3(147.130005f,38.5900002f,0f));
            objectives[7] = new Objective("Run back home.", new Vector3(57.1100006f,40.9399986f,0f));
            objectives[8] = new Objective("GO TO COURT!", new Vector3(102.980003f,56.1100006f,-0.00902594253f));

            ObjectiveManager om = GameObject.Find("ObjectiveHandler").GetComponent<ObjectiveManager>();
            om.objectiveLabel.text = objectives[0].name; // Update the objective label with the new objective description
            om.objectives = objectives; // Assign the objectives to the ObjectiveManager
            
            Debug.Log("om.objectives length: " + om.objectives.Length);
            om.objectivesInitialized = true;
        }

        public static void LoadCourtroomOne() {

        }

        // Introduces the plot with the blacksmith and the discussions over Elara's death and such, etc etc.
        // Make sure to unlock more doors, include new evidence, increase interaction limit for court
        public static void LoadLevelTwo() {
            currentLevel = 2;
            levelTarget = "George Heff"; // The target character for the current level
            // Get rid of Elara Vex first.
            UnityEngine.Object.Destroy(GameObject.Find("character_accused"));

            Objective[] objectives = new Objective[9]; 
            objectives[0] = new Objective("Find blacksmith George Heff in the bar. Talk to him, collect clues...", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
            objectives[1] = new Objective("Talk to George.", new Vector3(147.130005f,38.5900002f,0f));
            objectives[2] = new Objective("Go outside.", new Vector3(147.130005f,38.5900002f,0f));
            objectives[3] = new Objective("Go back home.", new Vector3(57.1100006f,40.9399986f,0f));
            objectives[4] = new Objective("Go to 'bed'.", new Vector3(57.1100006f,40.9399986f,0f));
            objectives[5] = new Objective("Sneak into George Heff's house.", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
            objectives[6] = new Objective("Collect suspicious items... in a timely manner!", new Vector3(147.130005f,38.5900002f,0f));
            objectives[7] = new Objective("Run back home.", new Vector3(57.1100006f,40.9399986f,0f));
            objectives[8] = new Objective("GO TO COURT!", new Vector3(102.980003f,56.1100006f,-0.00902594253f));

            ObjectiveManager om = GameObject.Find("ObjectiveHandler").GetComponent<ObjectiveManager>();
            om.objectiveLabel.text = objectives[0].name; // Update the objective label with the new objective description
            om.objectives = objectives; // Assign the objectives to the ObjectiveManager
            
            Debug.Log("om.objectives length: " + om.objectives.Length);
            om.objectivesInitialized = true;
        }

        public static void LoadCourtroomTwo() {

        }

        public static void LoadLevelThree() {

        }
    }
}

