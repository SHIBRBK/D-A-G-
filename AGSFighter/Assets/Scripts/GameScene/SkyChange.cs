using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyChange : MonoBehaviour
{
    [SerializeField] private Material[] sky; // スカイボックスのマテリアル配列
    [SerializeField] private Light directionalLight; // 照らすライト
    [SerializeField] private Vector3[] lightRotations; // ライトの方向
    private int num;

    void Start()
    {
        // 保存されているカウントを取得
        num = PlayerPrefs.GetInt("DayNightCycle", 0);

        // シーンロード時に昼夜を設定する
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetSkyboxAndLight();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SelectStage2")
        {
            SetSkyboxAndLight();
        }
    }

    void SetSkyboxAndLight()
    {
        // カウントをインクリメントし、スカイボックスとライトの設定
        num = (num + 1) % sky.Length;
        PlayerPrefs.SetInt("DayNightCycle", num);
       
        RenderSettings.skybox = sky[num];

        directionalLight.transform.rotation = Quaternion.Euler(lightRotations[num]);

        // ライトの回転角度と現在のスカイボックスの番号をログに出力
        Debug.Log("Current Skybox Index: " + num);
        Debug.Log("Current Light Rotation: " + directionalLight.transform.rotation.eulerAngles);

        // PlayerPrefsの変更を保存
        PlayerPrefs.Save();
    }
}
