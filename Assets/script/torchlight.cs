using UnityEngine;
using UnityEngine.InputSystem; 

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Component")]
    [SerializeField] private Light torchLight; 

    [Header("Audio Settings")]
    [SerializeField] private AudioClip switchSound; 
    private AudioSource audioSource;

    private bool isOn = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (torchLight != null)
        {
            torchLight.enabled = false;
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            ToggleTorch();
        }
    }

    private void ToggleTorch()
    {
        if (torchLight == null) return;

        isOn = !isOn;
        torchLight.enabled = isOn;

        // Check if everything is linked properly
        if (audioSource != null && switchSound != null)
        {
            audioSource.PlayOneShot(switchSound);
            Debug.Log("The sound code ran successfully!"); 
        }
        else
        {
            if (audioSource == null) Debug.LogError("Missing Audio Source Component on this object!");
            if (switchSound == null) Debug.LogError("You forgot to drag your sound file into the Switch Sound slot!");
        }
    }
}