using Domination.Warfare;
using NUnit.Framework;


public class UnitStateRestore
{
    [Test]
    public void UnitStateGetRestore([Values] WeaponType weaponType)
    {
        var unit = TestsUtils.CreateRandomUnit(weaponType);
        var unitData = unit.GetData();
        var newUnit = new Unit(unitData);

        Assert.AreEqual(unit.WeaponLevel, newUnit.WeaponLevel);
        Assert.AreEqual(unit.WeaponType, newUnit.WeaponType);
        Assert.AreEqual(unit.Health, newUnit.Health);
    }
}
