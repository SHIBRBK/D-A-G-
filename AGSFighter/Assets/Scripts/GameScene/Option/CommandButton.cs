using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    [SerializeField]
    private Button SPButton;
    [SerializeField]
    private Button SPMButton;
    [SerializeField]
    private Button SystemButton;

    [SerializeField]
    private GameObject SPCom;
    [SerializeField]
    private GameObject SPMCom;
    [SerializeField]
    private GameObject SystemCom;
    [SerializeField]
    private GameObject SelectCom;

    // Start is called before the first frame update
    void Start()
    {
        SetupMenuUIEvent();
    }

    private void OnEnable()
    {
        SPButton.Select();
    }

    public void SetupMenuUIEvent()
    {
        SPButton.onClick.AddListener(() =>
        {
            SPCom.SetActive(true);
            SPMCom.SetActive(false);
            SystemCom.SetActive(false);
        });

        SPMButton.onClick.AddListener(() =>
        {
            SPMCom.SetActive(true);
            SPCom.SetActive(false);
            SystemCom.SetActive(false);
        });

        SystemButton.onClick.AddListener(() =>
        {
            SystemCom.SetActive(true);
            SPMCom.SetActive(false);
            SPCom.SetActive(false);
        });
    }
}
