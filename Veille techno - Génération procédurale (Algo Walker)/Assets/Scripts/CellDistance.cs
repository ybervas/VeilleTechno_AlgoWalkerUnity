using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class CellDistance
    {
        private Vector2 cell;
        public Vector2 Cell
        {
            get { return cell; }
        }
        private int dist = -1;
        public int Dist
        {
            get { return dist; }
            set
            {
                dist = value;
            }
        }

        public CellDistance(Vector2 _cell, int _dist)
        {
            cell = _cell;
            dist = _dist;
        }
    }
}
