using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialogue Data/New Line Data", fileName = "Line - ")]
public class DialogueLineSO : ScriptableObject
{
    [Header("对话信息")]
    public string dialogueGroupName;
    public DialogueSpeakerSO speaker;

    [Header("对话内容")]
    [TextArea] public string[] textLine;

    [Header("回答设置")]
    public bool playerCanAnswer; // 如果Player可以回答，设置为true
    public DialogueLineSO[] answerLine;

    // 随机获取一条对话
    public string GetRandomLine()
    {
        return textLine[Random.Range(0, textLine.Length)];
    }
}
