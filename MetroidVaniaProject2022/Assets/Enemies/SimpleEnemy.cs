//this script extends the Enemy class
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    [SerializeField] private Transform groundDetection;
    [SerializeField] private Transform wallDetection;
    [SerializeField] private Vector2 movementDirection = Vector2.right;
    [SerializeField] private int state = 1; // 0 = idle, 1 = moving, 2 = attacking
    //range [0, 1]
    [Range(0, 1)]  [SerializeField] private float proportionOfTimeMoving = 0.8f;
    
    [Header("Attack")]
    [Range(0, 10)] [SerializeField] private float attackRange = 0.5f;
    [Range(0, 10)] [SerializeField] private float visionRange = 5f;
    [SerializeField] private Collider2D playerInRange;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackTime = 1f;
    [SerializeField] private float attackCounter = 0f;


    private void Start()
    {
        StartCoroutine(ChangeState());
    }

    private IEnumerator ChangeState()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            //random number between 0 and 1
            float random = Random.Range(0f, 1f);
            if (random < proportionOfTimeMoving && state == 0)
            {
                state = 1;
            }
            else if (random >= proportionOfTimeMoving && state == 1)
            {
                state = 0;
            }
            
            //player in range
            playerInRange = Physics2D.OverlapCircle(transform.position, visionRange, LayerMask.GetMask("Player"));
            

            if(state == 2 && playerInRange == null)
            {
                state = 0;
            }
            else if(state <=1 && playerInRange != null)
            {
                state = 2;
            }
        }
        yield return null;
    }

    

    public  void Update() {
        isGrounded = IsGrounded();

        if(isDead || isKnockback){
            return;
        }
        switch (state)
        {
            case 0:
                //idle
                animator.SetBool("isMoving", false);
                break;
            case 1:
                //moving
                Move();
                break;
            case 2:
                //pursuit
                Pursuit();

                break;
            default:
                break;
        }

        if(attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        else
        {
            isAttacking = false;
        }
    } 

    private void Pursuit()
    {
        
        if (playerInRange != null)
        {
            Collider2D playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));
            if (playerInAttackRange != null)
            {
                //attack
                animator.SetBool("isMoving", false);
                if (!isAttacking)
                {
                    attackCounter = attackTime;
                    isAttacking = true;
                    animator.SetTrigger("Attack");
                }
                
            }
            else if (playerInAttackRange == null)
            {
                //move towards player
                animator.SetBool("isMoving", true);
                if (transform.position.x < playerInRange.transform.position.x)
                {
                    rb.velocity = Vector2.right * moveSpeed;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    isFacingRight = true;
                    
                }
                else
                {
                    rb.velocity = Vector2.left * moveSpeed;
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    isFacingRight = false;
                   
                }

            }
        }
        else
        {
            //idle
            animator.SetBool("isMoving", false);
        }
    }

    private void Move()
    {
        animator.SetBool("isMoving", true);
        rb.velocity = movementDirection * moveSpeed;
        
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        //wall detection with overlap circle
        Collider2D wallInfo = Physics2D.OverlapCircle(wallDetection.position, 0.5f, LayerMask.GetMask("Ground"));

        if (groundInfo.collider == false || wallInfo != null)
        {
            if (isFacingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                isFacingRight = false;
                movementDirection = Vector2.left;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isFacingRight = true;
                movementDirection = Vector2.right;
            }

            state = 0;
        }

    }   
    //OnDrawGizmosSelected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundDetection.position, Vector2.down * 2f);
        Gizmos.DrawRay(wallDetection.position, Vector2.right * 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}

