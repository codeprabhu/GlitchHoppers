using UnityEngine;

public class Parallaxkimarandi : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;
    public float scrollSpeed = 1f;

    private float spriteWidth;
    private Transform[] children;

    void Start()
    {
        // Get child references
        int count = transform.childCount;
        children = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // Assume all backgrounds have same width
        SpriteRenderer sr = children[0].GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // Scroll the parent layer leftwards
        transform.position += Vector3.left * scrollSpeed * parallaxSpeed * Time.deltaTime;

        // Recycle children when they go offscreen to the left
        foreach (var child in children)
        {
            // World X position relative to camera view
            float camX = Camera.main.transform.position.x;
            float distance = child.position.x - camX;

            if (distance < -spriteWidth)
            {
                // Move this child to the rightmost child + width
                float rightMost = GetRightmostChildX();
                child.position = new Vector3(rightMost + spriteWidth, child.position.y, child.position.z);
            }
        }
    }

    float GetRightmostChildX()
    {
        float maxX = float.MinValue;
        foreach (var child in children)
        {
            if (child.position.x > maxX)
                maxX = child.position.x;
        }
        return maxX;
    }
}
