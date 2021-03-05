using Domination.Warfare;
using System;
using UnityEngine;


namespace Domination.Data
{
    [Serializable]
    public class UnitData : MonoBehaviour
    {
        public WeaponType weaponType;
        public int weaponLevel;
        public int health;
    }
}
