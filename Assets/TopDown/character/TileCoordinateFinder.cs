using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCoordinateFinder : MonoBehaviour
{
    public Tilemap tilemap;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
            Debug.Log("Clicked cell: " + cellPos);
        }
    }
}
