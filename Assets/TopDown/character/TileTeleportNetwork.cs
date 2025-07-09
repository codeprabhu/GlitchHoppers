using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileTeleportNetwork : MonoBehaviour
{
    public TransformManager transformManager;
    public Tilemap tilemap;

    [System.Serializable]
    public class TeleportPair
    {
        public Vector3Int leafTileCell;
        public Vector3Int waterTileCell;
    }

    public List<TeleportPair> teleportPairs = new List<TeleportPair>();

    private Dictionary<Vector3Int, Vector3Int> teleportMap;

    void Start()
    {
        teleportMap = new Dictionary<Vector3Int, Vector3Int>();
        foreach (var pair in teleportPairs)
        {
            teleportMap[pair.leafTileCell] = pair.waterTileCell;
            teleportMap[pair.waterTileCell] = pair.leafTileCell; // Reverse mapping
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject currentForm = GetCurrentForm();
            if (currentForm == null || tilemap == null)
                return;

            Vector3 worldPos = currentForm.transform.position;
            Vector3Int currentCell = tilemap.WorldToCell(worldPos);

            foreach (var pair in teleportPairs)
            {
                // Check if player is within 2x2 tile radius (4x4 area centered on teleport tile)
                if (IsWithinArea(currentCell, pair.leafTileCell, 2))
                {
                    currentForm.transform.position = tilemap.GetCellCenterWorld(pair.waterTileCell);
                    return;
                }
                else if (IsWithinArea(currentCell, pair.waterTileCell, 2))
                {
                    currentForm.transform.position = tilemap.GetCellCenterWorld(pair.leafTileCell);
                    return;
                }
            }
        }
    }

    bool IsWithinArea(Vector3Int playerCell, Vector3Int targetCell, int radius)
    {
        return Mathf.Abs(playerCell.x - targetCell.x) < radius &&
               Mathf.Abs(playerCell.y - targetCell.y) < radius;
    }

    GameObject GetCurrentForm()
    {
        return (GameObject)typeof(TransformManager)
            .GetField("currentForm", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(transformManager);
    }
}
