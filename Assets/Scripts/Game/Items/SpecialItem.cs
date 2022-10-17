
using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;


namespace Game.Items
{
    public class SpecialItem : Item
    {
        public override MatchType GetMatchType()
        {
            return MatchType.Special;
        }

        public virtual void Trigger(Cell cell)
        {

        }

        public virtual SpecialItemType GetSpecialItemType()
        {
            return SpecialItemType.None;
        }
    }
}