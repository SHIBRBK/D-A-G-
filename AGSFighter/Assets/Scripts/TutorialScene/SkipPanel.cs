using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipPanel : MonoBehaviour
{
    //スキップ確認パネル
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Button leftButton;
    [SerializeField]
    private Button rightButton;

    //チュートリアルを受けるかどうかのテキスト
    [SerializeField]
    private GameObject tutoText;
    [SerializeField]
    private GameObject tutoTextSkip;

    //チュートリアルをスキップするかどうかのテキスト
    [SerializeField]
    private GameObject skipText;
    [SerializeField]
    private GameObject skipTextNo;

    //最初のチュートリアル
    [SerializeField]
    private GameObject tuto1;

    //チュートリアルを集めた親
    [SerializeField]
    private GameObject tuto;

    //プレイヤーのState変更のため
    [SerializeField]
    private GameObject player;
    private PlayerAction playerState;
    private bool tutorialSkipped = false;

    private void Start()
    {
        StartCoroutine(FadeController.Instance.FadeIn());
        playerState = player.GetComponent<PlayerAction>(); 
        playerState.State = PlayerAction.MyState.Freeze;
        SetUpMenuUIEvent();
    }

    private void OnEnable()
    {
        leftButton.Select();
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }
        //チュートリアルの途中でスキップパネルを出す
        if (playerState.State == PlayerAction.MyState.Game)
        {
            SkipTutorial();
            leftButton.Select();
        }
    }

    //チュートリアルをスキップ
    private void SkipTutorial()
    {
        tuto.SetActive(false);
        panel.SetActive(true);
        skipText.SetActive(true);
        tutoText.SetActive(false);
        tutoTextSkip.SetActive(false);
        skipTextNo.SetActive(true);
        playerState.State = PlayerAction.MyState.Freeze;
        tutorialSkipped = true;
    }

    //シーン読み込み
    private void LoadSelectModeScene()
    {
        SceneManager.LoadScene("SelectModeScene");
    }

    private void SetUpMenuUIEvent()
    {
        leftButton.onClick.AddListener(() =>
        {
            tuto.SetActive(true);
            panel.SetActive(false);
            tutoText.SetActive(false);
            skipText.SetActive(false);

            //左側を肯定系のボタンにしたいため
            //チュートリアルの途中でスキップパネルが表示されていたら
            //スキップする用ボタンになる
            if (!tutorialSkipped)
            {
                tuto1.SetActive(true);
            }
            else
            {
                LoadSelectModeScene();
            }

            playerState.State = PlayerAction.MyState.Game;
        });

        rightButton.onClick.AddListener(() =>
        {
            if (!tutorialSkipped)
            {
                LoadSelectModeScene();
            }
            else
            {
                tuto.SetActive(true);
                skipText.SetActive(false);
                panel.SetActive(false);
                playerState.State = PlayerAction.MyState.Game;
            }
        });
    }
}
