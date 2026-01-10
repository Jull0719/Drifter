using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea] public string effectDescription;
    protected Player player;

    public virtual bool CanBeUsed()
    {
        return true;
    }

    public virtual void Execute()
    {

    }

    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    public virtual void Unsubscribe()
    {

    }
}
