using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private float autoCloseTime = 6f; // seconds before auto-close
private Coroutine autoCloseCoroutine;

    public static DialogManager Instance;

    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public Image portraitImage;
    public GameObject continuePrompt;

    [SerializeField] private float typeSpeed = 0.05f;

    private string[] lines;
    private int index;

    private bool isTyping = false;
    private bool canContinue = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogPanel.SetActive(false);
        continuePrompt.SetActive(false);
    }

    public void StartDialog(string[] dialogLines)
    {
        lines = dialogLines;
        index = 0;
        dialogPanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    
    void Update()
    {
        if (dialogPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Return) && canContinue)
        {
            continuePrompt.SetActive(false);
            index++;
            if (index < lines.Length)
                StartCoroutine(TypeLine());
            else
                EndDialog();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        canContinue = false;
        dialogText.text = "";

        foreach (char c in lines[index])
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        canContinue = true;
        continuePrompt.SetActive(true);

        // Start auto-close countdown
        if (autoCloseCoroutine != null)
            StopCoroutine(autoCloseCoroutine);

        autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);

    }

    IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseTime);

        if (canContinue && dialogPanel.activeInHierarchy)
        {
            EndDialog();
        }
    }

}
