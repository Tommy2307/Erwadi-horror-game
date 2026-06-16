using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Timer Settings")]
    [SerializeField] private float displayDuration = 5f; // Shows on screen for 5 seconds
    [SerializeField] private float fadeDuration = 1.5f;   // Smoothly fades over 1.5 seconds

    void Start()
    {
        if (tutorialText == null)
        {
            tutorialText = GetComponent<TextMeshProUGUI>();
        }

        if (tutorialText != null)
        {
            // Set the instruction message
            tutorialText.text = "Press F to turn on the Flashlight";
            
            // Set text color to white and fully visible at start
            Color txtColor = tutorialText.color;
            txtColor.a = 1f;
            tutorialText.color = txtColor;

            // Start the delay and fade process
            StartCoroutine(DisplayAndFadeTutorial());
        }
    }

    private IEnumerator DisplayAndFadeTutorial()
    {
        // 1. Leave the message on screen for 5 seconds
        yield return new WaitForSeconds(displayDuration);

        // 2. Smoothly fade transparency out
        float currentTime = 0f;
        Color startColor = tutorialText.color;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            tutorialText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // 3. Deactivate the object completely when done
        tutorialText.gameObject.SetActive(false);
    }
}