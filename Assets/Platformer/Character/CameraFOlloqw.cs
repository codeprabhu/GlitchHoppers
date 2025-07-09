using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public Vector2 minPosition;
    public Vector2 maxPosition;

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position based on target and offset
        Vector3 desiredPosition = target.position + offset;

        // Clamp within camera bounds if needed
        float clampedX = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, transform.position.z);

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);
    }
}
