using UnityEngine;
using ModelBridge;
public class JudgeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ModelBridge.Bridge.ChatCompletion("http://127.0.0.1:8080",
                                                    "Instructions",
                                                    "Requests"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
