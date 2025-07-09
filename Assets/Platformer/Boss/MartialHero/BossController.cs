using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using System;

public class BossSamuraiController : MonoBehaviour
{
    [Header("Firebomb Hitbox")]
    public GameObject firebombHitbox;
    public float firebombDuration = 1.2f;

    [Header("Movement")]
    public float moveSpeed = 1f;
    public float flipSmoothTime = 0.1f;
    private float currentScaleXVelocity;

    [Header("Phases")]
    private int currentPhase = 1;
    private bool isDead = false;

    [Header("References")]
    public BossHealth bossHealth;

    [Header("Attack Settings")]
    public float timeBetweenAttacks = 2f;
    private float attackTimer = 3f;
    private bool isAttacking = false;

    [Header("Animation & Combat")]
    public Animator animator;
    public GameObject headHitbox;
    public Transform fireballSpawnPoint;
    public GameObject fireballPrefab;
    public Transform player;
    public GameObject whirlwindHitbox;

    [Header("Distance-Based Attack Ranges")]
    public float specialRange = 3f;
    public float whirlwindRange = 6f;
    public float fireBombRange = 9f;
    public float upperCutRange = 999f;

    [Header("Chase Behavior")]
    private bool facingRight = true;
    public float minDistanceToPlayer = 2.5f;
    [Header("Hug Knockback Settings")]
    public float hugDurationThreshold = 1f;
    public float knockbackForce = 20f;

    private float hugTimer = 0f;
    private bool isPlayerTouching = false;
    private Rigidbody2D playerRb;

    void FlipByDirection()
    {
        if (player == null) return;

        bool shouldFaceRight = player.position.x > transform.position.x;

        if (shouldFaceRight != facingRight)
        {
            // Flip only when direction changes
            facingRight = shouldFaceRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
    void Start()
    {
        headHitbox.SetActive(true);
        EnterPhase(1);
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
    }

    void Update()
    {
        if (isDead) return;

        Move();
        HandleAttackCycle();
    }

    void Move()
    {
        if (!player) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance > minDistanceToPlayer)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", true);
        }

        FlipByDirection();
    }
    void HandleAttackCycle()
    {
        attackTimer += Time.deltaTime;

        if (!isAttacking && attackTimer >= timeBetweenAttacks)
        {
            attackTimer = 0f;
            PerformDistanceBasedAttack();
        }
    }

    void PerformDistanceBasedAttack()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);
        string chosenAttack;

        if (distance > fireBombRange)
            chosenAttack = "Attack_UpperCut";
        else if (distance > whirlwindRange)
            chosenAttack = "Attack_FireBomb";
        else if (distance > specialRange)
            chosenAttack = "Attack_Whirlwind";
        else
            chosenAttack = "Attack_Special";

        Debug.Log("[BOSS] Chose attack: " + chosenAttack + " at distance " + distance);

        isAttacking = true;
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", false);
        animator.SetTrigger(chosenAttack);

        // Reset state after delay based on chosen attack
        float attackDuration = chosenAttack switch
        {
            "Attack_Special" => 1.6f,
            "Attack_Whirlwind" => 2.6f,
            "Attack_FireBomb" => 1.2f,
            "Attack_UpperCut" => 2.1f,
            _ => 1.2f
        };

        Invoke(nameof(ResetAttackState), attackDuration);
    }

    void ResetAttackState()
    {
        isAttacking = false;
        animator.SetBool("isRunning", true);
        animator.SetBool("isIdle", false);
    }

    public void EnterPhase(int phase)
    {
        currentPhase = phase;

        switch (phase)
        {
            case 1:
                timeBetweenAttacks = 8f;
                moveSpeed = 3f;
                break;
            case 2:
                timeBetweenAttacks = 7f;
                moveSpeed = 5f;
                break;
            case 3:
                timeBetweenAttacks = 5f;
                moveSpeed = 7f;
                break;
        }

        //Debug.Log($"Boss entered Phase {phase}");
    }

    public void TakeHeadDamage(int damage)
    {
        if (isDead) return;

        bossHealth.TakeDamage(damage);
        animator.SetTrigger("Hurt");
    }

    public void TakeBodyHit()
    {
        if (isDead) return;
        animator.SetTrigger("Defend");
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        headHitbox.SetActive(false);
        Destroy(whirlwindHitbox);
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", false);
        animator.SetTrigger("Death");
        Debug.Log("Boss defeated.");
        StartCoroutine(HandleDeathCutscene());
    }

    private IEnumerator HandleDeathCutscene()
    {
        yield return new WaitForSeconds(5f);
        CutsceneManager.Instance?.PlayFinalBossCutscene();
    }

    public void NormalAttack1()
    {
        if (fireballPrefab && player)
        {
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
            Vector2 dir = (player.position - fireballSpawnPoint.position).normalized;
            fireball.GetComponent<Rigidbody2D>().linearVelocity = dir * 8f;
        }
    }
    public void FireballSpawn()
    {
        if (fireballPrefab && player && fireballSpawnPoint)
        {
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
            FireballScript fbScript = fireball.GetComponent<FireballScript>();

            if (fbScript != null)
                fbScript.player = player;
        }
    }


    public void FirebombTrigger()
    {
        if (firebombHitbox != null)
        {
            firebombHitbox.SetActive(true);
            StartCoroutine(DisableFirebombAfterDelay());
        }
    }

    private IEnumerator DisableFirebombAfterDelay()
    {
        yield return new WaitForSeconds(firebombDuration);
        if (firebombHitbox != null)
            firebombHitbox.SetActive(false);
        ResetAttackState();
    }

    public void SpecialAttack()
    {
        Debug.Log("SPECIAL ATTACK: Flaming sword unleashed.");
        // Do damage etc.
    }
    public void WhirlwindRoutine()
    {
        StartCoroutine(PerformWhirlwindRoutine());
    }

    private IEnumerator PerformWhirlwindRoutine()
    {
        if (whirlwindHitbox != null)
        {
            whirlwindHitbox.SetActive(true);
            yield return new WaitForSeconds(1.1f);
            whirlwindHitbox.SetActive(false);
        }

        ResetAttackState();
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (!playerRb)
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        isPlayerTouching = true;
        hugTimer += Time.deltaTime;

        if (hugTimer >= hugDurationThreshold)
        {
            ApplyKnockbackToPlayer(collision.gameObject.transform);
            hugTimer = 0f;
            isPlayerTouching = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            isPlayerTouching = false;
            hugTimer = 0f;
        }
    }
    void ApplyKnockbackToPlayer(Transform target)
    {
        if (!playerRb) return;

        Vector2 knockDir = (target.position - transform.position).normalized;
        knockDir.y = 100f; // Optional upward component
        knockDir.Normalize();

        playerRb.linearVelocity = Vector2.zero; // Reset first
        playerRb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

        Debug.Log("[BOSS] Player hugged too long — Knockback!");
    }


    private void OnDrawGizmosSelected()
    {
        if (!player) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, specialRange);

        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(transform.position, whirlwindRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fireBombRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, upperCutRange);
    }

}
