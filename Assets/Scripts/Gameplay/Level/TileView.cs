using Domination.LevelLogic;
using UnityEngine;


namespace Domination.LevelView
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer terrainSpriteRenderer = default;
        [SerializeField] private GameObject border = default;
        [SerializeField] private GameObject fogSprite = default;

        private Tile tile;

        
        public Vector2Int Position { get; private set; }
        public SettlmentView Settlment { get; private set; }
        public bool IsInFog { get; private set; }


        private void Awake()
        {
            border.SetActive(false);
        }


        public void Init(Vector2Int position, Tile tile)
        {
            this.tile = tile;
            Position = position;
            terrainSpriteRenderer.color = TilesContainer.GetTileColor(tile.Type);
        }


        public void AttachSettlment(SettlmentView attachedSettlment)
        {
            Settlment = attachedSettlment;
        }


        public void SetSelection(bool isSelected)
        {
            border.SetActive(isSelected);
        }

        public void SetFogActive(bool isFogActive)
        {
            IsInFog = isFogActive;
            fogSprite.SetActive(isFogActive);
            terrainSpriteRenderer.gameObject.SetActive(!isFogActive);
            Settlment?.gameObject.SetActive(!isFogActive);
        }
    }
}
