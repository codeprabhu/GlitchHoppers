using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    public float parallaxFactor = 0.5f;
    public float textureUnitSizeX;

    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;

        // Get width of sprite/texture
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            textureUnitSizeX = sr.sprite.bounds.size.x * transform.localScale.x;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPos;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
        lastCamPos = cam.position;

        // Looping
        float offset = cam.position.x - transform.position.x;
        if (Mathf.Abs(offset) >= textureUnitSizeX)
        {
            float shift = (offset > 0 ? 1 : -1) * textureUnitSizeX;
            transform.position += new Vector3(shift, 0, 0);
        }
    }
}
