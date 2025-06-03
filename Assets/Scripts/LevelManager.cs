using UnityEngine;
using CampaignObjects;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public CampaignObjects.LevelContainer[] managerLevels;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void LoadLevelOne() {

    }

    // Introduces the plot with the blacksmith and the discussions over Elara's death and such, etc etc.
    // Make sure to unlock more doors, include new evidence, increase interaction limit for court
    void LoadLevelTwo() {

    }

    void LoadLevelThree() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
