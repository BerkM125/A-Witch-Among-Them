using UnityEngine;
using UnityEngine.SceneManagement;

public class CourtroomEntrance : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Detect collision with the player
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            SceneManager.LoadScene("Courtroom");
        }
    }
}
