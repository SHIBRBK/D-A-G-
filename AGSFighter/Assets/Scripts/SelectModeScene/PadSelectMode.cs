using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PadSelectMode : MonoBehaviour
{
    public Selectable[] buttons; // ボタンの配列
    private int currentIndex = 0; // 現在のフォーカスされているボタンのインデックス
    private float maxIdleTime = 5f; // フォーカス解除までの待機時間
    private float ignoreInputTime = 1f; // PAD 入力を無視する時間
    private float timeSinceLastInput = 0f; // 最後の入力からの経過時間
    private Vector2 previousMousePosition; // 前フレームのマウス座標
    private bool inputEnabled = true; // PAD 入力が有効かどうかのフラグ
    private bool canPlaySound = true; // サウンド再生が可能かどうかのフラグ
    private float soundCooldown = 0.2f; // サウンド再生のクールダウンタイム

    private float previousHorizontalInput = 0f; // 前フレームの水平方向入力値
    private bool[] buttonHovered; // ボタンのホバー状態を追跡する配列

    void Start()
    {
        // 初期状態で両方のボタンのフォーカスを解除する
        EventSystem.current.SetSelectedGameObject(null);
        buttonHovered = new bool[buttons.Length];
    }

    void Update()
    {
        if (!inputEnabled)
            return; // PAD 入力が無効化されている場合は処理を行わない

        // ゲームパッドの入力を取得
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            // 入力があった場合はフォーカス解除のカウントをリセット
            if (gamepad.leftStick.x.ReadValue() != 0 || gamepad.buttonSouth.wasPressedThisFrame)
            {
                timeSinceLastInput = 0f;
            }
            else
            {
                timeSinceLastInput += Time.deltaTime;
                if (timeSinceLastInput >= maxIdleTime)
                {
                    // 一定時間入力がなかった場合はフォーカスを解除
                    EventSystem.current.SetSelectedGameObject(null);
                    return;
                }
            }

            // ボタンのフォーカスを水平方向の入力に応じて切り替える
            float horizontalInput = gamepad.leftStick.x.ReadValue();
            if (Mathf.Abs(horizontalInput) > 0.5f && canPlaySound && horizontalInput != previousHorizontalInput)
            {
                // ボタンのフォーカスを切り替える
                currentIndex += (int)Mathf.Sign(horizontalInput);
                currentIndex = Mathf.Clamp(currentIndex, 0, buttons.Length - 1);
                EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
                SoundManager.Instance.PlayUIClip("カーソル移動5");
                StartCoroutine(SoundCooldown()); // サウンドのクールダウンタイムを開始
            }

            // 水平方向の入力値を更新
            previousHorizontalInput = horizontalInput;

            // PADのAボタンが押されたときに、選択されているボタンのOnClickイベントをトリガーする
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                SoundManager.Instance.PlayUIClip("決定ボタンを押す11");
                // 選択されているボタンがあればOnClickイベントをトリガーする
                if (buttons.Length > 0 && currentIndex >= 0 && currentIndex < buttons.Length)
                {
                    // ボタンのインデックスによって遷移先を決定する
                    switch (currentIndex)
                    {
                        case 0: // 配列の一つ目のボタンが押された場合
                            SceneManager.LoadScene("SelectStageScene");
                            break;
                        case 1: // 配列の二つ目のボタンが押された場合
                            SceneManager.LoadScene("SoloGameScene");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // マウスの入力を検知してフォーカスを設定する
        Vector2 mousePosition = Input.mousePosition;
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
            {
                if (!buttonHovered[i])
                {
                    buttonHovered[i] = true;
                    SoundManager.Instance.PlayUIClip("カーソル移動5");
                }
            }
            else
            {
                buttonHovered[i] = false;
            }
        }

        // マウスクリック時のサウンドを再生
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject != null)
            {
                SoundManager.Instance.PlayUIClip("決定ボタンを押す11");
            }
        }
    }

    // サウンド再生のクールダウンタイムを制御するコルーチン
    private IEnumerator SoundCooldown()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(soundCooldown);
        canPlaySound = true;
    }

    // シーンがロードされた際に呼び出されるコールバック
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // シーンがアンロードされる際に呼び出されるコールバック
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンがロードされた時の処理
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // セレクトモードシーンがロードされた時、PAD入力を無効化する
        if (scene.name == "SelectModeScene")
        {
            inputEnabled = false;
            Invoke("EnableInput", ignoreInputTime); // 一定時間後に PAD 入力を有効化する
            SoundManager.Instance.PlayUIClip("シーンロードサウンド");
        }
    }

    // PAD 入力を有効化するメソッド
    void EnableInput()
    {
        inputEnabled = true;
    }
}
