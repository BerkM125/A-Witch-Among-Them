using UnityEngine;

namespace CampaignObjects {

    public class Objective {
        public string name = "";
        public bool fulfilled = false;
        public Vector3 position = new Vector3(0f, 0f, 0f);
        public Objective(string name) {
            this.name = name;
            this.fulfilled = false;
        }
        public Objective(string name, Vector3 position) {
            this.name = name;
            this.fulfilled = false;
            this.position = position;
        }
    }

    // Called LevelContainer to avoid confusion with DoorManager.Level
    public class LevelContainer {
        public int levelNumber;
        public int interactionLimit;
        
        public string levelTarget;
        // This key will be used to unlock the necessary doors.
        public string doorLevelKey;
        // Thing that stores the evidence?

        // Thing that stores the objectives
        public Objective[] objectives;
        // Thing that stores the player data?

        [SerializeField]
        private DoorManager doorManager;

        public void OpenLevelDoors() {
            // UNLOCKED
            doorManager.ChangeLevelLockStatus(doorLevelKey, false);
        }
        
        public LevelContainer(string levelTarget, int levelNumber, int interactionLimit, string doorLevelKey) {
            this.levelTarget = levelTarget;
            this.levelNumber = levelNumber;
            this.interactionLimit = interactionLimit;
            this.doorLevelKey = doorLevelKey;
            this.objectives = new Objective[0]; // Initialize with an empty array
            this.doorManager = GameObject.Find("DoorManager").GetComponent<DoorManager>();
        }

        public LevelContainer() {
            this.levelTarget = "Elara Vex";
            this.levelNumber = 1;
            this.interactionLimit = 2;
            this.doorLevelKey = "level1";
            this.objectives = new Objective[9]; // Initialize with an empty array
            this.doorManager = GameObject.Find("DoorManager").GetComponent<DoorManager>();
        }
    }

    public class Item {

    }
}