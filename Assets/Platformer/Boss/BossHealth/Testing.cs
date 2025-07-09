using UnityEngine;

public class TestBossDamage : MonoBehaviour
{
    public BossHealth boss;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boss.TakeDamage(30);
        }
    }
}
