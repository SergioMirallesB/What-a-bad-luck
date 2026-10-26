using UnityEngine;

public class SpikeController : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TeleportDamage(1);

        }

    }

}
