using UnityEngine;

public class WorldSwitcher : MonoBehaviour
{
    public GameObject normalWorld;
    public GameObject glitchedWorld;
    public KeyCode switchKey = KeyCode.Tab;

    private bool isInGlitchWorld = false;

    void Start()
    {
        SetActiveWorld(false); // Start in Normal World
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            isInGlitchWorld = !isInGlitchWorld;
            SetActiveWorld(isInGlitchWorld);
        }
    }

    void SetActiveWorld(bool glitch)
    {
        // Toggle entire world objects
        normalWorld.SetActive(!glitch);
        glitchedWorld.SetActive(glitch);
    }
}
