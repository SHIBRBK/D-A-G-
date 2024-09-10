using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneName : MonoBehaviour
{
    [SerializeField]
    private string name;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        name = SceneManager.GetActiveScene().name;
    }

    public string GetSceneName()
    {
        return name;
    }
}
