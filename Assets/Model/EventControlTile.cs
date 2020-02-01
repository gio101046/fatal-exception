﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Model
{
    public class EventControlTile
    {
        public KeyCode keyCode;
        public Tile tile;
        public Tile successTile;
        public Vector3Int position;

        public EventControlTile(KeyCode keyCode, Tile tile, Tile successTile, Vector3Int position)
        {
            this.keyCode = keyCode;
            this.tile = tile;
            this.successTile = successTile;
            this.position = position;
        }
    }
}
