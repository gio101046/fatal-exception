using System;
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
        public Tile pressedTile;
        public Tile successTile;
        public Tile failTile;
        public Vector3Int position;
        public bool isPlusTile;
        public bool isPerformed;
        public bool isPressed;

        public EventControlTile(KeyCode keyCode, Tile tile, Tile pressedTile, Tile successTile, Tile failTile, Vector3Int position, bool isPlusTile = false)
        {
            this.keyCode = keyCode;
            this.tile = tile;
            this.pressedTile = pressedTile;
            this.successTile = successTile;
            this.failTile = failTile;
            this.position = position;
            this.isPlusTile = isPlusTile;
        }
    }
}
