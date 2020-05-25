using UnityEngine;


namespace Domination
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer terrainSpriteRenderer = default;
        [SerializeField] private GameObject border = default;
        [SerializeField] private GameObject fogSprite = default;

        
        public Vector2Int Position { get; private set; }
        public Settlment Settlment { get; private set; }
        public bool IsInFog { get; private set; }


        private void Awake()
        {
            border.SetActive(false);
        }


        public void Init(Vector2Int position, TileType tileType)
        {
            Position = position;
            terrainSpriteRenderer.color = TilesContainer.GetTileColor(tileType);
        }


        public void AttachSettlment(Settlment attachedSettlment)
        {
            Settlment = attachedSettlment;
            Settlment.Tile = this;
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
