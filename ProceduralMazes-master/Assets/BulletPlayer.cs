using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy.Die();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("EnemyShield"))
        {
            EnemyShieldController enemy = collision.gameObject.GetComponent<EnemyShieldController>();
            enemy.Die();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Well"))
        {
            Destroy(gameObject);
        }
    }
}
