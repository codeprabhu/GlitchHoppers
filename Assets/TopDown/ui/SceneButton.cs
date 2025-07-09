using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class SceneButton : MonoBehaviour
{
    public string sceneToLoad;
    public string levelName;
    [TextArea]
    public string levelDescription;

    public Color hoverColor = Color.yellow;
    private Color originalColor;

    private SpriteRenderer spriteRenderer;
    private LevelSelectorUI uiManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        uiManager = FindFirstObjectByType<LevelSelectorUI>();
    }

    private void OnMouseEnter()
    {
        spriteRenderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }

    private void OnMouseDown()
    {
        if (uiManager == null)
        {
            Debug.LogWarning("No LevelSelectorUI found.");
            return;
        }

        LevelSelectorUI.LevelData data = new LevelSelectorUI.LevelData
        {
            levelName = levelName,
            description = levelDescription,
            sceneName = sceneToLoad
        };

        uiManager.ShowLevelInfo(data);
    }
}
