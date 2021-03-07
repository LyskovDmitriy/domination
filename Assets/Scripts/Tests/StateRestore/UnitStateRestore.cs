using Domination.Warfare;
using NUnit.Framework;


public class UnitStateRestore
{
    [Test]
    public void UnitStateGetRestore([Values] WeaponType weaponType, [Values(1, 2, 3)] int weaponLevel, [Values(1, 10, 100)] int health)
    {
        var unit = new Unit(weaponLevel, weaponType, health);
        var unitData = unit.GetData();
        var newUnit = new Unit(unitData);

        Assert.AreEqual(unit.WeaponLevel, newUnit.WeaponLevel);
        Assert.AreEqual(unit.WeaponType, newUnit.WeaponType);
        Assert.AreEqual(unit.Health, newUnit.Health);
    }
}
