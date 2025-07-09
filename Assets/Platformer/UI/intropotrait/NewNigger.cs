using UnityEngine;
using System.Collections.Generic;

public class DialogTriggerSessionOnly : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Tooltip("Unique ID for this dialog within the game session")]
    public string dialogID = "dialog_intro_001";

    private bool hasBeenUsed = false;

    // Static list = survives across scenes, resets on app restart
    private static HashSet<string> triggeredDialogs = new HashSet<string>();

    private void Start()
    {
        if (triggeredDialogs.Contains(dialogID))
        {
            Destroy(gameObject); // already triggered this dialog this session
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenUsed) return;

        if (other.CompareTag("Player"))
        {
            hasBeenUsed = true;

            // Show the dialog
            DialogManager.Instance.StartDialog(dialogueLines);

            // Mark it as used for this session
            triggeredDialogs.Add(dialogID);

            Destroy(gameObject, 0.5f); // optional
        }
    }
}
