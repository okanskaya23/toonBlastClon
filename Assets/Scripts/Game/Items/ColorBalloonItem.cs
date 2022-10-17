using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class ColorBalloonItem : Item
    {
        private MatchType _matchType;

        public void PrepareColorBalloonItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            Prepare(itemBase, GetSpritesForMatchType());
        }

        private Sprite GetSpritesForMatchType()
        {
            var imageLibrary = ServiceProvider.GetImageLibrary;

            switch (_matchType)
            {
                case MatchType.Green:
                    return imageLibrary.GreenBalloonSprite;
                case MatchType.Yellow:
                    return imageLibrary.YellowBalloonSprite;
                case MatchType.Blue:
                    return imageLibrary.BlueBalloonSprite;
                case MatchType.Red:
                    return imageLibrary.RedBalloonSprite;
            }

            return null;
        }

        public override MatchType GetMatchType()
        {
            return _matchType;
        }

        public override bool IsTappable()
        {
            return false;
        }

        public override bool IsMatchable()
        {
            return false;
        }

        public override bool IsAdjacentBreakable()
        {
            return true;
        }
    }
}
