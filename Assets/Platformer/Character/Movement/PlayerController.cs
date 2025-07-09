using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SlashHitbox slashHitbox;
    [SerializeField] private BossHitboxPlayer bossHitbox;
    private float lastNormalSlashTime = -999f;
    [SerializeField] private float normalSlashCooldown = 1f;

    private bool isAttacking;
    private float lastHeavySlashTime = -999f; // Initialize so it’s available on first use
    [SerializeField] private float heavySlashCooldown = 3f;
    [SerializeField] private CooldownBar cooldownBar;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Collider Resizing")]
    [SerializeField] private CapsuleCollider2D mainCollider;
    [SerializeField] private Vector2 standingSize = new Vector2(1f, 2f);
    [SerializeField] private Vector2 crouchingSize = new Vector2(1f, 1.2f);
    [SerializeField] private Vector2 standingOffset = new Vector2(0f, 0f);
    [SerializeField] private Vector2 crouchingOffset = new Vector2(0f, -0.4f);
    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] private int normalSlashDamage = 20;
    [SerializeField] private int heavySlashDamage = 40;


    private Rigidbody2D body;
    private Animator anim;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isCrouching;
    private bool previousCrouchState;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        mainCollider.size = standingSize;
        mainCollider.offset = standingOffset;
    }

    [System.Obsolete]
    private void Update()
    {
        // Prevent any input during an attack animation
        if (isAttacking)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

            // Check if current state is not a slash or it's finished playing
            if (!state.IsName("SingleSlash") && !state.IsName("DoubleSlash"))
            {
                isAttacking = false;
            }
            else if (state.normalizedTime >= 1f)
            {
                isAttacking = false;
            }
            else
            {
                return; // Lock all input
            }
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        isCrouching = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float speed = isCrouching ? moveSpeed * 0.5f : moveSpeed;
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isCrouching)
        {
            Jump();
        }

        float velocityX = Mathf.Abs(body.linearVelocity.x);
        float velocityY = body.linearVelocity.y;

        anim.SetFloat("velocityX", velocityX);
        anim.SetFloat("velocityY", velocityY);
        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Run", velocityX > 0.1f && isGrounded && !isCrouching);
        anim.SetBool("Fall", velocityY < -0.1f && !isGrounded);
        anim.SetBool("Crouch", isCrouching);

        if (!wasGrounded && isGrounded)
        {
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("Fall");
            anim.SetTrigger("Land");
        }

        // Handle collider resizing
        if (isCrouching != previousCrouchState)
        {
            if (isCrouching)
            {
                mainCollider.size = crouchingSize;
                mainCollider.offset = crouchingOffset;
            }
            else
            {
                float heightDiff = (standingSize.y - crouchingSize.y);
                transform.position += new Vector3(0f, heightDiff / 2f, 0f);

                mainCollider.size = standingSize;
                mainCollider.offset = standingOffset;
            }

            previousCrouchState = isCrouching;
            // Apply downward velocity when crouching in air
            if (!isGrounded && isCrouching)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, -jumpForce);
            }

        }

        wasGrounded = isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            // If Shift is held and cooldown passed, do heavy slash
            if (Input.GetKey(KeyCode.LeftShift) && Time.time - lastHeavySlashTime >= heavySlashCooldown)
            {
                HeavySlash();
                lastHeavySlashTime = Time.time;
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && Time.time - lastNormalSlashTime >= normalSlashCooldown)
            {
                NormalSlash();
                Debug.Log("Normal slash executed");
                lastNormalSlashTime = Time.time;
            }

        }

    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetTrigger("Jump");
    }

    private void NormalSlash()
    {
        anim.SetTrigger("Slash1");
        isAttacking = true;

        if (slashHitbox != null)
        {
            slashHitbox.ActivateHitbox(normalSlashDamage);
        }
        if(bossHitbox != null)
        {
            bossHitbox.ActivateHitbox(normalSlashDamage);
        }
    }


    [System.Obsolete]
    private void HeavySlash()
    {
        anim.SetTrigger("Slash2");
        isAttacking = true;

        if (slashHitbox != null)
        {
            slashHitbox.ActivateHitbox(heavySlashDamage);
        }
        if(bossHitbox != null)
        {
            bossHitbox.ActivateHitbox(normalSlashDamage);
        }

        if (cooldownBar != null)
            cooldownBar.StartCooldown();
    }


    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    private void DealDamage(int damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        foreach (Collider2D enemy in hits)
        {
            enemy.GetComponent<IDamageable>()?.TakeDamage(damage);
        }

    }

}

