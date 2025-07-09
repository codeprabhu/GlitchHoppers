using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D), typeof(SpriteRenderer))]
public class SyncColliderToSprite : MonoBehaviour
{
    private PolygonCollider2D polyCollider;
    private SpriteRenderer spriteRenderer;
    private Sprite lastSprite;

    void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        Sprite current = spriteRenderer.sprite;
        if (current != lastSprite)
        {
            lastSprite = current;
            UpdateColliderShape(current);
        }
    }

    void UpdateColliderShape(Sprite sprite)
    {
        polyCollider.pathCount = sprite.GetPhysicsShapeCount();

        var shape = new List<Vector2>();
        for (int i = 0; i < polyCollider.pathCount; i++)
        {
            shape.Clear();
            sprite.GetPhysicsShape(i, shape);
            polyCollider.SetPath(i, shape);
        }
    }
}
