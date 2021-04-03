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


    public static Vector2Int MapResolution => asset.Value.mapResolution;

    public static TileTextureData TileTexture => asset.Value.tileTexture;

    public static int MinCastleDistanceFromBorder => asset.Value.minCastleDistanceFromBorder;
    public static int MaxCastleDistanceFromBorder => asset.Value.maxCastleDistanceFromBorder;
    public static int MinDistanceBetweenCastles => asset.Value.minDistanceBetweenCastles;
    public static float DistanceBetweenCastlesPower => asset.Value.distanceBetweenCastlesPower;
    public static int SpawnedVillagesCount => asset.Value.spawnedVillagesCount;
    public static int MinDistanceBetweenSettlments => asset.Value.minDistanceBetweenSettlments;
    public static int MinVillageDistanceFromBorder => asset.Value.minVillageDistanceFromBorder;
    public static float DistanceToVillagePower => asset.Value.distanceToVillagePower;
    public static TileType[] PossibleCastleSpawnTiles => asset.Value.possibleCastleSpawnTiles;
    public static TileType[] PossibleVillageSpawnTiles => asset.Value.possibleVillageSpawnTiles;
}
