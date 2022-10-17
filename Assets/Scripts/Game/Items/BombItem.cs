
using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using Game.Mechanics;

namespace Game.Items
{
    public class BombItem : SpecialItem
    {
        public void PrepareBombItem(ItemBase itemBase)
        {
            var bombSprite = ServiceProvider.GetImageLibrary.BombSprite;

            Prepare(itemBase, bombSprite);
        }

        public override void Trigger(Cell cell)
        {
            base.Trigger(cell);
            cell.Item.TryExecute();
            SpecialItemTrigger.TriggerGenericExplosion(cell, 1, 1);
        }

        public override SpecialItemType GetSpecialItemType()
        {
            return SpecialItemType.Bomb;
        }


    }

}
