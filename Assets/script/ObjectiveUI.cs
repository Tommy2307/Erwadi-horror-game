using UnityEngine;
using TMPro;
using System.Collections; // Fixed: Removed the '.Empty' typo!

public class ObjectiveUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI objectiveText;

    [Header("Timer Settings")]
    [SerializeField] private float displayDuration = 60f; // Time before fading starts
    [SerializeField] private float fadeDuration = 2f;     // How long the fade-out transition takes

    void Start()
    {
        if (objectiveText == null)
        {
            objectiveText = GetComponent<TextMeshProUGUI>();
        }

        if (objectiveText != null)
        {
            // Set the initial objective text at the start of the game
            objectiveText.text = "Objective: Escape the House";
            
            // Make sure it starts fully visible
            Color txtColor = objectiveText.color;
            txtColor.a = 1f;
            objectiveText.color = txtColor;

            // Start the countdown and fade process
            StartCoroutine(DisplayAndFadeObjective());
        }
    }

    private IEnumerator DisplayAndFadeObjective()
    {
        // 1. Wait on screen for the specified display duration (60 seconds)
        yield return new WaitForSeconds(displayDuration);

        // 2. Smoothly fade the alpha over time
        float currentTime = 0f;
        Color startColor = objectiveText.color;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            
            // Calculate progress between 0 and 1
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            
            // Apply the new alpha value to the text
            objectiveText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            
            yield return null; // Wait for the next frame
        }

        // 3. Completely disable the text object when finished fading
        objectiveText.gameObject.SetActive(false);
    }
}