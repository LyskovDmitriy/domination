using UnityEngine;


namespace Domination
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = default;
        [SerializeField] private GameObject border = default;


        public Settlment Settlment { get; private set; }


        private void Awake()
        {
            border.SetActive(false);
        }


        public void SetType(TileType tileType)
        {
            spriteRenderer.color = TilesContainer.GetTileColor(tileType);
        }


        public void AttachSettlment(Settlment attachedSettlment)
        {
            Settlment = attachedSettlment;
        }


        public void SetSelection(bool isSelected)
        {
            border.SetActive(isSelected);
        }
    }
}
