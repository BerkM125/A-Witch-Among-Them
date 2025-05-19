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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SetActive(false); // Ensure the dialogue box is hidden initially
        dialogues = new Dictionary<string, Queue>();
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

    private void SetDialogue(string character_id)
    {
        // dialogueText.text = dialogue;
        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
        }
        textDone = false;
        while (dialogues[character_id].Count > 0) {
            string message = (string) dialogues[character_id].Dequeue();
            typingCoroutine = StartCoroutine(TypeDialogue(message));
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

    private void SetActive(bool active, string name, Sprite portrait)
    {
        SetActive(active);
        SetName(name);
        SetPortrait(portrait);
        SetDialogue(name);
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
        if (!dialogues.contains(character_id)) {
            dialogue[character_id] = new Queue();
        }   
        dialogue[character_id].Enqueue(message);
    }

    public void ShowDialogue(string character_id) {
        Debug.Log("Showing dialogue for character: " + character_id);
        Debug.Log("Message: " + message);
        foreach (CharacterData.Character character in characterData.characters) {
            if (character.characterId == character_id) {
                Debug.Log("hit character " + character_id);
                SetActive(true, character.characterName, character.characterImage);
                return;
            }
        }
        Debug.LogError("Unknown character " + character_id);
    }
}
