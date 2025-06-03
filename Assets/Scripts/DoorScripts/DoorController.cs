using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D circleCollider2D;
    private bool open = false;
    private float minDistance = 2f;
    private float minTeleportDistance = 1f;
    private AudioSource audioSource;

    public Transform teleportLocation;
    public string sceneName;
    public bool changeScene;

    public bool locked = false;
    public string lockedMessage = "This door is locked.";

    public DialogueBoxController dialogueBoxController;
    void Start()
    {
        animator = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator.SetBool("IsOpen", false);
        audioSource = GetComponent<AudioSource>();
    }
    void DestroyPersistentObjects()
    {
        GameObject[] persistentObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in persistentObjects)
        {
            if (obj.scene.name == null) // This checks objects in the DontDestroyOnLoad scene
            {
                Destroy(obj);
            }
        }
    }
    void Update()
    {
        if (!locked || (locked && !open))
        {
            animator.SetBool("IsOpen", open);
        }
        if (open && Input.GetKeyDown(KeyCode.E))
        {
            if (locked)
            {
                if (dialogueBoxController != null)
                {
                    dialogueBoxController.AddDialogue("character_player", lockedMessage);
                    dialogueBoxController.ShowDialogue();
                    // StartCoroutine(dialogueBoxController.TypeDialogue());
                }
                else
                {
                    Debug.LogWarning("DialogueController is not assigned. Please assign it in the inspector.");
                }
            }
            else if (audioSource.isPlaying)
            {
                // If the audio is already playing, do nothing
                return;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen"))
            {
                // If the door is already open, do nothing
                return;
            }
            else
            {
                audioSource.Play();
                if (!changeScene)
                {
                    // if (Vector2.Distance(transform.position, teleportLocation.position) < minTeleportDistance)
                    // {
                    GameObject.FindGameObjectWithTag("Player").transform.position = teleportLocation.position;
                    // }
                }
                else if (gameObject.name == "Square")
                {
                    // Code to load the scene from sceneName string
                    DestroyPersistentObjects();
                    DestroyPersistentObjects();

                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                }
            }
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         animator.SetBool("IsOpen", true);
    //     }
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         animator.SetBool("IsOpen", false);
    //     } 
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Vector2.Distance(transform.position, collision.transform.position) < minDistance)
            {
                open = true;
                // animator.SetBool("IsOpen", true);
            }
            // open = true;
            // animator.SetBool("IsOpen", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            open = false;
            // animator.SetBool("IsOpen", false);
        }
    }
}
