using UnityEngine;
using UnityEditor;


public class TileTextureData : ScriptableObject
{
    [SerializeField] private TileType[] tiles = default;
    [SerializeField] private Vector2Int resolution = default;


    public Vector2Int Resolution => resolution;


    public TileType GetTileType(int index) => tiles[index];

    public void SetData(TileType[] tiles)
    {
        this.tiles = tiles;
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }


    public void SetResolution(Vector2Int resolution)
    {
        this.resolution = resolution;
        tiles = new TileType[resolution.x * resolution.y];
    }
}
