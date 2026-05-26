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
    [Header("当前对话")]
    private DialogueLineSO currentLine;
    private bool canInteract;
    private bool waitToConfirm;

    public void PlayDialogueLine(DialogueLineSO line)
    {
        currentLine = line;
        canInteract = false;

        speakerPortrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;

        fullTextToShow = line.GetRandomLine();
        typeTextCo = StartCoroutine(TypeTextCo(fullTextToShow));

        StartCoroutine(EnableInteractionCo());
    }

    public void DialogueInteraction()
    {
        if (canInteract == false)
            return;

        if (typeTextCo != null)
        {
            completeTyping();
            waitToConfirm = true;
            return;
        }

        if (waitToConfirm)
        {
            waitToConfirm = false;
            HandleNextAction();
        }
    }

    // 结束键入效果，显示整句话
    private void completeTyping()
    {
        if (typeTextCo != null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text = fullTextToShow;
            typeTextCo = null;
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

        waitToConfirm = true;
        typeTextCo = null;
    }

    // 对话响应
    private void HandleNextAction()
    {
        switch (currentLine.actionType)
        {
            case DialogueActionType.OpenShop:
                UI.instance.SwitchToInGameUI();
                UI.instance.OpenShopUI(true);
                UI.instance.ToggleInventoryUI();
                break;
        }
    }

    IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract = true;
    }
}
