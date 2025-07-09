using UnityEngine;
using System.Collections.Generic;

public class TilemapLayoutManager : MonoBehaviour
{
    [Header("Tilemap Layouts")]
    public List<GameObject> tilemapLayouts;  // Drag Layout1 - Layout4 here
    public float switchInterval = 10f;

    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        ActivateLayout(0);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % tilemapLayouts.Count;
            ActivateLayout(currentIndex);
        }
    }

    void ActivateLayout(int index)
    {
        for (int i = 0; i < tilemapLayouts.Count; i++)
        {
            tilemapLayouts[i].SetActive(i == index);
        }
    }
}
