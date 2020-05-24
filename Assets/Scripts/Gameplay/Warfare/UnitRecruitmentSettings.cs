using System;
using UnityEngine;


namespace Domination.Warfare
{
    [Serializable]
    public class WeaponInfo
    {
        [SerializeField] private Weapon weapon = default;
        [SerializeField] private int upgradePrice = default;

        public Weapon Weapon => weapon;
        public int UpgradePrice => upgradePrice;
    }


    [CreateAssetMenu]
    public class UnitRecruitmentSettings : ScriptableObject
    {
        private static readonly ResourceAsset<UnitRecruitmentSettings> asset = new ResourceAsset<UnitRecruitmentSettings>("UnitRecruitmentSettings");

        [SerializeField] private WeaponInfo[] meleeWeapons = default;
        [SerializeField] private WeaponInfo[] rangedWeapons = default;
        [SerializeField] private int meleeUnitHealth = default;
        [SerializeField] private int rangedUnitHealth = default;
        [SerializeField] private int unitPrice = default;


        public static WeaponInfo[] MeleeWeapons => asset.Instance.meleeWeapons;
        public static WeaponInfo[] RangedWeapons => asset.Instance.rangedWeapons;

        public static int UnitPrice => asset.Instance.unitPrice;


        public static WeaponInfo[] GetWeapons(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Melee:
                    return asset.Instance.meleeWeapons;
                case WeaponType.Ranged:
                    return asset.Instance.rangedWeapons;
            }

            return null;
        }

        public static int GetHealth(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Melee:
                    return asset.Instance.meleeUnitHealth;
                case WeaponType.Ranged:
                    return asset.Instance.rangedUnitHealth;
            }

            return 0;
        }
    }
}
