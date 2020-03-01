﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;


    public void SetType(TileType tileType)
    {
        spriteRenderer.color = TilesContainer.GetTileColor(tileType);
    }
}
