using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Items;


namespace Game.Mechanics
{
    public static class SpecialItemTrigger
    {

        public static void TriggerGenericExplosion(Cell cell, int horizontalDistance, int verticalDistance)
        {
            var x = cell.X;
            var y = cell.Y;
            for (int i = -horizontalDistance; i <= horizontalDistance; i++)
            {
                for (int j = -verticalDistance; j <= verticalDistance; j++)
                {
                    if (CheckOutOfBorder(x + i, y + j))
                    {
                        var curCell = cell.Board.Cells[x + i, y + j];

                        if (!curCell.HasItem()) continue;

                        if (curCell.Item.GetMatchType() == MatchType.Special)
                        {
                            ((SpecialItem)(curCell.Item)).Trigger(curCell);
                        }
                        else
                        {
                            curCell.Item.TryExecute();
                        }
                    }
                }

            }
        }

        private static bool CheckOutOfBorder(int x, int y)
        {
            return (x < Board.Cols && x >= 0 && y < Board.Rows && y >= 0);
        }



    }
}
