using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using SelectCharacter; // 追加

public class PadStageSelect : MonoBehaviour
{
    public GameObject[] topImages; // 上の画像の配列
    public GameObject[] searchImage; // 下の画像の配列
    public SelectStageScene selectStageScene; // SelectStageSceneの参照を追加

    private Vector2 currentStickInput;
    private Vector2 currentRightStickInput;
    private Vector2 currentLeftStickInput;
    private int currentIndex = 0;
    private bool stickInputReceived = false;
    private float stickInputCooldownDuration = 0.5f; // スティック入力のクールダウン時間（秒）
    private float stickInputCooldownTimer = 0f; // スティック入力のクールダウンタイマー

    void Start()
    {
        // 初期設定などがあればここに記述
    }

    void Update()
    {
        // ゲームパッドの入力を取得
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return;

        // Dパッドの入力を取得
        var dpadInput = gamepad.dpad.ReadValue();

        if (topImages.Length > 0)
        {
            // Dパッドの左入力があるかチェック
            if (dpadInput.x < 0 && dpadInput.y == 0)
            {

                if (!stickInputReceived)
                {
                    SoundManager.Instance.PlayUIClip("カーソル移動5");
                    // 左にスクロール
                    currentIndex = (currentIndex - 1 + topImages.Length) % topImages.Length;
                    SwitchTopImage(currentIndex);
                    SwitchSearchImage(currentIndex);
                    stickInputReceived = true;
                    stickInputCooldownTimer = stickInputCooldownDuration;
                }
            }
            // Dパッドの右入力があるかチェック
            else if (dpadInput.x > 0 && dpadInput.y == 0)
            {

                if (!stickInputReceived)
                {
                    SoundManager.Instance.PlayUIClip("カーソル移動5");
                    // 右にスクロール
                    currentIndex = (currentIndex + 1) % topImages.Length;
                    SwitchTopImage(currentIndex);
                    SwitchSearchImage(currentIndex);
                    stickInputReceived = true;
                    stickInputCooldownTimer = stickInputCooldownDuration;
                }
            }
        }

        // 右スティックの入力を取得
        currentRightStickInput = gamepad.rightStick.ReadValue();
        // 左スティックの入力を取得
        currentLeftStickInput = gamepad.leftStick.ReadValue();

        // 右スティックの水平方向の入力があるかチェック
        if (Mathf.Abs(currentRightStickInput.x) > 0.5f)
        {
            if (!stickInputReceived)
            {
                SoundManager.Instance.PlayUIClip("カーソル移動5");
                // 右スティックの水平方向の入力に応じて画像を切り替える
                SwitchImageByStickInput(currentRightStickInput.x);
                stickInputReceived = true;
                stickInputCooldownTimer = stickInputCooldownDuration;
            }
        }
        // 左スティックの水平方向の入力があるかチェック
        else if (Mathf.Abs(currentLeftStickInput.x) > 0.5f)
        {
            if (!stickInputReceived)
            {
                SoundManager.Instance.PlayUIClip("カーソル移動5");
                // 左スティックの水平方向の入力に応じて画像を切り替える
                SwitchImageByStickInput(currentLeftStickInput.x);
                stickInputReceived = true;
                stickInputCooldownTimer = stickInputCooldownDuration;
            }
        }

        // クールダウンタイマーを更新
        if (stickInputReceived)
        {
            stickInputCooldownTimer -= Time.deltaTime;
            if (stickInputCooldownTimer <= 0f)
            {
                stickInputReceived = false;
            }
        }

        // Aボタンが押されたかどうかをチェック
        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            // Aボタンが押されたときの処理を実行
            Debug.Log("A button pressed.");
            for (int i = 0; i < searchImage.Length; i++)
            {
                if (searchImage[i] != null)
                {
                    if (i == currentIndex)
                    {
                        Debug.Log("Bottom image selected: " + i);
                        // ステージ選択の処理を追加
                        string stageName = searchImage[i].name; // ここでステージ名を取得
                        Debug.Log("Selected stage: " + stageName);
                        SoundManager.Instance.PlayUIClip("決定ボタンを押す11");
                        selectStageScene.GoToOtherScene("Select"+stageName); // シーン遷移を実行
                    }
                }
            }
        }
    }

    void SwitchImageByStickInput(float stickHorizontalInput)
    {
        if (topImages.Length == 0)
            return;

        if (stickHorizontalInput > 0) // 右方向にスティックを倒した場合
        {
            currentIndex = (currentIndex + 1) % topImages.Length;
        }
        else if (stickHorizontalInput < 0) // 左方向にスティックを倒した場合
        {
            currentIndex = (currentIndex - 1 + topImages.Length) % topImages.Length;
        }

        SwitchTopImage(currentIndex);
        SwitchSearchImage(currentIndex);
    }

    void SwitchTopImage(int index)
    {
        for (int i = 0; i < topImages.Length; i++)
        {
            if (topImages[i] != null)
                topImages[i].SetActive(false);
        }
        if (index >= 0 && index < topImages.Length && topImages[index] != null)
            topImages[index].SetActive(true);
    }

    void SwitchSearchImage(int index)
    {
        for (int i = 0; i < searchImage.Length; i++)
        {
            if (searchImage[i] != null)
            {
                // 選択状態の見た目を変更する処理を追加
                // 例: searchImage[i].GetComponent<Image>().color = (i == index) ? Color.red : Color.white;
            }
        }
    }
}
