using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private KeyType currentKey;
    private bool hasAnyKey = false; 

    [Header("Hand Visual References")]
    [SerializeField] private GameObject handKey1Visual; 
    [SerializeField] private GameObject handKey2Visual; 
    [SerializeField] private GameObject handFinalKeyVisual; 

    void Start()
    {
        // Game start aagum pothu entha key-um kaiyila irukka koodadhu!
        if (handKey1Visual != null) handKey1Visual.SetActive(false);
        if (handKey2Visual != null) handKey2Visual.SetActive(false);
        if (handFinalKeyVisual != null) handFinalKeyVisual.SetActive(false);
    }

    public bool HasKey(KeyType keyType)
    {
        return hasAnyKey && currentKey == keyType;
    }

    public bool IsInventoryEmpty()
    {
        return !hasAnyKey;
    }

    // CHANGED: Changed from void to bool to pass success/fail status
    public bool AddKey(KeyType keyType)
    {
        if (!IsInventoryEmpty())
        {
            Debug.LogWarning("[INVENTORY] Cannot pick up! Player is already holding a key.");
            return false; // Reject pickup
        }

        currentKey = keyType;
        hasAnyKey = true;
        
        Debug.Log($"[INVENTORY] Added {keyType}!");
        UpdateHandVisuals(keyType, true);
        
        return true; // Accept pickup
    }

    public void UseKey(KeyType keyType)
    {
        if (hasAnyKey && currentKey == keyType)
        {
            hasAnyKey = false; 
            Debug.Log($"[INVENTORY] Used {keyType}!");
            UpdateHandVisuals(keyType, false);
        }
    }

    private void UpdateHandVisuals(KeyType keyType, bool show)
    {
        if (keyType == KeyType.Key1 && handKey1Visual != null) 
        {
            handKey1Visual.SetActive(show);
        }
        else if (keyType == KeyType.Key2 && handKey2Visual != null) 
        {
            handKey2Visual.SetActive(show);
        }
        else if (keyType == KeyType.FinalKey && handFinalKeyVisual != null) 
        {
            handFinalKeyVisual.SetActive(show);
        }
    }
}