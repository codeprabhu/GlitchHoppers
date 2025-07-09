using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapReplicator : MonoBehaviour
{
    public Tilemap templateTilemap;
    public Tilemap baseTilemap;
    public Transform player;

    public int chunkWidth = 20; // Width of one chunk in tiles
    public int bufferChunks = 2;

    private HashSet<int> spawnedChunks = new HashSet<int>();

    void Start()
    {
        ReplicateChunkAt(0); // Start with center chunk
    }

    void Update()
    {
        int playerChunk = Mathf.FloorToInt(player.position.x / chunkWidth);

        for (int offset = -bufferChunks; offset <= bufferChunks; offset++)
        {
            int chunkIndex = playerChunk + offset;
            if (!spawnedChunks.Contains(chunkIndex))
            {
                ReplicateChunkAt(chunkIndex);
            }
        }
    }

    void ReplicateChunkAt(int chunkIndex)
    {
        Vector3Int offset = new Vector3Int(chunkIndex * chunkWidth, 0, 0);

        BoundsInt bounds = templateTilemap.cellBounds;
        TileBase[] tiles = templateTilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3Int localPos = new Vector3Int(x, y, 0);
                    Vector3Int newPos = bounds.position + localPos + offset;
                    baseTilemap.SetTile(newPos, tile);
                }
            }
        }

        spawnedChunks.Add(chunkIndex);
    }
}
