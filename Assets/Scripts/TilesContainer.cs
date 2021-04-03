using UnityEngine;


[CreateAssetMenu]
public class TilesContainer : ScriptableObject
{
    private static readonly ResourceAsset<TilesContainer> asset = new ResourceAsset<TilesContainer>("TilesContainer");

    [SerializeField] private Color mountainsColor = default;
    [SerializeField] private Color grassColor = default;
    [SerializeField] private Color seaColor = default;
    [SerializeField] private Color forestColor = default;
    [SerializeField] private Color borderColor = default;


    public static Color GetTileColor(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Forest:
                return asset.Value.forestColor;
            case TileType.Grass:
                return asset.Value.grassColor;
            case TileType.Mountain:
                return asset.Value.mountainsColor;
            case TileType.Sea:
                return asset.Value.seaColor;
            case TileType.Border:
                return asset.Value.borderColor;
        }

        return Color.white;
    }
}
