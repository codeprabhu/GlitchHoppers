using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int currentCoins = 0;
    public int totalCoins = 0;

    public string[] dialogueLines;
    public TMP_Text coinCountText;

    private HashSet<string> collectedCoinIDs = new HashSet<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdateCoinUI();
    }

    public void AddCoins(string coinID)
    {
        if (collectedCoinIDs.Contains(coinID)) return;

        collectedCoinIDs.Add(coinID);
        currentCoins++;
        UpdateCoinUI();

        if (currentCoins >= totalCoins)
        {
            OnAllCoinsCollected();
        }
    }

    public bool HasCollected(string coinID)
    {
        return collectedCoinIDs.Contains(coinID);
    }

    void UpdateCoinUI()
    {
        coinCountText.text = currentCoins + " / " + totalCoins;
    }

    void OnAllCoinsCollected()
    {
        Debug.Log("All coins collected!");
        DialogManager.Instance.StartDialog(dialogueLines);
    }
}
