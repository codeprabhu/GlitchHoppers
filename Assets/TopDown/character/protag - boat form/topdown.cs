using System.Collections.Generic;
using UnityEngine;

public class topdown : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;

    // Walking animations
    public List<Sprite> nSprites, sSprites, swSprites, wSprites, nwSprites;

    // Idle animations
    public List<Sprite> idlen, idles, idlesw, idlew, idlenw;

    public float walkSpeed = 2f;
    public float frameRate = 8f;

    private Vector2 direction;
    private Vector2 lastDirection;
    private float animationTimer;
    private bool isMoving;
    private List<Sprite> lastMoveSprites; // Store the last movement sprites used

    private enum FacingDirection { N, S, SW, W, NW }
    private FacingDirection lastFacing;

    void Start()
    {
        lastFacing = FacingDirection.W;
        lastDirection = Vector2.left;
        lastMoveSprites = wSprites; // Default to west sprites
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        isMoving = direction.magnitude > 0;

        if (isMoving)
        {
            body.linearVelocity = direction * walkSpeed;
            lastDirection = direction;
            lastFacing = GetDirectionFromInput(direction);
            lastMoveSprites = GetWalkSprites(lastFacing); // Store the sprites we're currently using
        }
        else
        {
            body.linearVelocity = Vector2.zero;
        }

        HandleSpriteFlip();
        SetSprite();
    }

    void SetSprite()
    {
        List<Sprite> sprites = isMoving ? GetWalkSprites(lastFacing) : GetIdleSprites();

        if (sprites != null && sprites.Count > 0)
        {
            int frame = (int)(animationTimer * frameRate) % sprites.Count;
            spriteRenderer.sprite = sprites[frame];
            animationTimer += Time.deltaTime;
        }
    }

    void HandleSpriteFlip()
    {
        float flipX = isMoving ? direction.x : lastDirection.x;
        bool shouldFlip = flipX > 0;
        spriteRenderer.flipX = shouldFlip;
    }

    FacingDirection GetDirectionFromInput(Vector2 dir)
    {
        if (dir.y > 0) // Moving up
        {
            if (dir.x < 0) return FacingDirection.NW;  // Up-left
            else if (dir.x > 0) return FacingDirection.NW; // Up-right (use NW sprites, flip handled by HandleSpriteFlip)
            else return FacingDirection.N; // Straight up
        }
        else if (dir.y < 0) // Moving down
        {
            if (dir.x < 0) return FacingDirection.SW;  // Down-left
            else if (dir.x > 0) return FacingDirection.SW; // Down-right (use SW sprites, flip handled by HandleSpriteFlip)
            else return FacingDirection.S; // Straight down
        }
        else // Moving horizontally only
        {
            if (dir.x != 0) return FacingDirection.W; // Left or right (flip handled by HandleSpriteFlip)
            else return lastFacing; // No input, keep last direction
        }
    }

    List<Sprite> GetWalkSprites(FacingDirection dir)
    {
        switch (dir)
        {
            case FacingDirection.N: return nSprites;
            case FacingDirection.S: return sSprites;
            case FacingDirection.SW: return swSprites;
            case FacingDirection.W: return wSprites;
            case FacingDirection.NW: return nwSprites;
            default: return wSprites;
        }
    }

    List<Sprite> GetIdleSprites()
    {
        // Use the lastMoveSprites to determine which idle animation to play
        if (lastMoveSprites == nSprites) return idlen;
        if (lastMoveSprites == sSprites) return idles;
        if (lastMoveSprites == swSprites) return idlesw;
        if (lastMoveSprites == wSprites) return idlew;
        if (lastMoveSprites == nwSprites) return idlenw;
        return idlew; // Default fallback
    }
}