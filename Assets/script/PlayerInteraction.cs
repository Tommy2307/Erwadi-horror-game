using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 3.5f;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI uiText;

    private Camera cam;
    private PlayerInventory inventory;
    private string currentLockMessage = "";
    private float messageTimer = 0f;

    void Start()
    {
        cam = Camera.main;
        
        if (uiText == null)
        {
            GameObject uiObj = GameObject.Find("intraction");
            if (uiObj != null)
                uiText = uiObj.GetComponent<TextMeshProUGUI>();
            else
                uiText = FindFirstObjectByType<TextMeshProUGUI>();
        }
        
        inventory = GetComponent<PlayerInventory>();
        if (inventory == null)
            inventory = FindFirstObjectByType<PlayerInventory>();
    }

    void Update()
    {
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;
            if (uiText != null) uiText.text = currentLockMessage;
            if (messageTimer <= 0) currentLockMessage = ""; 
            return; 
        }

        bool inputPressedThisFrame = false;
        if (Keyboard.current != null)
        {
            inputPressedThisFrame = Keyboard.current.eKey.wasPressedThisFrame;
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // PURE OLD KEY CHECK
            if (hit.collider.CompareTag("Key"))
            {
                if (uiText != null) uiText.text = "Press E to pick up the key";
                
                if (inputPressedThisFrame)
                {
                    KeyItem keyScript = hit.collider.GetComponent<KeyItem>();
                    if (keyScript != null)
                    {
                        // FIXED: Check if the key item successfully picked up or failed!
                        bool success = keyScript.PickUp(inventory);
                        
                        if (!success)
                        {
                            // Trigger your built-in timer message system!
                            currentLockMessage = "You can't pick up! Your hand is full!";
                            messageTimer = 2.5f; // Shows on screen for 2.5 seconds
                        }
                    }
                }
                return;
            }

            // PURE OLD DOOR CHECK
            Door doorScript = hit.collider.GetComponent<Door>() ?? hit.collider.GetComponentInParent<Door>() ?? hit.collider.GetComponentInChildren<Door>();
            if (doorScript != null)
            {
                if (uiText != null) uiText.text = doorScript.GetDoorPrompt();

                if (inputPressedThisFrame)
                {
                    string returnedMessage = "";
                    doorScript.InteractWithDoor(ref returnedMessage);
                    
                    if (!string.IsNullOrEmpty(returnedMessage)) 
                    {
                        currentLockMessage = returnedMessage;
                        messageTimer = 2.5f;
                    }
                }
                return;
            }
        }

        if (uiText != null && string.IsNullOrEmpty(currentLockMessage)) uiText.text = "";
    }
}