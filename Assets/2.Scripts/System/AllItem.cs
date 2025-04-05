using UnityEngine;

using System.Collections.Generic;

[CreateAssetMenu(fileName = "AllItem", menuName = "Scriptable Objects/AllItem")]
public class AllItem : ScriptableObject
{
    public List<ItemData> items;
    public ItemData GetItemById(int id)
    {
        return items.Find(i => i.ID == id);
    }
}
