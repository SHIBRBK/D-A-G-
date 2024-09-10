using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private CanvasGroup fadeCanvasGroup;
    private GameObject fade;
    public float fadeDuration = 1.0f;

    public enum GameMode
    {
        Solo,
        VS
    }

    public GameMode SelectedGameMode { get; private set; }

    private void Awake()
    {
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

    private void Start()
    {
        fade = GameObject.FindGameObjectWithTag("Fade");
        fadeCanvasGroup = fade.GetComponent<CanvasGroup>();
        if (fadeCanvasGroup != null)
        {
            // ゲーム開始時にフェードイン
            FadeController.Instance.StartCoroutine(FadeController.Instance.FadeIn());
        }
    }

    // ラウンドの開始処理
    public void StartRound()
    {
        // ラウンド開始時の処理をここに記述
    }

    // ラウンド終了時の処理
    public void EndRound()
    {
        // ラウンド終了時にフェードアウトし、次のラウンドの準備
        StartCoroutine(EndRoundCoroutine());
    }

    private IEnumerator EndRoundCoroutine()
    {
        yield return StartCoroutine(FadeOut());
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        fadeCanvasGroup.gameObject.SetActive(true); // CanvasGroupを表示
        fadeCanvasGroup.alpha = 0;
        while (fadeCanvasGroup.alpha < 1.0f)
        {
            fadeCanvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1;
        while (fadeCanvasGroup.alpha > 0.0f)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.gameObject.SetActive(false); // CanvasGroupを非表示
    }

    public void SetGameMode(GameMode mode)
    {
        SelectedGameMode = mode;
    }

}
