using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private KeyType requiredKey = KeyType.Key1;
    [SerializeField] private bool isLocked = true; // uncheck for unlocked doors
    [SerializeField] private bool isMainDoor = false; // Ticked na intha door eppavume open aagadhu

    private PlayerInventory playerInventory;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Door Sound Effects")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip lockedSound;

    private bool isUnlocked = false;
    private bool isOpen = false;
    private bool hasInteractedOnce = false;
    private float lastInteractTime = 0f;
    private float interactCooldown = 0.2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInParent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // If door is not locked it opens freely
        if (!isLocked) isUnlocked = true;

        isOpen = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerInventory = player.GetComponent<PlayerInventory>();
    }

    public void InteractWithDoor(ref string uiMessage)
    {
        if (Time.time - lastInteractTime < interactCooldown) return;
        lastInteractTime = Time.time;

        // RULE 1: Main Door ah irundha strictly block panelam & locked sound play pannuvom
        if (isMainDoor)
        {
            uiMessage = "This door is permanently sealed shut!";
            PlaySound(lockedSound);
            return;
        }

        if (!isUnlocked)
        {
            if (playerInventory != null && playerInventory.HasKey(requiredKey))
            {
                uiMessage = "";
                isUnlocked = true;
                hasInteractedOnce = false;
                playerInventory.UseKey(requiredKey);
                isOpen = true;

                PlaySound(openSound);

                if (animator != null)
                {
                    animator.ResetTrigger("close");
                    animator.SetTrigger("open"); // Animator la lowercase 'open' check panniக்கோங்க
                }
            }
            else
            {
                // Dynamic feedback text telling the player exactly what key they need
                uiMessage = $"Locked! You need {requiredKey}.";
                hasInteractedOnce = true;
                PlaySound(lockedSound);
            }
        }
        else
        {
            isOpen = !isOpen;
            uiMessage = "";

            if (isOpen)
            {
                PlaySound(openSound);
                if (animator != null)
                {
                    animator.ResetTrigger("close");
                    animator.SetTrigger("open");
                }
            }
            else
            {
                PlaySound(closeSound);
                if (animator != null)
                {
                    animator.ResetTrigger("open");
                    animator.SetTrigger("close");
                }
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    public string GetDoorPrompt()
    {
        if (isMainDoor)
        {
            return "Main Door (Locked)";
        }

        if (!isUnlocked)
        {
            // Player door ah parthale enna key venum nu munnadiye text text-ah dynamic ah kaatum
            return hasInteractedOnce ? $"Needs Key To open" : $"Press E to Use the Key";
        }
        else
        {
            return isOpen ? "Press E to Close Door" : "Press E to Open Door";
        }
    }
}