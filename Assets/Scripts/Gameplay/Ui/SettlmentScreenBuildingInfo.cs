using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SettlmentScreenBuildingInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel = default;


    public void SetInfo(Settlment.Building buildingInfo)
    {
        if (buildingInfo == null)
        {
            nameLabel.text = "EMPTY";
        }
        else
        {
            nameLabel.text = $"{buildingInfo.type.ToString()} {buildingInfo.level}";
        }
    }
}
