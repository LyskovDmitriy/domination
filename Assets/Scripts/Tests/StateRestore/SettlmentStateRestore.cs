using Domination;
using Domination.LevelLogic;
using NUnit.Framework;
using UnityEngine;


public class SettlmentStateRestore
{
    private static readonly Vector2Int[] POSITIONS = new[] { Vector2Int.zero, Vector2Int.one };


    [Test]
    public void VillageStateRestore([ValueSource(nameof(POSITIONS))] Vector2Int postion,
        [Values] BuildingType builtBuilding,
        [Random(0, 5, 2)] int builtBuildingLevel)
    {
        var character = new AiCharacter();

        var village = new Village(postion);
        village.Build(builtBuilding, builtBuildingLevel);
        village.Lord = character;

        var data = village.GetData();
        Assert.AreEqual(data.type, SettlmentType.Village);

        var newVillage = new Village(data);

        Assert.AreEqual(village.Id, newVillage.Id);
        Assert.AreEqual(village.Position, newVillage.Position);

        var initialVillageBuildings = village.GetBuildings();
        var newVillageBuildings = newVillage.GetBuildings();
        Assert.AreEqual(initialVillageBuildings.Length, newVillageBuildings.Length);

        for (int i = 0; i < initialVillageBuildings.Length; i++)
        {
            initialVillageBuildings[i].Equals(newVillageBuildings[i]);
        }
    }
}
