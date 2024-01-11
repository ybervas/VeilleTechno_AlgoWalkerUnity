using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Dungeon
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        private Tilemap tilemap;
        [SerializeField]
        private List<Tile> tiles;
        [SerializeField]
        private Vector2 gridSize;
        private List<List<CellType>> grid;
        private List<Walker> walkers = new List<Walker>();
        private readonly int MIN_DUNGEON_DEPTH = 6;
        private List<CellDistance> dijkstraMap;
        [SerializeField]
        private Canvas dijkstraCanvas;
        private Vector2 entryCell;
        private Vector2 exitCell;


        void Start()
        {
            InitGrid();
            UpdateGridDisplay();
        }

        private void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                generateDungeon();
            }
        }

        private void generateDungeon()
        {
            Debug.Log("Génération commencée");


            do
            {
                InitGrid();
                dijkstraMap = new List<CellDistance>();
                entryCell = new Vector2(UnityEngine.Random.Range(0, gridSize.x - 1), UnityEngine.Random.Range(0, gridSize.y - 1));
                PlaceWalker(entryCell, 5, 3);

                while (walkers.Count != 0)
                {
                    List<Walker> walkersToRemove = new List<Walker>();
                    List<Walker> walkersToAdd = new List<Walker>();


                    foreach (Walker walker in walkers)
                    {
                        walker.Step(GetAccessibleCells(walker.Cell));

                        if (walker.Moved)
                        {
                            SetCell(walker.Cell, CellType.EMPTY);
                            walker.Moved = false;
                        }

                        if (walker.Arrived)
                        {
                            walkersToRemove.Add(walker);
                        }

                        if (walker.SubWalkersSteps.Contains(walker.NbSteps))
                        {
                            walkersToAdd.Add(walker);
                        }
                    }

                    foreach (Walker walker in walkersToRemove)
                    {
                        walkers.Remove(walker);
                    }

                    foreach (Walker walker in walkersToAdd)
                    {
                        PlaceWalker(walker.Cell, 5, 0);
                    }
                }

                UpdateGridDisplay();
                ComputeCellDistances(entryCell);
            }
            while (GetDungeonDepth() < MIN_DUNGEON_DEPTH);

            List<Vector2> furtherestCells = GetFurtherestCells();
            exitCell = furtherestCells[UnityEngine.Random.Range(0, furtherestCells.Count - 1)];
            PlaceEntryAndExitCells();
            DisplayDijkstraMap();

            Debug.Log("Génération terminée");
        }

        private void InitGrid()
        {
            grid = new List<List<CellType>>();

            for (int i = 0; i < gridSize.x; i++)
            {
                grid.Add(new List<CellType>());
                for (int j = 0; j < gridSize.y; j++)
                {
                    grid[i].Add(CellType.WALL);
                }
            }
        }

        private void UpdateGridDisplay()
        {
            CellType cellType;

            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    cellType = grid[i][j];

                    Vector3Int currentCell = tilemap.WorldToCell(new Vector2(i, j));

                    tilemap.SetTile(currentCell, tiles[(int)cellType]);
                }
            }
        }

        private void PlaceWalker(Vector2 cell, int nbSteps, int nbStepsBetweenSubWalker)
        {
            Walker walker = new Walker(cell, nbSteps, nbStepsBetweenSubWalker);
            walkers.Add(walker);
        }

        private List<Vector2> GetAccessibleCells(Vector2 cell)
        {
            List<Vector2> adjacents = Utils.GetAdjacentsCells(cell);
            List<Vector2> accessibles = new List<Vector2>();

            foreach (Vector2 adjacent in adjacents)
            {
                if (IsInsideGrid(adjacent) && grid[(int)adjacent.x][(int)adjacent.y] != CellType.EMPTY)
                {
                    accessibles.Add(adjacent);
                }
            }

            return accessibles;
        }

        private bool IsInsideGrid(Vector2 cell)
        {
            return cell.x >= 0 && cell.x < gridSize.x && cell.y >= 0 && cell.y < gridSize.y;
        }

        public void SetCell(Vector2 cell, CellType cellType)
        {
            grid[(int)cell.x][(int)cell.y] = cellType;
        }

        private void ComputeCellDistances(Vector2 cell, int dist = 0)
        {
            if (dijkstraMap.Count <= 0)
            {
                dijkstraMap.Add(new CellDistance(cell, dist));
            }

            dist++;

            List<Vector2> adjacents = Utils.GetAdjacentsCells(cell);

            foreach (Vector2 adjacent in adjacents)
            {
                Vector3Int adjacentCell = tilemap.WorldToCell(new Vector2(adjacent.x, adjacent.y));
                TileBase tileId = tilemap.GetTile(adjacentCell);

                if (tileId != null || !IsInsideGrid(adjacent))
                {
                    continue;
                }

                CellDistance cellDist = GetDijkstraCellDist(adjacent);

                if (cellDist == null)
                {
                    dijkstraMap.Add(new CellDistance(adjacent, dist));
                    ComputeCellDistances(adjacent, dist);
                }
                else if (cellDist.Dist >= dist)
                {
                    cellDist.Dist = dist;
                    ComputeCellDistances(adjacent, dist);
                }
            }
        }

        private CellDistance GetDijkstraCellDist(Vector2 cell)
        {
            foreach (CellDistance cellDist in dijkstraMap)
            {
                if (cellDist.Cell == cell)
                {
                    return cellDist;
                }
            }
            return null;
        }

        private void DisplayDijkstraMap()
        {
            int textCount = 0;

            for (int i = dijkstraCanvas.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(dijkstraCanvas.transform.GetChild(i).gameObject);
            }

            foreach (CellDistance cellDist in dijkstraMap)
            {
                float camSize = Camera.main.orthographicSize;

                GameObject texteObject = new GameObject(textCount.ToSafeString());
                TextMeshPro text = texteObject.AddComponent<TextMeshPro>();

                text.transform.SetParent(dijkstraCanvas.transform);

                text.text = cellDist.Dist.ToString();
                text.fontSize = 8;
                text.alignment = TextAlignmentOptions.CenterGeoAligned;
                text.GetComponent<MeshRenderer>().sortingOrder = 1;
                text.rectTransform.sizeDelta = new Vector2(1, 1);
                text.transform.position = tilemap.WorldToCell(new Vector2(cellDist.Cell.x, cellDist.Cell.y));
                text.transform.position += text.rectTransform.localScale / 2;

                textCount++;
            }
        }

        private void PlaceEntryAndExitCells()
        {
            grid[(int)entryCell.x][(int)entryCell.y] = CellType.ENTREE;
            Vector3Int entryCellV3 = new Vector3Int((int)entryCell.x, (int)entryCell.y);
            tilemap.SetTile(entryCellV3, tiles[(int)CellType.ENTREE]);

            grid[(int)exitCell.x][(int)exitCell.y] = CellType.SORTIE;
            Vector3Int exitCellV3 = new Vector3Int((int)exitCell.x, (int)exitCell.y);
            tilemap.SetTile(exitCellV3, tiles[(int)CellType.SORTIE]);
        }

        private List<Vector2> GetFurtherestCells()
        {
            int dungeonDepth = GetDungeonDepth();
            List<Vector2> furtherestCells = new List<Vector2>();

            foreach (CellDistance cellDist in dijkstraMap)
            {
                if (cellDist.Dist >= dungeonDepth)
                {
                    furtherestCells.Add(cellDist.Cell);
                }
            }

            return furtherestCells;
        }

        private int GetDungeonDepth()
        {
            int maxDepth = 0;

            foreach (CellDistance cellDist in dijkstraMap)
            {
                if (cellDist.Dist > maxDepth)
                {
                    maxDepth = cellDist.Dist;
                }
            }
            return maxDepth;
        }

    }
}

