using UnityEngine;

public class BossFollow : MonoBehaviour
{
    [SerializeField] private Transform target;     // Player's transform
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;       // Adjust in Inspector

    [Header("Y Axis Clamping")]
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Clamp Y value between minY and maxY
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
