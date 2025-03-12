using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Instructions {
    public class JudgeDialogue
    {
        public string openingInstructions { get; set; }
        public string judgmentInstruction { get; set; }
        public string finalInstructions { get; set; }
        public List<string> evidence { get; set; }
    }
    public class JudgeInstructions
    {
        public JudgeDialogue prototype { get; set; }
    }
}