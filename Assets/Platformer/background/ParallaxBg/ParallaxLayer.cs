using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;
    public Transform cameraTransform;

    private Vector3 lastCameraPosition;
    private float spriteWidth;

    private Transform[] children;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;

        // Get children transforms (background slices)
        int count = transform.childCount;
        children = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // Assuming all child sprites have same width, get width of first child sprite renderer
        SpriteRenderer sr = children[0].GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;
        float movement = deltaX * parallaxSpeed;

        // Move the entire layer by parallax amount
        transform.position += new Vector3(movement, 0, 0);

        // Loop each child sprite if it goes offscreen (to the left)
        foreach (var child in children)
        {
            float distanceToCamera = child.position.x - cameraTransform.position.x;

            if (distanceToCamera < -spriteWidth)
            {
                // Child is too far left — move it to the rightmost edge
                float rightmostX = GetRightmostChildX();
                child.position = new Vector3(rightmostX + spriteWidth, child.position.y, child.position.z);
            }
            else if (distanceToCamera > spriteWidth)
            {
                // Child is too far right — move it to the leftmost edge
                float leftmostX = GetLeftmostChildX();
                child.position = new Vector3(leftmostX - spriteWidth, child.position.y, child.position.z);
            }
        }


        lastCameraPosition = cameraTransform.position;
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
    float GetLeftmostChildX()
    {
        float minX = float.MaxValue;
        foreach (var child in children)
        {
            if (child.position.x < minX)
                minX = child.position.x;
        }
        return minX;
    }

}
