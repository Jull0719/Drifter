using UnityEditor;
using UnityEngine;

public enum RewardType { Merchant, Inn, None }

[CreateAssetMenu(menuName = "Data/Quest/QuestData", fileName = "Quest - ")]
public class QuestDataSO : ScriptableObject
{
    public string questSaveId;
    [Space]
    [Header("任务信息")]
    public string questName;
    [TextArea] public string questDescription;
    
    [Header("任务目标")]
    public string targetId;
    public int targetAmount;

    [Header("任务奖励")]
    public RewardType rewardType;
    public Inventory_Item[] rewardItems;
    public int rewardMoney;

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        questSaveId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
