using Game.Core.Enums;
using Game.Items;
using Game.Managers;
using UnityEngine;

namespace Game.Core.ItemBase
{
    public class ItemFactory : MonoBehaviour, IProvidable
    {
        [SerializeField] private ItemBase ItemBasePrefab;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public Item CreateItem(ItemType itemType, Transform parent)
        {
            if (itemType == ItemType.None)
            {
                return null;
            }

            var itemBase = Instantiate(ItemBasePrefab, Vector3.zero, Quaternion.identity, parent);

            Item item = null;
            switch (itemType)
            {
                case ItemType.GreenCube:
                    item = CreateCubeItem(itemBase, MatchType.Green);
                    break;
                case ItemType.YellowCube:
                    item = CreateCubeItem(itemBase, MatchType.Yellow);
                    break;
                case ItemType.BlueCube:
                    item = CreateCubeItem(itemBase, MatchType.Blue);
                    break;
                case ItemType.RedCube:
                    item = CreateCubeItem(itemBase, MatchType.Red);
                    break;
                case ItemType.Crate:
                    item = CreateCrateItem(itemBase);
                    break;
                case ItemType.Balloon:
                    item = CreateBalloonItem(itemBase);
                    break;
                case ItemType.BlueBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Blue);
                    break;
                case ItemType.GreenBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Green);
                    break;
                case ItemType.RedBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Red);
                    break;
                case ItemType.YellowBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Yellow);
                    break;
                case ItemType.Bomb:
                    item = CreateBombItem(itemBase);
                    break;
                case ItemType.VerticalRocket:
                    item = CreateVerticalRocket(itemBase);
                    break;
                case ItemType.HorizontalRocket:
                    item = CreateHorizontalRocket(itemBase);
                    break;
                default:
                    Debug.LogWarning("Can not create item: " + itemType);
                    break;
            }

            item.ItemType = itemType;
            return item;
        }

        private Item CreateCubeItem(ItemBase itemBase, MatchType matchType)
        {
            var cubeItem = itemBase.gameObject.AddComponent<CubeItem>();
            cubeItem.PrepareCubeItem(itemBase, matchType);

            return cubeItem;
        }

        private Item CreateCrateItem(ItemBase itemBase)
        {
            var crateItem = itemBase.gameObject.AddComponent<CrateItem>();
            crateItem.PrepareCrateItem(itemBase);
            return crateItem;
        }

        private Item CreateBalloonItem(ItemBase itemBase)
        {
            var balloonItem = itemBase.gameObject.AddComponent<BalloonItem>();
            balloonItem.PrepareBalloonItem(itemBase);
            return balloonItem;
        }

        private Item CreateColorBalloonItem(ItemBase itemBase, MatchType matchType)
        {
            var colorBalloonItem = itemBase.gameObject.AddComponent<ColorBalloonItem>();
            colorBalloonItem.PrepareColorBalloonItem(itemBase, matchType);

            return colorBalloonItem;
        }

        private Item CreateBombItem(ItemBase itemBase)
        {
            var bombItem = itemBase.gameObject.AddComponent<BombItem>();
            bombItem.PrepareBombItem(itemBase);
            return bombItem;
        }
        private Item CreateVerticalRocket(ItemBase itemBase)
        {
            var verticalRocketItem = itemBase.gameObject.AddComponent<VerticalRocketItem>();
            verticalRocketItem.PrepareVerticalRocketItem(itemBase);
            return verticalRocketItem;
        }
        private Item CreateHorizontalRocket(ItemBase itemBase)
        {
            var horizontalRocketItem = itemBase.gameObject.AddComponent<HorizontalRocketItem>();
            horizontalRocketItem.PrepareHorizontalRocketItem(itemBase);
            return horizontalRocketItem;
        }
    }
}