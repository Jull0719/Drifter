using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : MonoBehaviour
{
    [Header("说话人信息")]
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [Space]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueChoices;
    [Header("打字机效果")]
    [SerializeField] private float textSpeed = 0.1f;
    private string fullTextToShow;
    private Coroutine typeTextCo;

    public void PlayDialogueLine(DialogueLineSO line)
    {
        speakerPortrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;

        fullTextToShow = line.GetRandomLine();
        typeTextCo = StartCoroutine(TypeTextCo(fullTextToShow));
    }

    public void DialogueInteraction()
    {
        if (typeTextCo != null && dialogueText.text.Length > 5)
        {
            completeTyping();
            return;
        }
    }

    // 结束键入效果，显示整句话
    private void completeTyping()
    {
        if (typeTextCo != null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text = fullTextToShow;
        }
    }

    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
