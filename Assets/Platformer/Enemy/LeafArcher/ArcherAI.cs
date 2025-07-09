using UnityEngine;
using System.Collections;

public class LeafArcherAI : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private MeleeHitbox slashHitbox;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool isDead = false;
    private bool isAttacking;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Combat")]
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private float rangedRange = 6f;
    [SerializeField] private float chaseRange = 8f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int meleeDamage = 15;
    [SerializeField] private int maxHealth = 100;

    private float lastAttackTime = -999f;
    private int currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead)
            return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;

        // Face the player
        if (direction.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);

        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            SetAnimationState(idle: true); // explicitly force idle during attack
            return;
        }

        // Movement & attack logic
        if (distance <= meleeRange && Time.time - lastAttackTime >= attackCooldown)
        {
            StartCoroutine(PerformMelee());
        }
        else if (distance <= rangedRange && Time.time - lastAttackTime >= attackCooldown)
        {
            StartCoroutine(PerformRanged());
        }
        else if (distance <= chaseRange && distance > rangedRange)
        {
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
            SetAnimationState(run: true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            SetAnimationState(idle: true);
        }   

        // Jumping
        if (ShouldJump())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("Fall", rb.linearVelocity.y < -0.1f && !isGrounded);
    }
    private void SetAnimationState(bool run = false, bool idle = false)
    {
        anim.SetBool("Run", run);
        anim.SetBool("Idle", idle);
    }


    private IEnumerator PerformMelee()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        anim.SetTrigger("Melee");

        yield return new WaitForSeconds(0.767f);
        isAttacking = false; // total animation duration
        anim.SetBool("Idle", true);
    }
    private void Trigger()
    {
        slashHitbox?.ActivateHitbox(meleeDamage);
    }
    private void untrigger()
    {
        slashHitbox?.DisableHitbox();
    }
    private IEnumerator PerformRanged()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        anim.SetTrigger("Shoot");

        yield return new WaitForSeconds(1.556f); // total animation duration
        isAttacking = false;
        anim.SetBool("Idle", true);
    }

    public void SpawnArrow()
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
            Vector2 dir = (player.position - arrowSpawnPoint.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            arrow.GetComponent<Rigidbody2D>().linearVelocity = dir * 10f;
        }
    }

    public void TakeDamage(int amount)
    {
        GetComponent<EnemyHealth>()?.TakeDamage(amount);
        anim.SetTrigger("Hurt");
    }

    public void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("Die");
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        StartCoroutine(WaitForDeathAnimation());
    }
    private IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        while (!state.IsName("Die") || state.normalizedTime < 0.95f)
        {
            yield return null;
            state = anim.GetCurrentAnimatorStateInfo(0);
        }

        Destroy(gameObject);
    }
    private bool ShouldJump()
    {
        return false; // placeholder
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangedRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (slashHitbox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(slashHitbox.transform.position, 0.5f);
        }
    }
}
