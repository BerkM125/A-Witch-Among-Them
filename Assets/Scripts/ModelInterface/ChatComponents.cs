using Newtonsoft.Json;
using System.Collections.Generic;

namespace ChatComponents {
    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class Choice
    {
        public string FinishReason { get; set; }
        public int Index { get; set; }
        public Message Message { get; set; }
    }

    public class Usage
    {
        public int CompletionTokens { get; set; }
        public int PromptTokens { get; set; }
        public int TotalTokens { get; set; }
    }

    public class Timings
    {
        public int PromptN { get; set; }
        public double PromptMs { get; set; }
        public double PromptPerTokenMs { get; set; }
        public double PromptPerSecond { get; set; }
        public int PredictedN { get; set; }
        public double PredictedMs { get; set; }
        public double PredictedPerTokenMs { get; set; }
        public double PredictedPerSecond { get; set; }
    }

    public class ChatResponse
    {
        public List<Choice> Choices { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public string SystemFingerprint { get; set; }
        public string Object { get; set; }
        public Usage Usage { get; set; }
        public string Id { get; set; }
        public Timings Timings { get; set; }
    }

    public class ChatRequest
    {
        public List<Message> Messages { get; set; }
        public string Model { get; set; }
    }

    public class GeminiPart
    {
        public string text { get; set; }
        public GeminiPart(string t) {
            text = t;
        }
    }
    public class GeminiMessage
    {
        public string role { get; set; }
        public List<GeminiPart> parts { get; set; }
        public GeminiMessage(string r, List<GeminiPart> p)
        {
            role = r;
            parts = p;
        }
    }
    public class GeminiRequest
    {
        public List<GeminiMessage> contents { get; set; }
        public GeminiRequest(List<GeminiMessage> cont)
        {
            contents = cont;
        }
    }

    public class GeminiResponse
    {
        [JsonProperty("candidates")]
        public List<Candidate> Candidates { get; set; }

        [JsonProperty("usageMetadata")]
        public UsageMetadata UsageMetadata { get; set; }

        [JsonProperty("modelVersion")]
        public string ModelVersion { get; set; }
    }

    public class Candidate
    {
        [JsonProperty("content")]
        public Content Content { get; set; }

        [JsonProperty("finishReason")]
        public string FinishReason { get; set; }

        [JsonProperty("avgLogprobs")]
        public double AvgLogprobs { get; set; }
    }

    public class Content
    {
        [JsonProperty("parts")]
        public List<Part> Parts { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }

    public class Part
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class UsageMetadata
    {
        [JsonProperty("promptTokenCount")]
        public int PromptTokenCount { get; set; }

        [JsonProperty("candidatesTokenCount")]
        public int CandidatesTokenCount { get; set; }

        [JsonProperty("totalTokenCount")]
        public int TotalTokenCount { get; set; }

        [JsonProperty("promptTokensDetails")]
        public List<TokenDetail> PromptTokensDetails { get; set; }

        [JsonProperty("candidatesTokensDetails")]
        public List<TokenDetail> CandidatesTokensDetails { get; set; }
    }

    public class TokenDetail
    {
        [JsonProperty("modality")]
        public string Modality { get; set; }

        [JsonProperty("tokenCount")]
        public int TokenCount { get; set; }
    }
}