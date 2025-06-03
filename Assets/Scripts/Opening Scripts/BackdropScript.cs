using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using CampaignObjects;

public class BackdropScript : MonoBehaviour
{
    public Image backdrop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeToNewScene() 
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene("New Athens Town"); // Uncomment this line to change the scene after fading
    }

    public IEnumerator FadeToBlack()
    {
        float duration = 3f;
        float elapsed = 0f;

        Color originalColor = new Color(0, 0, 0, 0);
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            backdrop.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

    }

    public IEnumerator FadeFromBlack()
    {
        float duration = 3f;
        float elapsed = 0f;

        Color originalColor = backdrop.color;
        float startAlpha = 1f;
        float endAlpha = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            backdrop.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }
    }

    public IEnumerator FullFadeScene()
    {
        yield return StartCoroutine(FadeToBlack());
        // Load the next scene here if neede
        // SceneManager.LoadScene("NextSceneName"); // Uncomment this line to change the scene after fading
        yield return StartCoroutine(FadeFromBlack());

    }

    public IEnumerator FadeToNightTime()
    {
        yield return StartCoroutine(FullFadeScene());
        backdrop.color = new Color(0, 0.1056604f, 0.7207546f, 0.4f);
    }

    public IEnumerator FadeToDayTime()
    {
        backdrop.color = new Color(0, 0, 0, 0);
        yield return StartCoroutine(FullFadeScene());
    }
}
