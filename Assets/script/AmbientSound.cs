using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    [Header("Ambient Sounds")]
    [SerializeField] private AudioClip[] ambientClips;

    [Header("Settings")]
    [SerializeField] private float minDelay = 20f;
    [SerializeField] private float maxDelay = 60f;

    private AudioSource audioSource;
    private float timer = 0f;
    private float nextPlayTime;
    private int lastPlayedIndex = -1;
    private bool isPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false; // ← NEVER loop

        nextPlayTime = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        // Wait for current sound to finish first
        if (isPlaying)
        {
            if (!audioSource.isPlaying)
            {
                // Sound finished — start countdown for next sound
                isPlaying = false;
                timer = 0f;
                nextPlayTime = Random.Range(minDelay, maxDelay);
            }
            return; // Don't count timer while sound is playing
        }

        // Count timer only when no sound is playing
        timer += Time.deltaTime;

        if (timer >= nextPlayTime)
        {
            PlayRandomSound();
        }
    }

    private void PlayRandomSound()
    {
        if (ambientClips == null || ambientClips.Length == 0) return;

        int randomIndex;
        do {
            randomIndex = Random.Range(0, ambientClips.Length);
        } while (randomIndex == lastPlayedIndex && ambientClips.Length > 1);

        lastPlayedIndex = randomIndex;
        AudioClip clip = ambientClips[randomIndex];

        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
            isPlaying = true; // Mark as playing
            timer = 0f;
        }
    }
}