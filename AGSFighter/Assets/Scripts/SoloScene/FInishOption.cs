using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FInishOption : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button modeButton;
    [SerializeField]
    private Button titleButton;

    // Start is called before the first frame update
    void Start()
    {
        SetupMenuUIEvent();
    }

    public void ButtonSelect()
    {
        restartButton.Select();
    }


    public void SetupMenuUIEvent()
    {
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
