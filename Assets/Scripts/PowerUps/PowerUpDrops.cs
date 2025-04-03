using UnityEngine;

public class PowerUpDrops : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player != null) 
        {
            player.powerUpBar.AddPowerUpItem();
            Destroy(gameObject);
        }
    }
}
