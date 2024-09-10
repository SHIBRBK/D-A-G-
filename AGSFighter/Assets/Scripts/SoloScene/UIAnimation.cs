using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField]
    PlayerAction player;

    [SerializeField]
    GameObject ready;
    [SerializeField]
    GameObject go;
    [SerializeField]
    GameObject ui;
    [SerializeField]
    GameObject finishOption;
    [SerializeField]
    FInishOption finishPanel;
    [SerializeField]
    private AudioSource readyAudioSource;
    [SerializeField]
    private AudioClip readyAudio;

    Animator anim;
    SoloModeCountDown countDown;

    private void Start()
    {
        countDown = GetComponent<SoloModeCountDown>();
        anim = GetComponent<Animator>();
        countDown.StartStopTime(true);
        anim.SetTrigger("Ready");
        player.State = PlayerAction.MyState.Freeze;
    }

    // Start is called before the first frame update
    void ScoreTrue()
    {
        anim.SetTrigger("ScoreTrue");
        ui.SetActive(false);
    }

    public void SceneAnimFalse()
    {
        anim.SetTrigger("GameFinish");
    }

    void ReadyAudio()
    {
        readyAudioSource.PlayOneShot(readyAudio);
    }

    void GOTextStart()
    {
        ready.SetActive(false);
        go.SetActive(true);
        anim.SetTrigger("GO");
        SoundManager.Instance.PlayUIClip("男衆「始めいッ！」");
    }

    void GOFalse()
    {
        go.SetActive(false);
        countDown.StartStopTime(false);
        player.State = PlayerAction.MyState.Game;
    }

    void FinishOptionTrue()
    {
        finishOption.SetActive(true);
        finishPanel.ButtonSelect();
    }
}
