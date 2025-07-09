using System.Collections.Generic;
using UnityEngine;

public class TopDown : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;

    // Walking animations
    public List<Sprite> nSprites, sSprites, seSprites, eSprites, neSprites;

    // Running animations
    public List<Sprite> runNSprites, runSSprites, runSESprites, runESprites, runNESprites;

    // Idle animations (3 variations per direction)
    public List<Sprite> idleN, idleN_Alt1, idleN_Alt2;
    public List<Sprite> idleNE, idleNE_Alt1, idleNE_Alt2;
    public List<Sprite> idleE, idleE_Alt1, idleE_Alt2;
    public List<Sprite> idleSE, idleSE_Alt1, idleSE_Alt2;
    public List<Sprite> idleS, idleS_Alt1, idleS_Alt2;

    public float walkSpeed;
    public float runSpeedMultiplier = 2.4f;
    public float frameRate;
    public float idleThreshold = 3.0f; // Time before switching to random idle animation
    public float idleSwapInterval = 5.0f; // Time between random idle swaps
    public float defaultIdleChance = 0.6f; // 60% chance to return to default idle animation
    private int defaultIdleCount = 0; // Tracks number of default idle cycles
    private const int defaultIdleLimit = 3; // Ensures 3 default idle cycles before an alternate one

    private Vector2 direction;
    private float animationTimer;
    private List<Sprite> lastMoveSprites;
    private bool isMoving;
    private bool isRunning;
    private float idleTimer;
    private bool hasPlayedAltIdle;
    private float nextIdleSwapTime;
    private List<Sprite> currentIdleAnimation;

    void Start()
    {
        lastMoveSprites = idleE; // Default idle direction: East
        currentIdleAnimation = idleE;
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        isMoving = direction.magnitude > 0;
        isRunning = Input.GetKey(KeyCode.LeftShift); // Hold Shift to run

        if (isMoving)
        {
            idleTimer = 0; // Reset idle time
            hasPlayedAltIdle = false; // Reset alternate idle state
            defaultIdleCount = 0; // Reset default idle counter
            float speed = isRunning ? walkSpeed * runSpeedMultiplier : walkSpeed;
            body.linearVelocity = direction * speed;
            lastMoveSprites = GetSpriteDirection(); // Store last movement direction
            currentIdleAnimation = GetIdleSprites(); // Reset to default idle
        }
        else
        {
            body.linearVelocity = Vector2.zero; // Stop moving
            idleTimer += Time.deltaTime; // Increase idle time

            if (idleTimer >= idleThreshold && Time.time >= nextIdleSwapTime)
            {
                if (!hasPlayedAltIdle && Random.value > defaultIdleChance && defaultIdleCount >= defaultIdleLimit)
                {
                    currentIdleAnimation = GetRandomIdleAnimation(); // Alternate idle
                    hasPlayedAltIdle = true; // Mark alternate idle as played
                    defaultIdleCount = 0; // Reset default idle counter
                }
                else
                {
                    currentIdleAnimation = GetIdleSprites(); // Default idle
                    hasPlayedAltIdle = false; // Reset alternate idle flag
                    defaultIdleCount++; // Increment default idle counter
                }
                nextIdleSwapTime = Time.time + idleSwapInterval; // Schedule next switch
            }
        }

        HandleSpriteFlip();
        SetSprite();
    }

    void SetSprite()
    {
        List<Sprite> currentSprites;

        if (isMoving) // Walking or running animations
        {
            currentSprites = GetSpriteDirection();
        }
        else
        {
            currentSprites = currentIdleAnimation;
        }

        if (currentSprites != null && currentSprites.Count > 0)
        {
            int frame = (int)(animationTimer * frameRate) % currentSprites.Count;
            spriteRenderer.sprite = currentSprites[frame];
            animationTimer += Time.deltaTime;
        }
    }

    List<Sprite> GetSpriteDirection()
    {
        if (isRunning) // Running animations
        {
            if (direction.y > 0) return Mathf.Abs(direction.x) > 0 ? runNESprites : runNSprites;
            if (direction.y < 0) return Mathf.Abs(direction.x) > 0 ? runSESprites : runSSprites;
            return runESprites;
        }
        else // Walking animations
        {
            if (direction.y > 0) return Mathf.Abs(direction.x) > 0 ? neSprites : nSprites;
            if (direction.y < 0) return Mathf.Abs(direction.x) > 0 ? seSprites : sSprites;
            return eSprites;
        }
    }

    List<Sprite> GetIdleSprites()
    {
        if (lastMoveSprites == nSprites || lastMoveSprites == runNSprites) return idleN;
        if (lastMoveSprites == sSprites || lastMoveSprites == runSSprites) return idleS;
        if (lastMoveSprites == seSprites || lastMoveSprites == runSESprites) return idleSE;
        if (lastMoveSprites == eSprites || lastMoveSprites == runESprites) return idleE;
        if (lastMoveSprites == neSprites || lastMoveSprites == runNESprites) return idleNE;
        return idleE; // Default idle animation
    }

    List<Sprite> GetRandomIdleAnimation()
    {
        int randomIdle = Random.Range(0, 2); // Pick between the two extra idle animations
        if (currentIdleAnimation == idleN) return randomIdle == 0 ? idleN_Alt1 : idleN_Alt2;
        if (currentIdleAnimation == idleS) return randomIdle == 0 ? idleS_Alt1 : idleS_Alt2;
        if (currentIdleAnimation == idleSE) return randomIdle == 0 ? idleSE_Alt1 : idleSE_Alt2;
        if (currentIdleAnimation == idleE) return randomIdle == 0 ? idleE_Alt1 : idleE_Alt2;
        if (currentIdleAnimation == idleNE) return randomIdle == 0 ? idleNE_Alt1 : idleNE_Alt2;
        return idleE; // Default fallback
    }

    void HandleSpriteFlip()
    {
        if (!spriteRenderer.flipX && direction.x < 0)
            spriteRenderer.flipX = true;
        else if (spriteRenderer.flipX && direction.x > 0)
            spriteRenderer.flipX = false;
    }
}
