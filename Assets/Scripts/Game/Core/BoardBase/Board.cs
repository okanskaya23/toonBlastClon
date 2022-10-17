using System;
using System.Collections.Generic;
using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Items;
using Game.Managers;
using Game.Mechanics;
using UnityEngine;

namespace Game.Core.BoardBase
{
    public class Board : MonoBehaviour
    {
        public const int Rows = 9;
        public const int Cols = 9;

        public const int MinimumMatchCount = 2;
        public const int MinNoOfMatchesForBomb = 7;
        public const int MinNoOfMatchesForRocket = 5;
        public const int MinNoOfSpecialHintCount = 2;

        public Transform CellsParent;
        public Transform ItemsParent;
        public Transform ParticlesParent;

        [SerializeField] private Cell CellPrefab;

        public readonly Cell[,] Cells = new Cell[Cols, Rows];

        private readonly MatchFinder _matchFinder = new MatchFinder();
        private readonly bool[,] _visitedCells = new bool[Board.Rows, Board.Cols];

        public void Prepare()
        {
            CreateCells();
            PrepareCells();
        }

        private void CreateCells()
        {
            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Cols; x++)
                {
                    var cell = Instantiate(CellPrefab, Vector3.zero, Quaternion.identity, CellsParent);
                    Cells[x, y] = cell;
                }
            }
        }

        private void PrepareCells()
        {
            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Cols; x++)
                {
                    Cells[x, y].Prepare(x, y, this);
                }
            }
        }

        public void CellTapped(Cell cell)
        {
            if (cell == null) return;

            if (!cell.HasItem()) return;

            if (!cell.Item.IsTappable()) return;

            if (cell.Item.GetMatchType() == MatchType.Special)
            {
                TapSpecialItem(cell);
            }
            else { ExplodeMatchingCells(cell); }

            
        }

        private void TapSpecialItem(Cell cell)
        {
            var specialMatches = _matchFinder.FindMatches(cell, MatchType.Special);

            if (specialMatches.Count < MinNoOfSpecialHintCount)
            {
                ((SpecialItem)(specialMatches[0].Item)).Trigger(specialMatches[0]);
            }
            else if (specialMatches.Count >= MinNoOfSpecialHintCount)
            {
                List<int> referenceSpecials = new List<int>();

                List<int> specialRowValues = new();
                foreach(var item in specialMatches)
                {
                    specialRowValues.Add((int)(((SpecialItem)(item.Item)).GetSpecialItemType()));
                }

                specialRowValues.Sort();
                referenceSpecials.Add(specialRowValues[specialRowValues.Count - 1]);
                referenceSpecials.Add(specialRowValues[specialRowValues.Count - 2]);

                foreach (var specialCell in specialMatches)
                {
                    specialCell.Item.TryExecute();
                }
                TriggerSpecialCombo(cell, referenceSpecials);

            }
        }

        private void TriggerSpecialCombo(Cell cell, List<int> specialCell)
        {
            if(specialCell[0] == (int) SpecialItemType.Rocket && specialCell[1] == (int) SpecialItemType.Rocket)
            {
                SpecialItemTrigger.TriggerGenericExplosion(cell, 8, 0);
                SpecialItemTrigger.TriggerGenericExplosion(cell, 0, 8);
            }
            else if(specialCell[0] == (int)SpecialItemType.Bomb && specialCell[1] == (int)SpecialItemType.Rocket)
            {
                SpecialItemTrigger.TriggerGenericExplosion(cell, 1, 8);
                SpecialItemTrigger.TriggerGenericExplosion(cell, 8, 1);
            }
            else if(specialCell[0] == (int)SpecialItemType.Bomb && specialCell[1] == (int)SpecialItemType.Bomb)
            {
                SpecialItemTrigger.TriggerGenericExplosion(cell, 3, 3);
            }


        }

        

        

        private void ExplodeMatchingCells(Cell cell)
        {
            var cells = _matchFinder.FindMatches(cell, cell.Item.GetMatchType());
            if (cells.Count < MinimumMatchCount) return;

            int tempMatchCount = cells.Count;
            MatchType tempMatchType = cell.Item.GetMatchType();
            _matchFinder.FindAndAddAdjacentBreakables(cells);
            for (var i = 0; i < cells.Count; i++)
            {
                var explodedCell = cells[i];
                var item = explodedCell.Item;
                item.TryExecute();
            }
            if (tempMatchType == MatchType.Special)
            {

            }
            else if (tempMatchType != MatchType.None)
            {
                if (tempMatchCount >= 7)
                {
                    CreateSpecialItem(cell, ItemType.Bomb);
                }
                else if (tempMatchCount >= 5)
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        CreateSpecialItem(cell, ItemType.HorizontalRocket);
                    }
                    else
                    {
                        CreateSpecialItem(cell, ItemType.VerticalRocket);
                    }
                }
            }
        }

        public void GiveHintToSpecials()
        {
            ClearVisitedCells();
            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Cols; x++)
                {
                    if (_visitedCells[x, y] || Cells[x, y].Item == null) continue;

                    var cells = _matchFinder.FindMatches(Cells[x, y], Cells[x, y].Item.GetMatchType());
                    foreach (var item in cells)
                    {
                        _visitedCells[item.X, item.Y] = true;
                    }
                    if (Cells[x, y].Item.GetMatchType() == MatchType.Special)
                    {
                        GiveSpecialItemHints(cells);

                    }
                    else if (Cells[x, y].Item.GetMatchType() != MatchType.None)
                    {
                        GiveCubeItemHints(cells);
                    }
                }
            }

        }

        private static void GiveCubeItemHints(List<Cell> cells)
        {
            if (cells.Count >= MinNoOfMatchesForBomb)
            {
                for (var i = 0; i < cells.Count; i++)
                {
                    cells[i].Item.ChangeSprite(2);
                }
            }
            else if (cells.Count >= MinNoOfMatchesForRocket)
            {
                for (var i = 0; i < cells.Count; i++)
                {
                    cells[i].Item.ChangeSprite(1);
                }
            }
            else
            {
                for (var i = 0; i < cells.Count; i++)
                {
                    cells[i].Item.ChangeSprite(0);
                }
            }
        }

        private static void GiveSpecialItemHints(List<Cell> cells)
        {
            if (cells.Count >= MinNoOfSpecialHintCount)
            {
                for (var i = 0; i < cells.Count; i++)
                {
                    var tempParticle = cells[i].Item.GetComponentInChildren<ParticleSystem>();
                    if (tempParticle == null)
                    {
                        ServiceProvider.GetParticleManager.PlayComboParticleOnItem(cells[i].Item);
                    }

                }
            }
            else
            {
                var tempParticle = cells[0].Item.GetComponentInChildren<ParticleSystem>();
                if (tempParticle != null)
                {
                    ServiceProvider.GetParticleManager.StopParticle(tempParticle);
                }
            }
        }

        public void ClearVisitedCells()
        {
            for (var x = 0; x < _visitedCells.GetLength(0); x++)
            {
                for (var y = 0; y < _visitedCells.GetLength(1); y++)
                {
                    _visitedCells[x, y] = false;
                }
            }
        }
        public void CreateSpecialItem(Cell cell, ItemType itemType)
        {
            if (cell.Item == null)
            {
                cell.Item = ServiceProvider.GetItemFactory.CreateItem(itemType, ItemsParent);

                var offsetY = 0.0F;
                var targetCellBelow = cell.GetFallTarget().FirstCellBelow;
                if (targetCellBelow != null)
                {
                    if (targetCellBelow.Item != null)
                    {
                        offsetY = targetCellBelow.Item.transform.position.y + 1;
                    }
                }

                var p = cell.transform.position;

                cell.Item.transform.position = p;
                cell.Item.Fall();
            }
        }
        private void Update()
        {
            GiveHintToSpecials();
        }
        public Cell GetNeighbourWithDirection(Cell cell, Direction direction)
        {
            var x = cell.X;
            var y = cell.Y;
            switch (direction)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    y += 1;
                    break;
                case Direction.UpRight:
                    y += 1;
                    x += 1;
                    break;
                case Direction.Right:
                    x += 1;
                    break;
                case Direction.DownRight:
                    y -= 1;
                    x += 1;
                    break;
                case Direction.Down:
                    y -= 1;
                    break;
                case Direction.DownLeft:
                    y -= 1;
                    x -= 1;
                    break;
                case Direction.Left:
                    x -= 1;
                    break;
                case Direction.UpLeft:
                    y += 1;
                    x -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }

            if (x >= Cols || x < 0 || y >= Rows || y < 0) return null;

            return Cells[x, y];
        }
    }
}
