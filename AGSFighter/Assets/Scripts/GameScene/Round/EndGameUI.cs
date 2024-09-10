using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel; // エンドゲームパネル
    [SerializeField] private Text endGameMessageText; // エンドゲームメッセージを表示するテキスト

    private void Start()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false); // 初期状態でパネルを非表示にする
        }
    }

    // エンドゲームパネルを表示し、メッセージを設定する
    public void ShowEndGamePanel(string message)
    {
        if (endGamePanel != null && endGameMessageText != null)
        {
            endGameMessageText.text = message; // メッセージを設定する
            endGamePanel.SetActive(true); // パネルを表示する
        }
    }

    // エンドゲームパネルを非表示にする
    public void HideEndGamePanel()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false); // パネルを非表示にする
        }
    }
}
