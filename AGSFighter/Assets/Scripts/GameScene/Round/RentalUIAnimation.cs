using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RentalUIAnimation : MonoBehaviour
{

    [SerializeField]
    PlayerAction player1;
    [SerializeField]
    PlayerAction player2;
    private GameObject p1, p2;

    [SerializeField]
    GameObject round;
    [SerializeField]
    GameObject roundCount;
    [SerializeField]
    GameObject fight;
    [SerializeField]
    GameObject ui;
    [SerializeField]
    GameObject finishOption;

    public Animator anim;
    CountDown countDown;



    public void Start()
    {
        
        countDown = GetComponent<CountDown>();
        anim = GameObject.Find("RoundCount").GetComponent<Animator>();
        countDown.StartStopTime(true);
        GameObject.Find(
            "RoundCount").transform.Find(
            "ROUND").gameObject.SetActive(true);
        anim.SetTrigger("Ready");
        //PlayerAction
        //GameManager.Instance.transform.Find("yuji").GetComponent<PlayerAction>();
        p1 = GameObject.Find("yuji");
        player1 = p1.GetComponent<PlayerAction>();
        p2 = GameObject.Find("yuji2P");
        player2 = p2.GetComponent<PlayerAction>();
        player1.State = PlayerAction.MyState.Freeze;
        player2.State = PlayerAction.MyState.Freeze;
    }

    public void StartRound()
    {
        round.SetActive(true);
    }

    public void SceneAnimFalse()
    {
        anim.SetTrigger("GameFinish");
    }


    void GOTextStart()
    {
        roundCount.SetActive(true);
        anim.SetTrigger("Round");
    }

    void GoRound()
    {
        round.SetActive(false);
        roundCount.SetActive(false);
        fight.SetActive(true);
        anim.SetTrigger("GO");
        //SoundManager.Instance.PlayUIClip("試合開始のゴング");
        SoundManager.Instance.PlayUIClip("男衆「始めいッ！」");
    }

    void GOFalse()
    {
        fight.SetActive(false);
        countDown.StartStopTime(false);
        player1.State = PlayerAction.MyState.Game;
        player2.State = PlayerAction.MyState.Game;
    }

}
