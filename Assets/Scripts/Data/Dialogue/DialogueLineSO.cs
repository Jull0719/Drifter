using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialogue Data/New Line Data", fileName = "Line - ")]
public class DialogueLineSO : ScriptableObject
{
    [Header("对话信息")]
    public string dialogueGroupName;
    public DialogueSpeakerSO speaker;

    [Header("对话内容")]
    [TextArea] public string[] textLine;

    [Header("对话响应")]
    [TextArea] public string actionLine;
    public DialogueActionType actionType;

    [Header("回答设置")]
    [TextArea] public string playerChoiceAnswer;
    public DialogueLineSO[] choiceLines;

    // 获取第一句话
    public string GetFirstLine() => textLine[0];

    // 随机获取一条对话
    public string GetRandomLine() => textLine[Random.Range(0, textLine.Length)];
}
