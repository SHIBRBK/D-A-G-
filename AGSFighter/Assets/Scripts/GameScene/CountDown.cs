using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountDown : MonoBehaviour
{
    [SerializeField]
    private bool isCount = false;
    [SerializeField]
    private float initialCountDownTime; // 初期カウントダウンタイム
    [SerializeField]
    private float countDownTime; // カウントダウンタイム
    [SerializeField]
    private Text textCountDown; // 表示用テキストUI
  
    // Start is called before the first frame update
    void Start()
    {
        ResetCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCount&& countDownTime > 0)
        {
            // カウントダウンタイムを整形して表示
            textCountDown.text = string.Format("{0:00}", countDownTime);
                // 経過時刻を引いていく
                countDownTime -= Time.deltaTime;
            if(!RoundManager.Instance.isRoundOver && countDownTime <= 0)
            {
                RoundManager.Instance.TimeOut();
                Debug.Log("Finish!!");
                ResetCountDown(); // カウントダウンタイマーをリセットする
            }
        }
    }


    public void ResetCountDown()
    {
        isCount = true; ;
        countDownTime = initialCountDownTime;
        textCountDown.text = string.Format("{0:00}", countDownTime);
    }

    public void StartStopTime(bool isCount)
    {
        this.isCount = isCount;
    }
}
