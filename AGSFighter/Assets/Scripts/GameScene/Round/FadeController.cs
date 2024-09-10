// FadeController.cs

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;
    string sceneName;

    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDuration(float duration)
    {
        fadeDuration = duration;
    }

    public IEnumerator FadeIn()
    {
        yield return FadeCanvasGroup(1, 0, fadeDuration);
    }

    public IEnumerator FadeOut()
    {
        yield return FadeCanvasGroup(0, 1, fadeDuration);
    }

    private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = endAlpha;
    }
}
