using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettlmentViewScreen : UiUnit<object>
{
    public static readonly ResourceBehavior<SettlmentViewScreen> Prefab = new ResourceBehavior<SettlmentViewScreen>("Ui/SettlmentViewScreen");

    [SerializeField] private SettlmentScreenBuildingInfo buildingInfoPrefab = default;
    [SerializeField] private RectTransform buildingsInfoRoot = default;

    private List<SettlmentScreenBuildingInfo> buildingsInfo = new List<SettlmentScreenBuildingInfo>();


    public void Show(Settlment settlment, Action<object> onHidden = null)
    {
        Show(onHidden);

        buildingsInfo.ForEach((building) => building.gameObject.SetActive(false));

        List<Settlment.Building> buildings = settlment.Buildings;

        for (int i = 0; i < settlment.MaxBuildingsCount; i++)
        {
            SettlmentScreenBuildingInfo buildingInfo = null;

            if (i < buildingsInfo.Count)
            {
                buildingInfo = buildingsInfo[i];
            }
            else
            {
                buildingInfo = Instantiate(buildingInfoPrefab, buildingsInfoRoot);
                buildingsInfo.Add(buildingInfo);
            }

            buildingInfo.SetInfo((i < buildings.Count) ? buildings[i] : null);
            buildingInfo.gameObject.SetActive(true);
        }
    }
}
