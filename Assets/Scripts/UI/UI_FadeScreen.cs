using System.Collections;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    public Coroutine fadeCo { get; private set; }
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        //canvasGroup.alpha = 1;
    }

    /// <summary>
    /// 渐入，从黑变为透明
    /// </summary>
    public void FadeIn(float duration = 1)
    {
        canvasGroup.alpha = 1;
        Fade(0, duration);
    }

    /// <summary>
    /// 渐出，从透明变黑
    /// </summary>
    public void FadeOut(float duration = 1)
    {
        canvasGroup.alpha = 0;
        Fade(1, duration);
    }

    private void Fade(float targetAlpha, float targetDuration)
    {
        if (fadeCo != null)
            StopCoroutine(fadeCo);
        fadeCo = StartCoroutine(FadeCo(targetAlpha, targetDuration));
    }

    private IEnumerator FadeCo(float targetAlpha, float targetDuration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < targetDuration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / targetDuration);

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
