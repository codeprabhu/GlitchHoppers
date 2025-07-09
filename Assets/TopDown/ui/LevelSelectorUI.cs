using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectorUI : MonoBehaviour
{
    [System.Serializable]
    public class LevelData
    {
        public string levelName;
        public string description;
        public string sceneName;
    }

    public GameObject levelInfoPanel;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelDescriptionText;

    private string sceneToLoad;

    public void ShowLevelInfo(LevelData data)
    {
        sceneToLoad = data.sceneName;
        levelNameText.text = data.levelName;
        levelDescriptionText.text = data.description;
        levelInfoPanel.SetActive(true);
    }

    public void EnterLevel()
{
    if (!string.IsNullOrEmpty(sceneToLoad))
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && GameManager.Instance != null)
        {
            GameManager.Instance.UpdateMinimapPosition(player.transform.position);
            Debug.Log("Saved minimap position: " + player.transform.position);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}


    public void ClosePopup()
    {
        levelInfoPanel.SetActive(false);
        sceneToLoad = "";
    }
}
