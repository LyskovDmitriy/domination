using Domination.LevelLogic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class SettlmentStateRestore
{
    private static readonly Vector2Int[] POSITIONS = new[] { Vector2Int.zero, Vector2Int.one };


    [Test]
    public void VillageStateRestore([ValueSource(nameof(POSITIONS))] Vector2Int postion,
        [Values] BuildingType builtBuilding,
        [Random(0, 5, 2)] int builtBuildingLevel)
    {
        LogAssert.ignoreFailingMessages = true;

        var oldVillage = new Village(postion);
        oldVillage.Build(builtBuilding, builtBuildingLevel);

        var data = oldVillage.GetData();
        Assert.AreEqual(data.type, SettlmentType.Village);

        var newVillage = new Village(data);

        ValidateSettlmentRestore(oldVillage, newVillage);
    }

    [Test]
    public void CastleStateRestore([ValueSource(nameof(POSITIONS))] Vector2Int postion)
    {
        var oldCastle = new Castle(postion);

        var data = oldCastle.GetData();
        Assert.AreEqual(data.type, SettlmentType.Castle);

        var newCastle = new Castle(data);

        ValidateSettlmentRestore(oldCastle, newCastle);
    }

    private void ValidateSettlmentRestore(Settlment oldSettlment, Settlment newSettlment)
    {
        Assert.AreEqual(oldSettlment.Id, newSettlment.Id);
        Assert.AreEqual(oldSettlment.Position, newSettlment.Position);

        var initialVillageBuildings = oldSettlment.GetBuildings();
        var newVillageBuildings = newSettlment.GetBuildings();

        Assert.AreEqual(initialVillageBuildings, newVillageBuildings);
    }
}
