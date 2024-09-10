using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button backBattleButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button modeButton;
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    GameObject menuUI,menuBG;
    private int i = 0;


    // Start is called before the first frame update
    void Start()
    {
        SetupMenuUIEvent();
    }

    private void OnEnable()
    {
        backBattleButton.Select();
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }
        if(i == 1)
        {
            Time.timeScale = 1f;
            i = 0;
            menuUI.SetActive(false);
            menuBG.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            i = 1;
            menuUI.SetActive(true);
            menuBG.SetActive(true);
        }
    }

    public void SetupMenuUIEvent()
    {
        backBattleButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            i = 0;
            menuUI.SetActive(false);
            menuBG.SetActive(false);
        });

        restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("SoloGameScene");
        });

        modeButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("SelectModeScene");
        });

        titleButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleScene");
        });
    }
}
