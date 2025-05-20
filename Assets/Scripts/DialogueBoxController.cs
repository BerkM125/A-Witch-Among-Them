using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueBoxController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image portraitImage;

    private Coroutine typingCoroutine;
    public CharacterData characterData;

    private bool mouseDown = false;
    private bool textDone = false;

    private Dictionary<string, Queue> dialogues;
    void Awake() {
        dialogues = new Dictionary<string, Queue>();
        Debug.Log("Awake called, dialogues initialized");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SetActive(false); // Ensure the dialogue box is hidden initially
    }

    // Update is called once per frame
    void Update()
    {
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

    private IEnumerator SetDialogue(string character_id)
    {
        if (!dialogues.ContainsKey(character_id))
        {
            Debug.Log("Current dialogues keys: " + string.Join(", ", dialogues.Keys)); 
            Debug.LogError($"Key '{character_id}' not found in dictionary!");
            yield break; // Exit the method early if the key doesn't exist
        }
 
        
        // if (typingCoroutine != null) {
        //     StopCoroutine(typingCoroutine);
        // }

        var queue = dialogues[character_id];
        textDone = false;

        while (queue.Count > 0)
        {
            string message = (string) queue.Dequeue();
            yield return typingCoroutine = StartCoroutine(TypeDialogue(message));
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

    private void SetActive(bool active, string character_id, string name, Sprite portrait)
    {
        SetActive(active);
        SetName(name);
        SetPortrait(portrait);
        // SetDialogue(character_id);
        StartCoroutine(SetDialogue(character_id)); // <-- Coroutine!
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
       Debug.Log(character_id + ": " + message);

        // Initialize dictionary if not already initialized (but should normally be done once in Awake())
        if (dialogues == null)
        {
            dialogues = new Dictionary<string, Queue>();
        }

        // If the character doesn't have an entry in the dictionary, create one
        if (!dialogues.ContainsKey(character_id))
        {
            Debug.Log("Adding new queue for character: " + character_id);
            dialogues.Add(character_id, new Queue());
        }

        // Enqueue the new message for the character
        dialogues[character_id].Enqueue(message);

        // Debug the current keys in the dictionary
        Debug.Log("Current dialogues keys: " + string.Join(", ", dialogues.Keys)); 
    }

    public void ShowDialogue(string character_id) {
        Debug.Log("Showing dialogue for character: " + character_id);
        foreach (CharacterData.Character character in characterData.characters) {
            if (character.characterId == character_id) {
                Debug.Log("hit character " + character_id);
                SetActive(true, character_id, character.characterName, character.characterImage);
                return;
            }
        }
        Debug.LogError("Unknown character " + character_id);
    }
}
