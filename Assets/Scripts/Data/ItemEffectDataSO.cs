using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item Effect", fileName = "Item Effect - ")]
public class ItemEffectDataSO : ScriptableObject
{
    [TextArea] public string effectDescription;

    public virtual void Execute()
    {

    }
}
