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
        public string levelTargetID; // Separate from levelTarget, this is the name of the GameObject itself.
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
        
        // Ideal constructor
        public LevelContainer(string levelTarget, string levelTargetID, int levelNumber, int interactionLimit, string doorLevelKey) {
            this.levelTarget = levelTarget;
            this.levelTargetID = levelTargetID; // This is the name of the GameObject itself.
            this.levelNumber = levelNumber;
            this.interactionLimit = interactionLimit;
            this.doorLevelKey = doorLevelKey;
            this.objectives = new Objective[9]; // Initialize with an empty array
            this.doorManager = GameObject.Find("DoorManager").GetComponent<DoorManager>();
        }

        // Default constructor will assume level is 1
        public LevelContainer() : this("Elara Vex", "character_accused", 1, 2, "level1") {}

        public override string ToString()
        {
            return $@"Level Target: {this.levelTarget}
                     Level Number: {this.levelNumber}
                     Interaction Limit: {this.interactionLimit}
                     Door Level Key: {this.doorLevelKey}";
        }
    }

    public class Item {

    }
}