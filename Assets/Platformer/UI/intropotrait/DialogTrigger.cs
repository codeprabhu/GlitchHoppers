using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] dialogueLines; // unique text for this trigger

    private bool hasBeenUsed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenUsed) return;

        if (other.CompareTag("Player"))
        {
            hasBeenUsed = true;
            DialogManager.Instance.StartDialog(dialogueLines);
        }
    }
}
