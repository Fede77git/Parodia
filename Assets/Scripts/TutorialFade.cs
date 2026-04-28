using UnityEngine;
using System.Collections;


public class TutorialFade : MonoBehaviour
{
    public float waitTime = 4f;
    public float fadeDuration = 1.5f;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        yield return new WaitForSeconds(waitTime);

        float currentTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, currentTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
