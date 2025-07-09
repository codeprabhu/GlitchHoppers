using UnityEngine;

public class Coin : MonoBehaviour
{
    public string coinID; // Must be unique (e.g., "coin_1_2")

    void Start()
    {
        if (CoinManager.instance != null && CoinManager.instance.HasCollected(coinID))
        {
            Destroy(gameObject); // Already collected this session
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.instance.AddCoins(coinID);
            Destroy(gameObject);
        }
    }
}
