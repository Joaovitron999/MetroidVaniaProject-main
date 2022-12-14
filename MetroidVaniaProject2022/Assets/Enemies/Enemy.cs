//master class for all enemies

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //combat
    [SerializeField] protected int health = 3;
    protected bool isDead = false;
    public int damage = 1;
    //particles and sounds of damage
    [SerializeField] protected ParticleSystem damageParticles;
    [SerializeField] protected AudioSource damageSound;
    //rigidbody
    [SerializeField] protected Rigidbody2D rb;
    //Animator
    [SerializeField] protected Animator animator;
    //knockback
    [SerializeField] protected float knockbackForce = 10f;
    [SerializeField] protected float knockbackTime = 0.2f;
    protected float knockbackCounter;
    protected bool isKnockback;

    //IA
    [SerializeField] protected float moveSpeed = 3f;
    protected bool isFacingRight = true;
    protected bool isMovingRight = true;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected bool isGrounded = false;
   
       
    //Is Grounded
    public bool IsGrounded()
    {
        //OverlapCircle
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));
        return hit != null;
    }



    // Damage
    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        animator.SetTrigger("Damage");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            damageParticles.Play();
            damageSound.Play();
            StartCoroutine(Knockback(attackDirection));
        }
    }
    
    //Knockback
    protected IEnumerator Knockback(Vector2 attackDirection)
    {
        isKnockback = true;
        //testa se o ataque veio da direita ou da esquerda
        if (attackDirection.x > transform.position.x)
        {
            rb.velocity = new Vector2(-knockbackForce, knockbackForce);
        }
        else
        {
            rb.velocity = new Vector2(knockbackForce, knockbackForce);
        }
        yield return new WaitForSeconds(knockbackTime);
        isKnockback = false;
        
    }

    // Die (can be overriden)
    public void Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
        StartCoroutine(DesactivateComponents());
    }

    IEnumerator DesactivateComponents()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
    


}
   
