using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using Game.Mechanics;

namespace Game.Items
{

    public class RocketItem : SpecialItem
    {
        public override SpecialItemType GetSpecialItemType()
        {
            return SpecialItemType.Rocket;
        }
    }


    public class VerticalRocketItem : RocketItem
    {
        public void PrepareVerticalRocketItem(ItemBase itemBase)
        {
            var rocketSpriteVertical = ServiceProvider.GetImageLibrary.RocketVertical;

            Prepare(itemBase, rocketSpriteVertical);
        }

        public override void Trigger(Cell cell)
        {
            base.Trigger(cell);
            cell.Item.TryExecute();
            SpecialItemTrigger.TriggerGenericExplosion(cell, 0, 8);

        }

        
    }
    public class HorizontalRocketItem : RocketItem
    {
        public void PrepareHorizontalRocketItem(ItemBase itemBase)
        {
            var rocketSpriteHorizontal = ServiceProvider.GetImageLibrary.RocketHorizontal;
            Prepare(itemBase, rocketSpriteHorizontal);
        }

        public override void Trigger(Cell cell)
        {
            base.Trigger(cell);
            cell.Item.TryExecute();
            SpecialItemTrigger.TriggerGenericExplosion(cell, 8, 0);

        }

        
    }
}
