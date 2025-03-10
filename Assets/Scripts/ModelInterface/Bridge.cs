using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bridge : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Initialization code here
        StartCoroutine(ChatCompletion("http://localhost:8080"));
    }

    // Update is called once per frame
    void Update()
    {
        // Update code here
    }

    // Return simple completion text from model URL.
    // Model URL should be localhost:8080, not an specific path/endpoint.
    IEnumerator ChatCompletion(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + "/v1/chat/completions"))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}