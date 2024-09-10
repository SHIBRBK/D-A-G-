using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OptionButton : MonoBehaviour
{
    [SerializeField]
    private GameObject option, optionBG, audioSource, commandList, commandSP, key;
    [SerializeField]
    private List<GameObject> commandImage;
    [SerializeField]
    private Button audioButton;
    [SerializeField]
    private Button commandButton;
    [SerializeField]
    private Button keyButton;
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    private Button closeButton;
    bool optionActive = false;

    // Start is called before the first frame update
    void Start()
    {
        SetupMenuUIEvent();
    }
    private void Update()
    {
        if(OptionActiveFalse(audioSource.activeSelf))
        {
            option.SetActive(true);
            optionBG.SetActive(false);
            audioSource.SetActive(false);
        }
        else if(OptionActiveFalse(commandList.activeSelf))
        {
            option.SetActive(true);
            optionBG.SetActive(false);
            commandList.SetActive(false);
            foreach(GameObject command in commandImage)
            {
                command.SetActive(false);
            }
        }
        else if(OptionActiveFalse(key.activeSelf))
        {
            option.SetActive(true);
            optionBG.SetActive(false);
            key.SetActive(false);
        }
    }

    private bool OptionActiveFalse(bool optionActive)
    {
        return optionActive && (Input.GetMouseButtonDown(1)
            || Input.GetKeyDown(KeyCode.Joystick1Button1));
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }
        if(!optionActive)
        {
            Time.timeScale = 0f;
            optionActive = true;
            option.SetActive(true);
            audioButton.Select();
        }
        else
        {
            optionActive = false;
            Time.timeScale = 1f;
            option.SetActive(false);
            audioSource.SetActive(false);
            commandList.SetActive(false);
            foreach (GameObject command in commandImage)
            {
                command.SetActive(false);
            }
            key.SetActive(false);
        }
    }

    public void SetupMenuUIEvent()
    {
        audioButton.onClick.AddListener(() =>
        {
            option.SetActive(false);
            optionBG.SetActive(true);
            audioSource.SetActive(true);
        });

        commandButton.onClick.AddListener(() =>
        {
            option.SetActive(false);
            optionBG.SetActive(true);
            commandList.SetActive(true);
            commandSP.SetActive(true);
        });

        keyButton.onClick.AddListener(() =>
        {
            option.SetActive(false);
            optionBG.SetActive(true);
            key.SetActive(true);
        });

        titleButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleScene");
            SoundManager.Instance.StopBGM();
        });

        closeButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            option.SetActive(false);
            optionActive = false;
        });
    }
}
