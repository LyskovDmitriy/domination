using NUnit.Framework;
using UnityEngine;
using Domination.Warfare;


[TestOf(typeof(Unit))]
public class UnitTests
{
    [Test]
    public void UnitConstructorWithSetup([Values] WeaponType weaponType, [Values(1, 10, 100)] int health)
    {
        var weapon = ScriptableObject.CreateInstance<Weapon>();
        Unit unit = new Unit(weapon, weaponType, health);

        Assert.AreEqual(unit.Weapon, weapon);
        Assert.AreEqual(unit.WeaponType, weaponType);
        Assert.AreEqual(unit.Health, health);
    }

    [Test]
    public void UnitCloneConstructor([Values] WeaponType weaponType, [Values(1, 10, 100)] int health)
    {
        var weapon = ScriptableObject.CreateInstance<Weapon>();
        Unit unit = new Unit(weapon, weaponType, health);
        Unit copy = new Unit(unit);

        Assert.AreEqual(copy.Weapon, unit.Weapon);
        Assert.AreEqual(copy.WeaponType, unit.WeaponType);
        Assert.AreEqual(copy.Health, unit.Health);
    }
}
