
using Game.Core.ItemBase;
using Game.Managers;

namespace Game.Items
{
    public class CrateItem : Item
    {
        int life = 2;

        public void PrepareCrateItem(ItemBase itemBase)
        {
            var crateLayer2Sprite = ServiceProvider.GetImageLibrary.CrateLayer2Sprite;
            var crateLayer1Sprite = ServiceProvider.GetImageLibrary.CrateLayer1Sprite;
            
            Prepare(itemBase, crateLayer1Sprite);
            Prepare(itemBase, crateLayer2Sprite);
        }

        public override bool IsFallable()
        {
            return false;
        }

        public override bool IsAdjacentBreakable()
        {
            return true;
        }

        public override void TryExecute()
        {


            life--;
            if(life > 0)
            {
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            }
            else
            {
                base.TryExecute();
            }
        }

        public override bool IsTappable()
        {
            return false;
        }

        public override bool IsMatchable()
        {
            return false;
        }

    }
}
