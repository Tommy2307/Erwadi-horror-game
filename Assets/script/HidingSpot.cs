using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public Transform hidePoint;

    private bool playerHidden = false;

    public void TriggerHide(GameObject player)
    {
        if (!playerHidden)
        {
            player.transform.position = hidePoint.position;

            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.enabled = false;

            playerHidden = true;
        }
        else
        {
            ExitHide(player);
        }
    }

    public void ExitHide(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = true;

        playerHidden = false;
    }
}