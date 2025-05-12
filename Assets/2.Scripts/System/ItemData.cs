using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string itemName;
    public Sprite icon;
    public string description;
    public int buy;
    public int sell;
    public string type;
    public int upgrade;
}
