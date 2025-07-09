using UnityEngine;

public class DirectionalColliders5 : MonoBehaviour
{
    public GameObject colliderN, colliderNE, colliderE, colliderSE,
                      colliderS, colliderSW, colliderW, colliderNW;

    private Vector2 lastDirection = Vector2.down;

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input != Vector2.zero)
            lastDirection = input.normalized;

        UpdateColliders(lastDirection);
    }

    void UpdateColliders(Vector2 dir)
    {
        DisableAllColliders();

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        if (angle >= 337.5f || angle < 22.5f)
            colliderE.SetActive(true);
        else if (angle >= 22.5f && angle < 67.5f)
            colliderNE.SetActive(true);
        else if (angle >= 67.5f && angle < 112.5f)
            colliderN.SetActive(true);
        else if (angle >= 112.5f && angle < 157.5f)
            colliderNW.SetActive(true);
        else if (angle >= 157.5f && angle < 202.5f)
            colliderW.SetActive(true);
        else if (angle >= 202.5f && angle < 247.5f)
            colliderSW.SetActive(true);
        else if (angle >= 247.5f && angle < 292.5f)
            colliderS.SetActive(true);
        else // 292.5f to 337.5f
            colliderSE.SetActive(true);
    }

    void DisableAllColliders()
    {
        colliderN.SetActive(false);
        colliderNE.SetActive(false);
        colliderE.SetActive(false);
        colliderSE.SetActive(false);
        colliderS.SetActive(false);
        colliderSW.SetActive(false);
        colliderW.SetActive(false);
        colliderNW.SetActive(false);
    }
}
