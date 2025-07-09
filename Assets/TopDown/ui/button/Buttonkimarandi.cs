using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string sceneToLoad = "Glitch"; // Replace with your scene name
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    public float scaleSpeed = 10f;
    private bool isHovered = false;

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, isHovered ? hoverScale : normalScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting");
    }
}
