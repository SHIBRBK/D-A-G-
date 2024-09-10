using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCountDown : MonoBehaviour
{
    [SerializeField]
    protected bool isCount = false;
    [SerializeField]
    protected float initialCountDownTime; // 初期カウントダウンタイム
    [SerializeField]
    protected float countDownTime; // カウントダウンタイム
    [SerializeField]
    protected Text textCountDown; // 表示用テキストUI
    [SerializeField]
    protected PlayerAction player;

    protected virtual void Start()
    {
        ResetCountDown();
    }

    protected virtual void Update()
    {
        if (!isCount && countDownTime > 0)
        {
            // カウントダウンタイムを整形して表示
            textCountDown.text = string.Format("{0:00}", countDownTime);
            // 経過時刻を引いていく
            countDownTime -= Time.deltaTime;

            if (countDownTime <= 0)
            {
                OnCountDownEnd();
            }
        }
    }

    public virtual void ResetCountDown()
    {
        countDownTime = initialCountDownTime;
        textCountDown.text = string.Format("{0:00}", countDownTime);
    }

    public virtual void StartStopTime(bool isCount)
    {
        this.isCount = isCount;
    }

    protected abstract void OnCountDownEnd();
}
