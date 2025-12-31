using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea] public string effectDescription;

    public virtual bool CanBeUsed()
    {
        return true;
    }

    public virtual void Execute()
    {

    }
}
