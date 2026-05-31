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
    [SerializeField] private TextMeshProUGUI[] dialogueChoicesText;
    [Header("打字机效果")]
    [SerializeField] private float textSpeed = 0.1f;
    private string fullTextToShow;
    private Coroutine typeTextCo;
    [Header("当前对话")]
    private DialogueLineSO currentLine;
    private DialogueLineSO[] currentChoices;
    private DialogueLineSO selectedChoice;
    private int selectedChoiceIndex;

    private bool canInteract;
    private bool waitToConfirm;

    public void PlayDialogueLine(DialogueLineSO line)
    {
        HideAllChoices();

        currentLine = line;
        currentChoices = line.choiceLines;
        canInteract = false;

        speakerPortrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;

        fullTextToShow = line.actionType == DialogueActionType.None || 
                         line.actionType == DialogueActionType.PlayerMakeChoice ?
                         line.GetRandomLine() : line.actionLine;

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

        if (waitToConfirm || selectedChoice != null)
        {
            waitToConfirm = false;
            HandleNextAction();
        }
    }

    /// <summary>
    /// 选择对话选项
    /// </summary>
    /// <param name="direction">方向：-1 向上，1 向下</param>
    public void DialogueNavigation(int direction)
    {
        if (currentChoices == null || currentChoices.Length <= 1) return;

        selectedChoiceIndex += direction;
        selectedChoiceIndex = Mathf.Clamp(selectedChoiceIndex, 0, currentChoices.Length - 1);
        ShowChoices();
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
            case DialogueActionType.PlayerMakeChoice:
                if (selectedChoice == null)
                {
                    selectedChoiceIndex = 0;
                    ShowChoices();
                }
                else
                {
                    DialogueLineSO selectedChoice = currentChoices[selectedChoiceIndex];
                    PlayDialogueLine(selectedChoice);
                    selectedChoice = null;
                }
                break;
        }
    }

    IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract = true;
    }

    /// <summary>
    /// 显示对话选项
    /// </summary>
    private void ShowChoices()
    {
        for (int i = 0; i < dialogueChoicesText.Length; i++)
        {
            if (i < currentChoices.Length)
            {
                DialogueLineSO choice = currentChoices[i];
                string choiceText = choice.GetFirstLine();

                dialogueChoicesText[i].gameObject.SetActive(true);
                dialogueChoicesText[i].text = selectedChoiceIndex == i ?
                    $"<color=yellow>{i + 1}) {choiceText}" : $"{i + 1}) {choiceText}";
            }
            else
            {
                dialogueChoicesText[i].gameObject.SetActive(false);
            }
        }

        selectedChoice = currentChoices[selectedChoiceIndex];
    }

    /// <summary>
    /// 隐藏所有对话选项
    /// </summary>
    private void HideAllChoices()
    {
        foreach (var choice in dialogueChoicesText)
            choice.gameObject.SetActive(false);
    }
}
