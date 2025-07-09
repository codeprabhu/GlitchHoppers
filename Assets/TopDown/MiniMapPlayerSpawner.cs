using UnityEngine;

public class MinimapPlayerSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject playerPrefab;
    public Transform defaultSpawnPoint;

    void Start()
    {
        Vector3 spawnPosition;

        // Use saved position if it exists
        if (GameManager.Instance != null && GameManager.Instance.lastMinimapPosition != Vector3.zero)
        {
            spawnPosition = GameManager.Instance.lastMinimapPosition;
        }
        else
        {
            spawnPosition = defaultSpawnPoint.position;
        }

        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}
