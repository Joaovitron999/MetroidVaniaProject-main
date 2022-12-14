using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = false;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferTimeCounter;

    //Dash
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private ParticleSystem dashParticles;

    //Double Jump

    [SerializeField] private bool doubleJump;
    private bool canDoubleJump = false;

    [SerializeField] private bool activeDoubleJump;


    // Attack
    public int attackDamage = 1;
    private bool canAttack = true;
    private bool isAttacking = false;
    [SerializeField]
    private float attackCooldown = 1f;

    [SerializeField]
    private GameObject Arrow;
    [SerializeField]
    private float arrowSpeed = 10;
    [SerializeField]
    private int arrowDamage = 1;
    [SerializeField]
    private Transform ArrowSpawn;
    private bool canAttackBow = true;
    private bool isAttackingBow = false;
    [SerializeField]
    private float attackBowCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //Animations
    [SerializeField] private Animator animator;

    //Walk dust particles
    [SerializeField] private ParticleSystem walkDust;
    [SerializeField] private ParticleSystem fallDust;

    //Cam
    [SerializeField] private GameObject cam;
    private CameraController camController;

    //falling
    bool IsFallingInMaxSpeed = false;
    [SerializeField] private float fallingMaxSpeed = 20f;
    private bool IsFalling = false;

    //Audio
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource attackBowSound;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource groundSound;
    [SerializeField] private AudioSource walkSound;

    //Double Jump
    [SerializeField] private bool isJumping = false;

    private void Start()
    {
        cam = Camera.main.gameObject;
        camController = cam.GetComponent<CameraController>();
        originalGravity = rb.gravityScale;
        originalSpeed = speed;
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        isJumping = !IsGrounded();

        if ((jumpBufferTimeCounter > 0 && coyoteTimeCounter > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
            coyoteTimeCounter = 0;
            jumpBufferTimeCounter = 0;
        }

        if (Input.GetButtonDown("Jump") && isJumping && canDoubleJump && activeDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
            canDoubleJump = false;
        }

        //reseta o  canDoubleJump quando o player toca o chão
        if (IsGrounded())
        {
            canDoubleJump = true;
        }



        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0;
            jumpBufferTimeCounter = 0;
        }


        //Jump Buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }

        //anim Jump

        animator.SetBool("IsGrounded", IsGrounded());
        if (rb.velocity.y < -0.1f)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
        else if (rb.velocity.y > 0.1f)
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isAttacking)
        {
            StartCoroutine(Dash());
        }
        

        if (horizontal != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        Flip();
        //testes
        if (Input.GetKeyDown(KeyCode.J) && canAttack && !isAttacking && !isDashing)
        {
            StartCoroutine(Attack());
        }


        //if(Input.GetKeyDown(KeyCode.K) && canAttackBow && !isAttackingBow)
        //{
        //    StartCoroutine(AttackBow());
        //}

        //Detecta se o player está segurando o botão de ataque com arco por mais de 1 segundo

        if (Input.GetKey(KeyCode.K) && canAttackBow)
        {
            isAttackingBow = true;
            //velocidade do player no eixo y menos 1
            rb.velocity = new Vector2(0.0f, - 0.5f);
        }
        else
        {
            isAttackingBow = false;
        }


        animator.SetBool("isAttackingBow", isAttackingBow);


        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //Walk Dust
        if (horizontal != 0 && IsGrounded())
        {
            walkDust.Play();
        }
        else
        {
            walkDust.Stop();
        }

        //limit fall speed

        if (rb.velocity.y < -fallingMaxSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -fallingMaxSpeed);
            IsFallingInMaxSpeed = true;
        }
        if (IsFallingInMaxSpeed && IsGrounded())
        {
            IsFallingInMaxSpeed = false;
            camController.ShakeCamera(5f, 0.3f);

        }

        //Dust particles when landing
        if (!IsGrounded() && rb.velocity.y < -0.1f)
        {
            IsFalling = true;
        }
        if (IsFalling && IsGrounded())
        {
            fallDust.Play();
            IsFalling = false;
            //sound
            groundSound.Play();
        }
        else
        {
            //fallDust.Stop();
        }

        //Audio
        if (horizontal != 0 && IsGrounded() && !isDashing)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
        }
        else
        {
            walkSound.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing || isAttackingBow)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
        }
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        //particulas
        dashParticles.Play();
        //audio
        //dashSound.Play();
        //camera
        camController.ShakeCamera(7f, 0.2f);
        //animação
        animator.SetBool("IsDashing", true);

        PauseGravity();
        rb.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        ResumeGravity();
        isDashing = false;
        animator.SetBool("IsDashing", false);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    //testes
    private IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;
        animator.SetTrigger("attack1");
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        canAttack = true;
    }



    IEnumerator ResetCooldownAttackBow()
    {
        yield return new WaitForSeconds(attackBowCooldown);
        canAttackBow = true;
    }
    private void InstantiateArrow()
    {
        canAttackBow = false;
        GameObject a = Instantiate(Arrow, ArrowSpawn.position, ArrowSpawn.rotation);
        //set the damage
        a.GetComponent<ArrowController>().damage = arrowDamage;
        if (isFacingRight)
        {
            a.transform.localScale = new Vector3(transform.localScale.x, 1f, 1f);
            a.GetComponent<Rigidbody2D>().velocity = new Vector2(arrowSpeed, 0f);
        }
        else
        {
            a.GetComponent<Rigidbody2D>().velocity = new Vector2(-arrowSpeed, 0f);
        }
        StartCoroutine(ResetCooldownAttackBow());
    }

    float originalSpeed;
    bool PausedSpeed = false;
    private void PauseSpeed()
    {
        speed = 0f;
    }
    private void ResumeSpeed()
    {
        speed = originalSpeed;
    }

    //pause gravity
    float originalGravity;
    bool GravityPaused = false;
    private void PauseGravity()
    {
        GravityPaused = true;
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
    }
    private void ResumeGravity()
    {
        GravityPaused = false;
        rb.gravityScale = originalGravity;
    }
}
