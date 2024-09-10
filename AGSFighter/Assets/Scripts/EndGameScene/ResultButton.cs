using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReMatchButton()
    {
        GameObject name = GameObject.Find("SceneName");
        SceneName sceneName = name.GetComponent<SceneName>();
        SceneManager.LoadScene(sceneName.GetSceneName());
        RoundManager.Instance.SetSelectedStage(sceneName.GetSceneName());
        Destroy(name);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
