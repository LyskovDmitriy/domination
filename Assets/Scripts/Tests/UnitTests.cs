using NUnit.Framework;
using Domination.Warfare;


[TestOf(typeof(Unit))]
public class UnitTests
{
    [Test]
    public void UnitConstructorWithSetup(
        [Values] WeaponType weaponType, 
        [Values(1, 2, 3)] int weaponLevel, 
        [Values(1, 10, 100)] int health)
    {
        Unit unit = new Unit(weaponLevel, weaponType, health);

        Assert.AreEqual(unit.WeaponLevel, weaponLevel);
        Assert.AreEqual(unit.WeaponType, weaponType);
        Assert.AreEqual(unit.Health, health);
    }

    [Test]
    public void UnitCloneConstructor(
        [Values] WeaponType weaponType, 
        [Values(1, 2, 3)] int weaponLevel, 
        [Values(1, 10, 100)] int health)
    {
        Unit unit = new Unit(weaponLevel, weaponType, health);
        Unit copy = new Unit(unit);

        Assert.AreEqual(copy.WeaponLevel, unit.WeaponLevel);
        Assert.AreEqual(copy.WeaponType, unit.WeaponType);
        Assert.AreEqual(copy.Health, unit.Health);
    }
}
