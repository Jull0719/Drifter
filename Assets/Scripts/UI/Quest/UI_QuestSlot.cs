using TMPro;
using UnityEngine;

public class UI_QuestSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questNameText;

    public void SetQuest(QuestDataSO quest)
    {
        questNameText.text = quest.questName;        
    }
}
