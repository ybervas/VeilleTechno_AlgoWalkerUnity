using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Dungeon
{
    public class Walker
    {
        Vector2 cell;

        public Vector2 Cell
        {
            get { return cell; }
            set
            {
                if (value != cell)
                {
                    cell = value;
                    moved = true;
                }
            }
        }
        private int MAX_STEPS;
        private int nbSteps;
        public int NbSteps
        {
            get { return nbSteps; }
        }
        private int nbSubWalkers;
        public int NbSubWalkers
        {
            get { return nbSubWalkers; }
        }
        private List<int> subWalkersSteps = new List<int>();
        public List<int> SubWalkersSteps
        {
            get { return subWalkersSteps; }
        }
        private bool moved = false;
        public bool Moved
        {
            get { return moved; }
            set
            {
                moved = value;
            }
        }
        private bool arrived = false;

        public bool Arrived
        {
            get { return arrived; }
            set
            {
                arrived = value;
            }
        }

        public Walker(Vector2 _cell, int _maxSteps, int _nbSubWalkers)
        {
            cell = _cell;
            MAX_STEPS = _maxSteps;
            nbSubWalkers = _nbSubWalkers;
            chooseSubWalkersSteps();
        }

        public void Step(List<Vector2> accessiblesCells)
        {
            if (accessiblesCells.Count <= 0)
            {
                Arrived = true;
                return;
            }
            nbSteps += 1;
            Vector2 newCell = ChooseCell(accessiblesCells);

            Cell = newCell;

            Moved = true;

            if (nbSteps >= MAX_STEPS)
            {
                Arrived = true;
            }
        }

        private Vector2 ChooseCell(List<Vector2> accessiblesCells)
        {
            int rdId = Random.Range(0, accessiblesCells.Count - 1);

            return accessiblesCells[rdId];
        }

        private void chooseSubWalkersSteps()
        {
            List<int> stepsArray = Utils.CreateListFromInt(MAX_STEPS);
            Utils.ShuffleList(stepsArray);

            for (int i = 0; i < NbSubWalkers; i++)
            {
                if (stepsArray.Count <= 0)
                {
                    break;
                }

                subWalkersSteps.Add(stepsArray[0]);
                stepsArray.Remove(0);
            }
        }
    }
}

