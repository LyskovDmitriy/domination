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
                return asset.Instance.forestColor;
            case TileType.Grass:
                return asset.Instance.grassColor;
            case TileType.Mountain:
                return asset.Instance.mountainsColor;
            case TileType.Sea:
                return asset.Instance.seaColor;
            case TileType.Border:
                return asset.Instance.borderColor;
        }

        return Color.white;
    }
}
