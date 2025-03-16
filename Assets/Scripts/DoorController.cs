using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D circleCollider2D;
    private bool open = false;
    private float minDistance = 2f;
    private float minTeleportDistance = 1f;
    private AudioSource audioSource;

    public Transform teleportLocation;
    void Start()
    {
        animator = GetComponent<Animator>(); 
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator.SetBool("IsOpen", false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        animator.SetBool("IsOpen", open);
        if (open && Input.GetKeyDown(KeyCode.E)) {
            audioSource.Play();
            // if (Vector2.Distance(transform.position, teleportLocation.position) < minTeleportDistance)
            // {
                GameObject.FindGameObjectWithTag("Player").transform.position = teleportLocation.position;
            // }
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
