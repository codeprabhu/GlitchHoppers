using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
    private bool phase2Triggered = false;
    private bool phase3Triggered = false;

    [Header("Backgrounds")]
    public GameObject backgroundPhase1;
    public GameObject backgroundPhase2;
    public GameObject backgroundPhase3;
    public ScreenFlashController screenFlash;
    public BossShake screenShake;

    [Header("Boss Reference")]
    public BossSamuraiController bossController;


    void Start()
    {
        // Start with phase 1 background
        ActivateBackground(backgroundPhase1);
    }

    public void CheckPhase(int currentHealth)
    {
        if (!phase2Triggered && currentHealth <= 200)
        {
            phase2Triggered = true;
            screenFlash.Flash();
            screenShake.Shake();
            EnterPhase2();
        }
        else if (!phase3Triggered && currentHealth <= 100)
        {
            phase3Triggered = true;
            screenFlash.Flash();
            screenShake.Shake();
            EnterPhase3();
        }
    }

    void EnterPhase2()
    {
        Debug.Log("Entering Phase 2");
        ActivateBackground(backgroundPhase2);
        bossController.EnterPhase(2);
        // Add other changes like boss behavior
    }

    void EnterPhase3()
    {
        Debug.Log("Entering Phase 3");
        ActivateBackground(backgroundPhase3);
        bossController.EnterPhase(3);
        // Add other changes like attack speed, effects
    }

    void ActivateBackground(GameObject bgToEnable)
    {
        backgroundPhase1.SetActive(bgToEnable == backgroundPhase1);
        backgroundPhase2.SetActive(bgToEnable == backgroundPhase2);
        backgroundPhase3.SetActive(bgToEnable == backgroundPhase3);
    }
}
