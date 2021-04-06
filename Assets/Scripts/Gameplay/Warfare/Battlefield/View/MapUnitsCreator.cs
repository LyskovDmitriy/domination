using Domination.Battle.Logic;
using Domination.Warfare;
using UnityEngine;


namespace Domination.Battle.View
{
    [CreateAssetMenu]
    public class MapUnitsCreator : ScriptableObject
    {
        private static readonly ResourceAsset<MapUnitsCreator> asset = new ResourceAsset<MapUnitsCreator>("MapUnitsCreator");

        [SerializeField] private StructureView wallTilePrefab = default;
        [SerializeField] private StructureView gateTilePrefab = default;
        [SerializeField] private WarriorView knightViewPrefab = default;
        [SerializeField] private WarriorView archerViewPrefab = default;


        public static StructureView CreateStructure(Structure structure, Vector2 position, Transform parent)
        {
            var structureView = CreateMapUnit(structure.isGate ? asset.Value.gateTilePrefab : asset.Value.wallTilePrefab, position, parent);
            structureView.Init(structure);
            return structureView;
        }

        public static WarriorView CreateWarrior(Warrior warrior, Vector2 position, Transform parent)
        {
            var warriorView = CreateMapUnit(
                (warrior.Unit.WeaponType == WeaponType.Melee) ? asset.Value.knightViewPrefab : asset.Value.archerViewPrefab,
                position, parent);
            warriorView.Init(warrior, !warrior.IsAttacker);
            return warriorView;
        }


        private static T CreateMapUnit<T>(T prefab, Vector2 position, Transform parent) where T : Object =>
            Instantiate(prefab, position, Quaternion.identity, parent);
    }
}
