using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySystem : MonoBehaviour
{

    //角度を入れる変数
    [SerializeField]
    private float sunPos;

    //Skyboxを入れる変数、[]があると複数入れられるように
    [SerializeField] private Material[] skybox;


    void Update()
    {
        //X軸を回転させる
        transform.eulerAngles = new Vector3(sunPos, 0, 0);

        //1日のスピードを調節する
        sunPos += Time.deltaTime * 20;

        //0度以上72度未満の時に一つ目のSkyboxを出す
        if (sunPos >= 0 && 72 > sunPos)
            RenderSettings.skybox = skybox[0];

        //elseがないと切り替わらない
        else if (sunPos >= 72 && 144 > sunPos)
            RenderSettings.skybox = skybox[1];

        else if (sunPos >= 144 && 216 > sunPos)
            RenderSettings.skybox = skybox[2];

        else if (sunPos >= 216 && 288 > sunPos)
            RenderSettings.skybox = skybox[3];

        else if (sunPos >= 288 && 360 > sunPos)
            RenderSettings.skybox = skybox[4];

        //360度以上になったら0に戻す
        if (sunPos > 360)
            sunPos = 0;
    }
}