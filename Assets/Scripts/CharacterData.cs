using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Dialogue Box/CharacterData")]
public class CharacterData : ScriptableObject
{
    [System.Serializable]
    public class Character
    {
        public string characterName;
        public string characterId;
        [TextArea] public string description;
        public Sprite characterImage;
        // audio clips for blips
        public AudioClip[] blips;
    }

    [Header("List of Characters")]
    public Character[] characters;
}
