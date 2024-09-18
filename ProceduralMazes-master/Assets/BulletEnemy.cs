using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControls player = collision.gameObject.GetComponent<PlayerControls>();
            player.Die();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Well"))
        {
            Destroy(gameObject);
        }
    }
}
