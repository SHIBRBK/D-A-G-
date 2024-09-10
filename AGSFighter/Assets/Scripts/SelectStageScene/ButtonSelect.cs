using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public GameObject[] image;
    public GameObject[] searchImage;

    private Image currentImage;
    private bool[] buttonHovered;

    void Start()
    {
        currentImage = GetComponent<Image>();
        buttonHovered = new bool[searchImage.Length];
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // 下の画像の座標ごとに上の画像を切り替える
        for (int i = 0; i < searchImage.Length; i++)
        {
            if (searchImage[i] != null)
            {
                // 下の画像のRectTransformを取得
                RectTransform rectTransform = searchImage[i].GetComponent<RectTransform>();

                // マウスカーソルがボタンの上にあるかどうかを確認
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
                {
                    if (!buttonHovered[i])
                    {
                        buttonHovered[i] = true;
                        SoundManager.Instance.PlayUIClip("カーソル移動5");
                    }
                    SwitchTopImage(i);
                }
                else
                {
                    buttonHovered[i] = false;
                }
            }
        }
    }

    // 上の画像を切り替えるメソッド
    void SwitchTopImage(int index)
    {
        for (int i = 0; i < image.Length; i++)
        {
            if (image[i] != null)
            {
                // すべての上の画像を非表示にする
                image[i].SetActive(false);
            }
        }
        // 対応する上の画像を表示する
        if (index >= 0 && index < image.Length && image[index] != null)
        {
            image[index].SetActive(true);
        }
    }
}
