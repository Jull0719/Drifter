using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quest/QuestDataBase", fileName = "QuestDataBase - ")]
public class QuestDataBaseSO : ScriptableObject
{
    public QuestDataSO[] questDataList;

    public QuestDataSO GetQuestDataById(string questId)
    {
        return questDataList.FirstOrDefault<QuestDataSO>(quest => quest != null && quest.questSaveId == questId);
    }

#if UNITY_EDITOR
    [ContextMenu("收集所有任务数据")]
    public void CollectAllQuestsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:QuestDataSO");

        questDataList = guids.
            Select(guid => AssetDatabase.LoadAssetAtPath<QuestDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(quest => quest != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
