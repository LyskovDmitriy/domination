using System.Collections.Generic;
using System;
using UnityEngine;


[CreateAssetMenu]
public class BuildingsSettingsContainer : ScriptableObject
{
    private static readonly ResourceAsset<BuildingsSettingsContainer> asset = new ResourceAsset<BuildingsSettingsContainer>("BuildingsSettingsContainer");

    [SerializeField] private BuildingSettingsBase[] buildings = default;


    public static BuildingSettingsBase GetBuildingSettings(BuildingType type) => Array.Find(asset.Instance.buildings, (building) => building.Type == type);
}
