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
        public static bool playerLost = false;
        public static int currentLevel = 1;
        public static CampaignObjects.LevelContainer[] managerLevels = new CampaignObjects.LevelContainer[4]; // Assuming 3 levels for now

        public static void InitializeManagerLevels () {
            // Initialize the manager levels with the level containers
            managerLevels[0] = new CampaignObjects.LevelContainer("Elara Vex", "character_accused", 1, 2, "level1", "Clues");
            managerLevels[1] = new CampaignObjects.LevelContainer("George Heff", "george_heff", 2, 3, "level2", "Clues2");
            managerLevels[2] = new CampaignObjects.LevelContainer("Elizabeth Wallowmore", "elizabeth_wallowmore", 3, 3, "level3", "Clues3");
            managerLevels[3] = new CampaignObjects.LevelContainer("Benjamin Marderson", "benjamin_marderson", 4, 3, "level4", "Clues4");
        }

        // Load the levels, level resources, objectives
        public static void LoadLevel(int levelNumber) {
            currentLevel = levelNumber;
            CleanupObjects(levelNumber);
            LoadObjectives(levelNumber);
        }

        // Helper for clearing the scene of unnecessary objects
        static void CleanupObjects(int levelNumber) {
            // Find and destroy the character_accused object if it exists
            GameObject characterAccused = GameObject.Find("character_accused");
            if (characterAccused != null && levelNumber > 1) {
                UnityEngine.Object.Destroy(characterAccused);
            } 
            if (levelNumber > 2) {
                UnityEngine.Object.Destroy(GameObject.Find("george_heff"));
            }
            if (levelNumber > 3) {
                UnityEngine.Object.Destroy(GameObject.Find("elizabeth_wallowmore"));
            }  
            if (levelNumber > 4) {
                UnityEngine.Object.Destroy(GameObject.Find("benjamin_marderson"));
            }
        }

        // Helper for loading objecives by level number
        static void LoadObjectives(int levelNumber) {
            // Initialize objectives
            Objective[] objectives = new Objective[9]; 

            // Load objectives based on the level number
            switch (levelNumber) {
                case 0x01:
                    objectives[0] = new Objective("Find Elara Vex in the bar. Talk to her, collect clues...", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
                    objectives[1] = new Objective("Talk to Elara.", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[2] = new Objective("Go outside.", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[3] = new Objective("Go back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[4] = new Objective("Go to 'bed'.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[5] = new Objective("Sneak into Elara Vex's house.", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
                    objectives[6] = new Objective("Collect suspicious items... in a timely manner!", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[7] = new Objective("Run back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[8] = new Objective("GO TO COURT!", new Vector3(102.980003f,56.1100006f,-0.00902594253f));
                    break;
                case 0x02:
                    objectives[0] = new Objective("Find blacksmith George Heff in the bar. Talk to him, collect clues...", new Vector3(113.010002f,51.0699997f,-0.00902594253f));
                    objectives[1] = new Objective("Talk to George.", new Vector3(155.080002f,12.1700001f,-0.00902594253f));
                    objectives[2] = new Objective("Go outside.", new Vector3(155.080002f,12.1700001f,-0.00902594253f));
                    objectives[3] = new Objective("Go back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[4] = new Objective("Go to 'bed'.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[5] = new Objective("Sneak into George Heff's house.", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
                    objectives[6] = new Objective("Collect suspicious items... in a timely manner!", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[7] = new Objective("Run back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[8] = new Objective("GO TO COURT!", new Vector3(102.980003f,56.1100006f,-0.00902594253f));
                    break;
                case 0x03:
                    objectives[0] = new Objective("Find teacher Elizabeth Wallowmore at school. Talk to her...", new Vector3(59.9099998f,55.7799988f,0f));
                    objectives[1] = new Objective("Talk to Elizabeth Wallowmore.", new Vector3(57.9099998f,50.7799988f,0f));
                    objectives[2] = new Objective("Walk off school grounds.", new Vector3(50.9099998f,45.7799988f,0f));
                    objectives[3] = new Objective("Go back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[4] = new Objective("Go to 'bed'.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[5] = new Objective("Sneak into Ms. Wallowmore's school.", new Vector3(58.5999985f,55.7999992f,0f));
                    objectives[6] = new Objective("Collect suspicious items... in a timely manner!", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[7] = new Objective("Run back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[8] = new Objective("GO TO COURT!", new Vector3(102.980003f,56.1100006f,-0.00902594253f));
                    break;
                case 0x04:
                    objectives[0] = new Objective("Find town clerk, Benjamin Marderson in the bar. Talk to him, collect clues...", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
                    objectives[1] = new Objective("Talk to Benjamin.", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[2] = new Objective("Go outside.", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[3] = new Objective("Go back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[4] = new Objective("Go to 'bed'.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[5] = new Objective("Sneak into Benjamin's house.", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
                    objectives[6] = new Objective("Collect suspicious items... in a timely manner!", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[7] = new Objective("Run back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[8] = new Objective("GO TO COURT!", new Vector3(102.980003f,56.1100006f,-0.00902594253f));
                    break;
            }

            // Update the ObjectiveManager with the new objectives
            ObjectiveManager om = GameObject.Find("ObjectiveHandler").GetComponent<ObjectiveManager>();
            om.objectiveLabel.text = objectives[0].name; // Update the objective label with the new objective description
            om.objectives = objectives; // Assign the objectives to the ObjectiveManager
            om.objectivesInitialized = true;
        }
    }
}

