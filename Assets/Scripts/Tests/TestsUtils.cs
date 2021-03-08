using Domination.Warfare;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;


public static class TestsUtils
{
    public static Unit CreateRandomUnit()
    {
        var weaponTypes = Enum.GetValues(typeof(WeaponType));
        return CreateRandomUnit((WeaponType)weaponTypes.GetValue(Random.Range(0, weaponTypes.Length)));
    }

    public static Unit CreateRandomUnit(WeaponType weaponType) =>
        new Unit(weaponLevel: Random.Range(0, 3), weaponType, health: Random.Range(1, 100));
}
