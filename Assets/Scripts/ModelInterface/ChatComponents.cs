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
}