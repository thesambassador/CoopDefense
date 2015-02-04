using UnityEngine;
using System.Collections;

public enum ItemType
{
    Primary,
    Secondary,
    Utility,
    Passive
};

public abstract class Item
{
    public ItemType itemType;
    public string Name;

    public abstract void OnEquip();
    public abstract void OnUnequip();
    public abstract void OnUse();
    public abstract void PassiveEffect();
    public abstract void OnUpdate();

}

