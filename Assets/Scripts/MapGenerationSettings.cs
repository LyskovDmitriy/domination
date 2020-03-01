using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class MapGenerationSettings : ScriptableObject
{
    private static readonly ResourceAsset<MapGenerationSettings> asset = new ResourceAsset<MapGenerationSettings>("MapGenerationSettings");

    [SerializeField] private Vector2Int mapResolution = default;
    [SerializeField] private TileTextureData tileTexture = default;
    [Space]
    [SerializeField] private int minCastleDistanceFromBorder = default;
    [SerializeField] private int maxCastleDistanceFromBorder = default;
    [SerializeField] private int minDistanceBetweenCastles = default;
    [SerializeField] private float distanceBetweenCastlesPower = default;
    [SerializeField] private int spawnedVillagesCount = default;
    [SerializeField] private float distanceToVillagePower = default;
    [SerializeField] private int minVillageDistanceFromBorder = default;
    [SerializeField] private int minDistanceBetweenSettlments = default;
    [SerializeField] private TileType[] possibleCastleSpawnTiles = default;
    [SerializeField] private TileType[] possibleVillageSpawnTiles = default;


    public static Vector2Int MapResolution => asset.Instance.mapResolution;

    public static TileTextureData TileTexture => asset.Instance.tileTexture;

    public static int MinCastleDistanceFromBorder => asset.Instance.minCastleDistanceFromBorder;
    public static int MaxCastleDistanceFromBorder => asset.Instance.maxCastleDistanceFromBorder;
    public static int MinDistanceBetweenCastles => asset.Instance.minDistanceBetweenCastles;
    public static float DistanceBetweenCastlesPower => asset.Instance.distanceBetweenCastlesPower;
    public static int SpawnedVillagesCount => asset.Instance.spawnedVillagesCount;
    public static int MinDistanceBetweenSettlments => asset.Instance.minDistanceBetweenSettlments;
    public static int MinVillageDistanceFromBorder => asset.Instance.minVillageDistanceFromBorder;
    public static float DistanceToVillagePower => asset.Instance.distanceToVillagePower;
    public static TileType[] PossibleCastleSpawnTiles => asset.Instance.possibleCastleSpawnTiles;
    public static TileType[] PossibleVillageSpawnTiles => asset.Instance.possibleVillageSpawnTiles;
}
