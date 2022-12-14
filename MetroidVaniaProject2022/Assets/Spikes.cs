using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    //detect collision with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthController>().QuickDamage();
        }
    }
    
}