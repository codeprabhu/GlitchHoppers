using UnityEngine;
using System.Collections;

public class BadgerAI : MonoBehaviour, IDamageable
{
    [Header("Detection & Layers")]
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer; // NEW
    [SerializeField] private Transform groundCheck; // NEW
    [SerializeField] private float groundCheckRadius = 0.2f; // NEW

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private BadgerAttackTrigger attackTrigger;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float patrolRange = 6f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] public int damage = 20;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool isDead = false;
    private bool isAttacking = false;

    private float lastAttackTime = -999f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Update()
    {
        if (isDead || player == null || !isGrounded) return;

        if (isAttacking)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("Attack") || state.normalizedTime >= 1f)
                isAttacking = false;
            else
                return;
        }

        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (distance > patrolRange)
        {
            StopMovement();
            anim.SetBool("Idle", true);
        }
        else if (distance > attackRange)
        {
            MoveTowardPlayer();
        }
        else
        {
            StopMovement();
            if (Time.time - lastAttackTime >= attackCooldown)
                Attack();
        }
    
        float velocityX = Mathf.Abs(rb.linearVelocity.x);
        anim.SetBool("Run", velocityX > 0.1f);
    }

    private void MoveTowardPlayer()
    {
        anim.SetBool("Idle", false);
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        FlipSprite(direction);
    }

    private void StopMovement()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        anim.SetBool("Run", false);
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        isAttacking = true;
        lastAttackTime = Time.time;
    }

    private void FlipSprite(float direction)
    {
        transform.localScale = new Vector3(direction > 0.01f ? 1 : -1, 1, 1);
    }

    public void TakeHit(int damage)
    {
        GetComponent<EnemyHealth>()?.TakeDamage(damage);
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
        Destroy(gameObject,1.017f);
    }

    private IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        while (!state.IsName("Die") || state.normalizedTime < 0.95f)
        {
            yield return null;
            state = anim.GetCurrentAnimatorStateInfo(0);
        }

        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRange);

        // Draw ground check
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void EnablePoundHitbox() => attackTrigger?.EnableDamage();
    public void DisablePoundHitbox() => attackTrigger?.DisableDamage();

    public void TakeDamage(int damage) => TakeHit(damage);
}
