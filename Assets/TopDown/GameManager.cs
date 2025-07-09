using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 lastMinimapPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // keep this alive between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateMinimapPosition(Vector3 position)
    {
        lastMinimapPosition = position;
        //Debug.Log("GameManager: Updated minimap position to " + position);
    }
}
