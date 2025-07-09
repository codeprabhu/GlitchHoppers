using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public RectTransform minimapPanel;
    public RectTransform playerDot;
    public string playerTag = "Player";

    public Transform worldMinTransform; // Drag your bottom-left GameObject here
    public Transform worldMaxTransform; // Drag your top-right GameObject here

    private Transform player;
    private bool dotActive = false;

    void Update()
    {
        // Try to find player once
        if (!dotActive)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag(playerTag);
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
                playerDot.gameObject.SetActive(true);
                dotActive = true;
            }
        }

        // Don't proceed if player or bounds are missing
        if (!dotActive || player == null || worldMinTransform == null || worldMaxTransform == null)
            return;

        // Use assigned transforms for world bounds
        Vector2 worldMin = new Vector2(worldMinTransform.position.x, worldMinTransform.position.y);
        Vector2 worldMax = new Vector2(worldMaxTransform.position.x, worldMaxTransform.position.y);
        Vector2 worldSize = worldMax - worldMin;

        Vector2 playerPos = new Vector2(player.position.x, player.position.y) - worldMin;
        Vector2 normalized = new Vector2(playerPos.x / worldSize.x, playerPos.y / worldSize.y);
        Vector2 minimapSize = minimapPanel.rect.size;
        Vector2 minimapPos = new Vector2(normalized.x * minimapSize.x, normalized.y * minimapSize.y);

        playerDot.anchoredPosition = minimapPos;
    }
}
