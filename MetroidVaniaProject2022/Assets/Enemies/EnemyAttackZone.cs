using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    private Enemy enemy;
    private Collider2D attackZone;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        attackZone = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthController>().TakeDamage(enemy.damage, transform.position);
        }
    }
}