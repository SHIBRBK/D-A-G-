using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinStarsUI : MonoBehaviour
{
    public List<Image> winStars; // 勝ち星を表示するImageコンポーネントのリスト
    public Sprite defaultSprite; // 初期の画像
    public Sprite winSprite; // 勝ち星が獲得されたときの画像

    private void Start()
    {
        ResetWinStars();
    }

    // 勝ち星を更新するメソッド
    public void UpdateWinStars(int wins)
    {
        for (int i = 0; i < winStars.Count; i++)
        {
            if (i < wins)
            {
                winStars[i].sprite = winSprite;
            }
            else
            {
                winStars[i].sprite = defaultSprite;
            }
        }
    }

    // 勝ち星を初期状態にリセットするメソッド
    public void ResetWinStars()
    {
        foreach (var star in winStars)
        {
            star.sprite = defaultSprite;
        }
    }

}
