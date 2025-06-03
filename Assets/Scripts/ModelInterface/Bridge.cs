using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ModelBridge {
    public static class Bridge
    {
        // Load JSON payload for model and pack it correctly.
        private static string LoadJSON(string modelURI, 
                                            string systemInstructions,
                                            string prompt) {
            return $@"
            {{
                ""model"": ""deepseek-chat"",
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

        private static string ReformatForGemini(string jsonData) {
            Debug.Log("Reformatting JSON for Gemini API..." + jsonData);
            var originalData = JsonConvert.DeserializeObject<ChatComponents.ChatRequest>(jsonData);            
            var geminiData = new {
                system_instruction = new {
                    parts = new { 
                        text = originalData.Messages[1].Content
                    }
                },
                contents = new {
                    parts = new {
                        text = originalData.Messages[0].Content
                    }
                }
            };
            
            // Convert the new JSON object to a string
            return JsonConvert.SerializeObject(geminiData);
        }

        private static string FormatForHttp(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Encode the string to ensure special characters don't break the HTTP request
            return Uri.EscapeDataString(input);
        }

        // Return simple completion text from model URL.
        // Model URL should be localhost:8080, not an specific path/endpoint.
        public static IEnumerator ChatCompletion(string modelURI,
                                                string systemInstructions,
                                                string prompt,
                                                Action<string> callback)
        {
            Debug.Log("INITIATING CHAT COMPLETION...");
            // Create JSON payload
            string jsonData = LoadJSON(modelURI, FormatForHttp(systemInstructions), FormatForHttp(prompt));
            jsonData = ReformatForGemini(jsonData);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

            Debug.Log("Request body: " + jsonData);

            string geminiURI = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=AIzaSyDUKrq5_rdOpa1xwW4DHurGpOyh9kFZYFo";
            // Launch request to model and retrieve result, parse for content
            using (UnityWebRequest request = new UnityWebRequest(geminiURI, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Load all json response data
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Response: " + jsonResponse);

                    // Parse JSON response for chat response
                    ChatComponents.GeminiResponse response = JsonConvert.DeserializeObject<ChatComponents.GeminiResponse>(jsonResponse);
                    callback?.Invoke(response.Candidates[0].Content.Parts[0].Text);
                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }
    }
}
