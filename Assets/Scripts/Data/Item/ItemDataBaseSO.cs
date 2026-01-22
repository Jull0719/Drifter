using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item Data Base", fileName = "Item Data Base - ")]
public class ItemDataBaseSO : ScriptableObject
{
    public ItemDataSO[] itemDataList;
}
