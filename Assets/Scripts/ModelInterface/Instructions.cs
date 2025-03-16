using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Instructions {
    // thoughts on swappin to Enums?
    public class JudgeStatement
    {
        public const int DELIVER_OPENING_STATEMENT = 0x01;
        public const int DELIVER_JUDGMENT = 0x02;
        public const int DELIVER_FINAL_INSTRUCTIONS = 0x03;
        public const int CONVERSE_WITH_PLAYER = 0x04;
        public const int CONVERSE_WITH_DEFENDANT = 0x05;
        public const int DELIVER_AS_DEFENDANT = 0x06;
    }
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