using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D circleCollider2D;
    void Start()
    {
        animator = GetComponent<Animator>(); 
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("IsOpen", true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("IsOpen", false);
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("IsOpen", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("IsOpen", false);
        }
    }
}
