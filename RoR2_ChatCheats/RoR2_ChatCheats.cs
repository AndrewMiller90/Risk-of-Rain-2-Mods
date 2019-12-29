using BepInEx;
using Frogtown;
using RoR2;
using System;
using UnityEngine;

namespace RoR2_ChatCheats
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.sluppy.chatcheats", "ChatCheats", "1.0.0")]
    public class RoR_ChatCheats : BaseUnityPlugin
    {
        void Awake()
        {
            FrogtownShared.AddChatCommand("change_char", OnCharCommand);
            FrogtownShared.AddChatCommand("give_item", OnGiveCommand);
            FrogtownShared.AddChatCommand("remove_item", OnRemoveCommand);
            FrogtownShared.AddChatCommand("clear_items", OnClearItemsCommand);
        }
        private void SendFailedLookupMessage()
        {
            FrogtownShared.SendChat("Failed to lookup item.");
        }
        private bool OnRemoveCommand(string userName, string[] pieces)
        {
            var player = FrogtownShared.GetPlayerWithName(userName);

            ItemWithCount itemWithCount = null;
            try
            {
                itemWithCount = ParseItemWithCount(pieces);
            } catch(ArgumentException e)
            {
                SendFailedLookupMessage();
                return true;
            }

            int countPlayerHas = player.master.inventory.GetItemCount((ItemIndex)itemWithCount.index);
            itemWithCount.count = Math.Min(Math.Max(itemWithCount.count, 1), countPlayerHas);

            FrogtownShared.SendChat(string.Format("countPlayerHas = {0}\r\nitemWithCount.count = {1}", countPlayerHas, itemWithCount.count));
            
            if (itemWithCount.count > 0)
            {
                player.master.inventory.GiveItem((ItemIndex)itemWithCount.index, -itemWithCount.count);
                FrogtownShared.SendChat(string.Format("Took {0} items from {1}.", itemWithCount.count, userName));
            }

            return true;
        }

        private bool OnClearItemsCommand(string userName, string[] pieces)
        {
            var player = FrogtownShared.GetPlayerWithName(userName);
            foreach (ItemIndex itemIndex in ItemCatalog.allItems)
            {
                int count = player.master.inventory.GetItemCount(itemIndex);
                OnRemoveCommand(userName, new[] { pieces[0], itemIndex.ToString(), count.ToString() });
            }
            FrogtownShared.SendChat("Took all items from " + userName + ".");

            return true;
        }

        private bool OnGiveCommand(string userName, string[] pieces)
        {
            var player = FrogtownShared.GetPlayerWithName(userName);
            ItemWithCount itemWithCount = null;
            try
            {
                itemWithCount = ParseItemWithCount(pieces);
                if (itemWithCount.index < 0 || itemWithCount.index >= (int)ItemIndex.Count)
                {
                    SendFailedLookupMessage();
                    return true;
                }
            } catch(ArgumentException e)
            {
                SendFailedLookupMessage();
                return true;
            }
            itemWithCount.count = Math.Max(itemWithCount.count, 1);

            player.master.inventory.GiveItem((ItemIndex)itemWithCount.index, itemWithCount.count);
            FrogtownShared.SendChat(string.Format("Gave {0} items to {1}.", itemWithCount.count, userName));

            return true;
        }

        private bool OnCharCommand(string userName, string[] pieces)
        {
            if (pieces.Length >= 2)
            {
                int prefabIndex = -1;
                if (!Int32.TryParse(pieces[1], out prefabIndex))
                {
                    prefabIndex = BodyCatalog.FindBodyIndexCaseInsensitive(pieces[1]);
                }
                if (prefabIndex != -1)
                {
                    GameObject prefab = BodyCatalog.GetBodyPrefab(prefabIndex);

                    if (prefab != null)
                    {
                        if (FrogtownShared.ChangePrefab(userName, prefab))
                        {
                            FrogtownShared.SendChat(userName + " morphed into " + prefab.name);
                        }
                        else
                        {
                            FrogtownShared.SendChat(userName + " couldn't morph into " + prefab.name);
                        }
                    }
                    else
                    {
                        FrogtownShared.SendChat("Prefab not found");
                    }
                }
            }

            return true;
        }

        private ItemWithCount ParseItemWithCount(string[] pieces)
        {
            int count = 1;
            int index = -1;
            var itemWithCount = new ItemWithCount();
            if (pieces.Length >= 2)
            {
                if (!TryParseItemIndex(pieces[1], out index))
                {
                    throw new ArgumentException(nameof(pieces));
                }
                itemWithCount.index = index;
            }
            if (pieces.Length >= 3)
            {
                Int32.TryParse(pieces[2], out count);
            }
            itemWithCount.count = count;
            return itemWithCount;
        }

        private int ParseItemIndex(string value)
        {
            if (Int32.TryParse(value, out int output))
            {
                return output;
            }
            if (Enum.TryParse(value, true, out ItemIndexOverride itemIndexOverrideOut))
            {
                return (int)itemIndexOverrideOut;
            }
            if (Enum.TryParse(value, true, out ItemIndex itemIndexOut))
            {
                return (int)itemIndexOut;
            }
            throw new ArgumentException(nameof(value));
        }
        private bool TryParseItemIndex(string value, out int itemIndex)
        {
            itemIndex = -1;
            try
            {
                itemIndex = ParseItemIndex(value);
            }
            catch (ArgumentException e)
            {
                return false;
            }
            return true;
        }

        private class ItemWithCount
        {
            public int index { get; set; } = -1;
            public int count { get; set; } = 0;
        }
    }
}
