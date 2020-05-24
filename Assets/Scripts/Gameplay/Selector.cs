using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Domination
{
    public class Selector : MonoBehaviour
    {
        public event Action<Tile> OnTileSelected;
        public event Action OnTileDeselected;


        private new Camera camera;
        private Vector3? touchStartPosition;

        private Tile selectedTile;

        public void Init(Camera camera)
        {
            this.camera = camera;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    touchStartPosition = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (touchStartPosition.HasValue)
                {
                    if (touchStartPosition == Input.mousePosition)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(touchStartPosition.Value), Vector3.forward);

                        Tile tile = hit.transform?.GetComponentInParent<Tile>();

                        if (tile != null)
                        {
                            DeselectTile();
                            selectedTile = tile;
                            selectedTile.SetSelection(true);
                            OnTileSelected?.Invoke(selectedTile);
                        }
                    }
                }

                touchStartPosition = null;
            }
        }


        private void DeselectTile()
        {
            if (selectedTile != null)
            {
                selectedTile.SetSelection(false);
                selectedTile = null;
                touchStartPosition = -Vector3.one;
                OnTileDeselected?.Invoke();
            }
        }
    }
}
