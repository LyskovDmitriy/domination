using UnityEngine;
using System.Collections.Generic;


namespace Domination
{
    public class FogSystem : MonoBehaviour
    {
        [SerializeField] private float initialVisibilityRange = default;
        [SerializeField] private float increasedVisibilityRangePerTurn = default; //TODO: make dependent on scouts tower

        private Tile[,] tiles;
        private Character[] characters;
        private Dictionary<Character, HashSet<Tile>> characterToAdditionalVisibleTiles = new Dictionary<Character, HashSet<Tile>>();


        public void Init(Tile[,] tiles, Character[] characters)
        {
            this.tiles = tiles;
            this.characters = characters;
        }

        
        public void ApplyFog(int turnIndex, Character character)
        {
            Vector2Int castlePosition = character.Castle.Tile.Position;
            float visibilityRange = initialVisibilityRange + increasedVisibilityRangePerTurn * turnIndex;

            foreach (var tile in tiles)
            {
                tile.SetFogActive(visibilityRange < (tile.Position - castlePosition).magnitude);
            }
        }
    }
}
