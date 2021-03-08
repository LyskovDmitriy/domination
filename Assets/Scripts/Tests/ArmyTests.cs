using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Domination.Warfare;


[TestOf(typeof(Army))]
public class ArmyTests
{
    [Test]
    public void ArmyBasicCreation()
    {
        var army = new Army();
        Assert.IsTrue(army.IsEmpty);
        Assert.AreEqual(army.TotalUnitsCount, 0);

        var units = army.GetUnits();
        Assert.AreEqual(units.Count, 0);
    }

    [Test]
    public void ArmyUnitsAddingAndClearing(
        [Values(0, 1, 10)] int meleeUnitsCount, 
        [Values(0, 1, 10)] int rangedUnitsCount)
    {
        var meleeUnits = new List<Unit>(meleeUnitsCount);

        for (int i = 0; i < meleeUnitsCount; i++)
        {
            meleeUnits.Add(TestsUtils.CreateRandomUnit(WeaponType.Melee));
        }        

        var rangedUnits = new List<Unit>(rangedUnitsCount);

        for (int i = 0; i < rangedUnitsCount; i++)
        {
            rangedUnits.Add(TestsUtils.CreateRandomUnit(WeaponType.Ranged));
        }

        var allUnits = meleeUnits.Concat(rangedUnits).ToArray();

        var army = new Army();
        army.AddUnits(meleeUnits);
        army.AddUnits(rangedUnits);

        Assert.AreEqual(army.TotalUnitsCount, meleeUnitsCount + rangedUnitsCount);
        Assert.AreEqual(army.IsEmpty, (meleeUnitsCount == 0) && (rangedUnitsCount == 0));
        Assert.AreEqual(army.GetUnitsCount(WeaponType.Melee), meleeUnitsCount);
        Assert.AreEqual(army.GetUnitsCount(WeaponType.Ranged), rangedUnitsCount);
        Assert.AreEqual(army.GetUnits(), allUnits);

        army.Clear();
        Assert.AreEqual(army.IsEmpty, true);
    }
}
