using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class DoorManager : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public string key = "";
        public bool locked = false;
        public DoorController[] doors;
    }
    public Level[] levels;

    private Dictionary<string, Level> levelDictionary;

    public DialogueBoxController dialogueBoxController;

    void Start()
    {
        levelDictionary = new Dictionary<string, Level>();
        foreach (Level level in levels)
        {
            foreach (DoorController door in level.doors)
            {
                if (!door) continue;
                door.locked = level.locked; // Set the initial lock status of the doors
                door.dialogueBoxController = dialogueBoxController; // Assign the dialogue box controller to each door
            }
            if (!levelDictionary.ContainsKey(level.key))
            {
                levelDictionary.Add(level.key, level);
            }
            else
            {
                Debug.LogWarning($"Duplicate key found: {level.key}. Only the first instance will be used.");
            }

        }
    }

    public void ChangeLevelLockStatus(string levelName, bool status)
    {
        levelDictionary.TryGetValue(levelName, out Level level);
        level.locked = status;
        foreach (DoorController door in level.doors)
        {
            if (door != null)
            {
                door.locked = status; // Disable the door controller if locked
            }
        }
    }
}