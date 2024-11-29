using System.Collections.Generic;
using System.Linq;


public class Inventory
    {
        private List<Item> _itemList;

        public Inventory()
        {
            _itemList = new List<Item>();
            
        }

        public void AddItem(Item item)
        {
            _itemList.Add(item);
        }

        public void RemoveItem(string itemName)
        {
            // Find the item with the specified name
            Item itemToRemove = _itemList.Find(item => item.itemName == itemName);
    
            // If found, remove it
            if (itemToRemove != null)
            {
                _itemList.Remove(itemToRemove);
            }
        }
        public List<Item> GetItemList()
        {
            return _itemList;
        }
        
        public bool HasItemWithName(string itemName)
        {
            return _itemList.Any(item => item.itemName == itemName);
        }
    }
