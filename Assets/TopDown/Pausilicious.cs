using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pausilicious: MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button exitButton;
    public Button muteButton;
    
    [Header("Icons")]
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    [Header("Audio")]
    public AudioSource musicSource;

    private bool isPaused = false;
    private bool isMuted = false;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        exitButton.onClick.AddListener(ExitGame);
        muteButton.onClick.AddListener(ToggleMute);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your actual first scene name
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        musicSource.mute = isMuted;
        muteButton.image.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}
