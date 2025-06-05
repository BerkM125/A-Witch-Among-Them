using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InstructionSuite;
using LevelManagerNamespace;

public class ExecutionManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InitiateExecutionSequence()); // Start the execution sequence
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InitiateExecutionSequence()
    {
       yield return new WaitForSeconds(10f);

       // Remember, levels are incremented in court before.
       if(LevelManager.currentLevel == 4) {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
       }
       else if (!LevelManager.playerLost) {
        UnityEngine.SceneManagement.SceneManager.LoadScene("New Athens Town");
       }
       else if (LevelManager.playerLost) {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
       }
    }
}
