using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item Data Base", fileName = "Item Data Base - ")]
public class ItemDataBaseSO : ScriptableObject
{
    public ItemDataSO[] itemDataList;

    public ItemDataSO FindItemDataById(string itemId)
    {
        return itemDataList.FirstOrDefault(item => item != null && item.saveId == itemId);
    }

#if UNITY_EDITOR
    [ContextMenu("收集所有物品数据")]
    public void CollectAllItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");

        itemDataList = guids.
            Select(guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
