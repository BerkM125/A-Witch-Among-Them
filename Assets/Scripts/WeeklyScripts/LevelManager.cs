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
        public static CampaignObjects.LevelContainer[] managerLevels = new CampaignObjects.LevelContainer[3]; // Assuming 3 levels for now

        public static void InitializeManagerLevels () {
            // Initialize the manager levels with the level containers
            managerLevels[0] = new CampaignObjects.LevelContainer("Elara Vex", "character_accused", 1, 2, "level1");
            managerLevels[1] = new CampaignObjects.LevelContainer("George Heff", "george_heff", 2, 3, "level2");
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
                    objectives[0] = new Objective("Find blacksmith George Heff in the bar. Talk to him, collect clues...", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
                    objectives[1] = new Objective("Talk to George.", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[2] = new Objective("Go outside.", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[3] = new Objective("Go back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[4] = new Objective("Go to 'bed'.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[5] = new Objective("Sneak into George Heff's house.", new Vector3(115.940002f,44.0999985f,-0.00902594253f));
                    objectives[6] = new Objective("Collect suspicious items... in a timely manner!", new Vector3(147.130005f,38.5900002f,0f));
                    objectives[7] = new Objective("Run back home.", new Vector3(57.1100006f,40.9399986f,0f));
                    objectives[8] = new Objective("GO TO COURT!", new Vector3(102.980003f,56.1100006f,-0.00902594253f));
                    break;
                case 0x03:
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

