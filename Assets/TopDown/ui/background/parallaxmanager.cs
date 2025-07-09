// Attach this script to an empty GameObject called ParallaxManager
using UnityEngine;
using UnityEngine.EventSystems;

public class InfiniteParallaxSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class ScrollingLayer
    {
        public GameObject[] sprites;       // Two identical background pieces side-by-side
        public float scrollSpeed = 1f;     // Scroll speed for this layer
    }

    [System.Serializable]
    public class BackgroundSet
    {
        public ScrollingLayer[] layers;    // Each set has multiple parallax layers
    }

    public BackgroundSet[] backgroundSets;  // Two sets: Normal & Glitched
    private int activeSetIndex = 0;

    private float[,] layerWidths; // [backgroundSet, layer]

    void Start()
    {
        int setCount = backgroundSets.Length;
        layerWidths = new float[setCount, 10]; // Assuming max 10 layers per set

        for (int setIndex = 0; setIndex < setCount; setIndex++)
        {
            var set = backgroundSets[setIndex];
            for (int i = 0; i < set.layers.Length; i++)
            {
                var sprite = set.layers[i].sprites[0];
                layerWidths[setIndex, i] = sprite.GetComponent<SpriteRenderer>().bounds.size.x;
            }

            // Enable only the active set
            bool isActive = (setIndex == activeSetIndex);
            foreach (var layer in set.layers)
            {
                foreach (var sprite in layer.sprites)
                    sprite.SetActive(isActive);
            }
        }
    }

    void Update()
    {
        ScrollActiveBackground();

        if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject())
        {
            SwitchBackground();
        }
    }

    void ScrollActiveBackground()
    {
        var set = backgroundSets[activeSetIndex];

        for (int i = 0; i < set.layers.Length; i++)
        {
            var layer = set.layers[i];
            float width = layerWidths[activeSetIndex, i];

            foreach (var sprite in layer.sprites)
            {
                sprite.transform.position += Vector3.left * layer.scrollSpeed * Time.deltaTime;

                if (sprite.transform.position.x <= -width )
                {
                    float rightMost = GetRightmostX(layer.sprites);
                    sprite.transform.position = new Vector3(rightMost + width - 0.01f, sprite.transform.position.y, sprite.transform.position.z);
                }
            }
        }
    }

    float GetRightmostX(GameObject[] sprites)
    {
        float maxX = float.MinValue;
        foreach (var sprite in sprites)
        {
            if (sprite.transform.position.x > maxX)
                maxX = sprite.transform.position.x;
        }
        return maxX;
    }

    void SwitchBackground()
    {
        // Deactivate current
        foreach (var layer in backgroundSets[activeSetIndex].layers)
        {
            foreach (var sprite in layer.sprites)
                sprite.SetActive(false);
        }

        // Switch
        activeSetIndex = (activeSetIndex + 1) % backgroundSets.Length;

        // Activate new
        foreach (var layer in backgroundSets[activeSetIndex].layers)
        {
            foreach (var sprite in layer.sprites)
                sprite.SetActive(true);
        }
    }
}
