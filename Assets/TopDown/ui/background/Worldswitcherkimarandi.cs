using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSwitcherkimarandi : MonoBehaviour
{
    public GameObject normalWorld;
    public GameObject glitchedWorld;

    private bool isGlitched = false;

    void Start()
    {
        // Ensure only one is active at start
        normalWorld.SetActive(true);
        glitchedWorld.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // Ignore if click/touch is over UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            ToggleWorld();
        }
    }

    void ToggleWorld()
    {
        isGlitched = !isGlitched;

        normalWorld.SetActive(!isGlitched);
        glitchedWorld.SetActive(isGlitched);
    }
}
