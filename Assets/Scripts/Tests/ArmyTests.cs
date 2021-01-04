using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Domination.Warfare;


[TestOf(typeof(Army))]
public class ArmyTests
{
    [Test]
    public void ArmyBasicCreationTest()
    {
        var army = new Army();
        Assert.IsTrue(army.IsEmpty);
        Assert.AreEqual(army.TotalUnitsCount, 0);

        var units = army.GetUnits();
        Assert.AreEqual(units.Count, 0);
    }

    [Test]
    public void ArmyUnitsAddingAndClearingTest([Values(0, 1, 10)] int meleeUnitsCount, [Values(0, 1, 10)] int rangedUnitsCount)
    {
        var meleeUnits = new List<Unit>(meleeUnitsCount);

        for (int i = 0; i < meleeUnitsCount; i++)
        {
            meleeUnits.Add(new Unit(null, WeaponType.Melee, Random.Range(0, 100)));
        }        

        var rangedUnits = new List<Unit>(rangedUnitsCount);

        for (int i = 0; i < rangedUnitsCount; i++)
        {
            rangedUnits.Add(new Unit(null, WeaponType.Ranged, Random.Range(0, 100)));
        }

        var army = new Army();
        army.AddUnits(meleeUnits);
        army.AddUnits(rangedUnits);

        Assert.AreEqual(army.TotalUnitsCount, meleeUnitsCount + rangedUnitsCount);
        Assert.AreEqual(army.IsEmpty, (meleeUnitsCount == 0) && (rangedUnitsCount == 0));
        Assert.AreEqual(army.GetUnitsCount(WeaponType.Melee), meleeUnitsCount);
        Assert.AreEqual(army.GetUnitsCount(WeaponType.Ranged), rangedUnitsCount);

        var allUnits = army.GetUnits();
        foreach (var unit in allUnits)
        {
            Assert.IsTrue(meleeUnits.Contains(unit) || rangedUnits.Contains(unit));
        }

        army.Clear();
        Assert.AreEqual(army.IsEmpty, true);
    }
}
