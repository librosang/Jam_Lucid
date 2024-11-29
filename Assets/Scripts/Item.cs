
    using UnityEngine;

    public class Item
    {
        public enum ItemType
        {
            Flashlight,
            Battery,
            Cable,
            NightVision,
            StunGun
        }

        public ItemType itemType;
        public int amount;
        public string itemName;
        public string itemDescription;

        public Sprite GetSprite()
        {

            switch (itemType)
            {
                default:
                case ItemType.Flashlight: return ItemAssets.instance.flashlight;
                case ItemType.Battery: return ItemAssets.instance.battery;
                case ItemType.Cable: return ItemAssets.instance.cable;
                case ItemType.NightVision: return ItemAssets.instance.nightvision;
                case ItemType.StunGun: return ItemAssets.instance.stungun;
            }
        }
    }
