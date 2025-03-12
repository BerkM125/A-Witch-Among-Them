using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System;

namespace ModelBridge {
    static class Bridge
    {
        // Load JSON payload for model and pack it correctly.
        private static string LoadJSON(string modelURI, 
                                            string systemInstructions,
                                            string prompt) {
            return $@"
            {{
                ""messages"": [
                    {{
                        ""role"": ""user"",
                        ""content"": ""{prompt}""
                    }},
                    {{
                        ""role"": ""system"",
                        ""content"": ""{systemInstructions}""
                    }}
                ]
            }}";
        }

        // Return simple completion text from model URL.
        // Model URL should be localhost:8080, not an specific path/endpoint.
        public static IEnumerator ChatCompletion(string modelURI,
                                                string systemInstructions,
                                                string prompt)
        {
            // Create JSON payload
            string jsonData = LoadJSON(modelURI, systemInstructions, prompt);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

            // Launch request to model and retrieve result, parse for content
            using (UnityWebRequest request = new UnityWebRequest($@"{modelURI}/v1/chat/completions", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Load all json response data
                    string jsonResponse = request.downloadHandler.text;

                    // Parse JSON response for chat response
                    ChatComponents.ChatResponse response = JsonConvert.DeserializeObject<ChatComponents.ChatResponse>(jsonResponse);
                    return response.Choices[0].Message.Content;
                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                }
            
            }
        }
    }
}
