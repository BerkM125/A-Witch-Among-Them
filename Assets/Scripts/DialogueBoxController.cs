using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class DialogueBoxController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image portraitImage;

    private Coroutine typingCoroutine;
    public CharacterData characterData;

    private bool mouseDown = false;
    private bool textDone = false;

    // private Dictionary<string, Queue> dialogues;
    private Queue<Tuple<string, string>> dialogues;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Dictionary<string, CharacterData.Character> characters;
    
    public DialogueBoxController()
    {
        // SetActive(false); // Ensure the dialogue box is hidden initially
        characters = new Dictionary<string, CharacterData.Character>();
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update() {
        // Optional: Add input handling here if needed
        if (Input.GetMouseButtonDown(0)) {
            mouseDown = true;
            if (textDone) {
                mouseDown = false;
                textDone = false;
                SetActive(false);
            }
        }
    }

    private void SetName(string name)
    {
        nameText.text = name;
    }

    public IEnumerator ShowDialogue()
    {
        textDone = false;

        while (dialogues.Count > 0)
        {
            Tuple<string, string> message = dialogues.Dequeue();
            SetActive(true, message.Item1);
            yield return typingCoroutine = StartCoroutine(TypeDialogue(message.Item2));
            yield return new WaitUntil(() => mouseDown);  // Wait for user click to continue
            mouseDown = false;
        }

        textDone = true;
    }

    private void SetPortrait(Sprite portrait)
    {
        portraitImage.sprite = portrait;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        // gameObject.SetActive(true);
    }

    private void SetActive(bool active, string character_id) {
        CharacterData.Character c = characters[character_id];
        SetActive(active);
        SetName(c.characterName);
        SetPortrait(c.characterImage);
        // SetDialogue(character_id);
        // StartCoroutine(SetDialogue(character_id)); // <-- Coroutine!
    }

    private IEnumerator TypeDialogue(string message) {
        dialogueText.text = ""; // Clear the dialogue text
        foreach (char letter in message.ToCharArray()) {
            if (mouseDown) {
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Adjust typing speed here
        }
        dialogueText.text = message;
        mouseDown = false;
    }

    public void SkipTyping() {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    public void AddDialogue(string character_id, string message) {
        if (characterData.characters.Length != characters.Keys.Count)
        {
            characters = new Dictionary<string, CharacterData.Character>();
            foreach (CharacterData.Character character in characterData.characters) {
                characters.Add(character.characterId, character);
            }
        }

        // Initialize dictionary if not already initialized (but should normally be done once in Awake())
        if (dialogues == null) {
            dialogues = new Queue<Tuple<string, string>>();
        }

        // Enqueue the new message for the character
        dialogues.Enqueue(new Tuple<string, string>(character_id, message));
    }

}
