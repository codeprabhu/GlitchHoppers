using UnityEngine;

public class TransformManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boatPrefab;
    public Transform cameraTarget;  // Assign this in the Inspector

    private GameObject currentForm;
    public Transform defaultspawn;
    private bool isBoat = false;

    void Start()
    {
        Vector3 spawnPos = defaultspawn.position;

        // Use saved position from GameManager if it exists
        if (GameManager.Instance != null)
        {
            spawnPos = GameManager.Instance.lastMinimapPosition;
            //Debug.Log("TransformManager: Spawning player at saved position.");
        }

        // Avoid double spawn if one already exists
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            currentForm = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            currentForm = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        // Transform when Tab is pressed
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (isBoat)
                TransformToPlayer();
            else
                TransformToBoat();
        }

        // Keep camera target updated
        if (cameraTarget != null && currentForm != null)
        {
            cameraTarget.position = currentForm.transform.position;
        }
    }

    void TransformToBoat()
    {
        Vector3 pos = currentForm.transform.position;
        Destroy(currentForm);
        currentForm = Instantiate(boatPrefab, pos, Quaternion.identity);
        isBoat = true;
    }

    void TransformToPlayer()
    {
        Vector3 pos = currentForm.transform.position;
        Destroy(currentForm);
        currentForm = Instantiate(playerPrefab, pos, Quaternion.identity);
        isBoat = false;
    }
}
