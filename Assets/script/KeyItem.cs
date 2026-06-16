using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Key Settings")]
    [SerializeField] private KeyType keyType = KeyType.Key1;

    // CHANGED: Returns bool to tell the interaction system if pickup succeeded
    public bool PickUp(PlayerInventory inventory)
    {
        if (inventory != null)
        {
            if (inventory.AddKey(keyType))
            {
                Debug.Log($"[KEY] Picked up {keyType}!");
                gameObject.SetActive(false);
                return true; // Successfully taken
            }
        }
        return false; // Failed (Inventory full or missing)
    }
}