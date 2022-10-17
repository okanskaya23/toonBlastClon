using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class CubeItem : Item
    {
        private MatchType _matchType;

        public void PrepareCubeItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            Prepare(itemBase, GetSpritesForBombHint());
            Prepare(itemBase, GetSpritesForRocketHint());
            Prepare(itemBase, GetSpritesForMatchType());
        }

        private Sprite GetSpritesForMatchType()
        {
            var imageLibrary = ServiceProvider.GetImageLibrary;

            switch (_matchType)
            {
                case MatchType.Green:
                    return imageLibrary.GreenCubeSprite;
                case MatchType.Yellow:
                    return imageLibrary.YellowCubeSprite;
                case MatchType.Blue:
                    return imageLibrary.BlueCubeSprite;
                case MatchType.Red:
                    return imageLibrary.RedCubeSprite;
            }

            return null;
        }
        private Sprite GetSpritesForBombHint()
        {
            var imageLibrary = ServiceProvider.GetImageLibrary;

            switch (_matchType)
            {
                case MatchType.Green:
                    return imageLibrary.GreenCubeBombHintSprite;
                case MatchType.Yellow:
                    return imageLibrary.YellowCubeBombHintSprite;
                case MatchType.Blue:
                    return imageLibrary.BlueCubeBombHintSprite;
                case MatchType.Red:
                    return imageLibrary.RedCubeBombHintSprite;
            }

            return null;
        }
        private Sprite GetSpritesForRocketHint()
        {
            var imageLibrary = ServiceProvider.GetImageLibrary;

            switch (_matchType)
            {
                case MatchType.Green:
                    return imageLibrary.GreenCubeRocketHintSprite;
                case MatchType.Yellow:
                    return imageLibrary.YellowCubeRocketHintSprite;
                case MatchType.Blue:
                    return imageLibrary.BlueCubeRocketHintSprite;
                case MatchType.Red:
                    return imageLibrary.RedCubeRocketHintSprite;
            }

            return null;
        }

        public override MatchType GetMatchType()
        {
            return _matchType;
        }

        public override void TryExecute()
        {
            ServiceProvider.GetParticleManager.PlayCubeParticle(this);

            base.TryExecute();
        }
    }
}
