using System;
using UnityEngine;


[CreateAssetMenu]
public class BuildingsSettingsContainer : ScriptableObject
{
    private static readonly ResourceAsset<BuildingsSettingsContainer> asset = new ResourceAsset<BuildingsSettingsContainer>("BuildingsSettingsContainer");

    [SerializeField] private BuildingSettingsBase[] buildings = default;


    public static BuildingSettingsBase GetBuildingSettings(BuildingType type) => 
        Array.Find(asset.Value.buildings, (building) => building.Type == type);    
    
    public static T GetBuildingSettings<T>() where T : BuildingSettingsBase => 
        Array.Find(asset.Value.buildings, (building) => building is T) as T;
}
