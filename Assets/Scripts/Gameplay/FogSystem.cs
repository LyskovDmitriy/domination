using UnityEngine;
using Domination.LevelView;


namespace Domination
{
    public class FogSystem : MonoBehaviour
    {
        [SerializeField] private float initialVisibilityRange = default;
        [SerializeField] private float increasedVisibilityRangePerTurn = default; //TODO: make dependent on scouts tower

        private TileView[,] tiles;


        public void Init(TileView[,] tiles)
        {
            this.tiles = tiles;
        }

        
        public void ApplyFog(int turnIndex, Character character)
        {
            Vector2Int castlePosition = character.Castle.Position;
            float visibilityRange = initialVisibilityRange + increasedVisibilityRangePerTurn * turnIndex;

            foreach (var tile in tiles)
            {
                tile.SetFogActive(visibilityRange < (tile.Position - castlePosition).magnitude);
            }
        }
    }
}
