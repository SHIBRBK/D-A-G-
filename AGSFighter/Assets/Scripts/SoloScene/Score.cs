using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Text score;
    [SerializeField]
    private Text highScore;
    [SerializeField]
    private Text timeScoreText;
    [SerializeField]
    private Text perfectScoreText;
    [SerializeField]
    private Text totalScoreText;
    [SerializeField]
    private Text scoreText;

    private int scoreIndex;
    private int scoreCurrentIndex;
    private int timeScore;
    private int carBreakScore;
    private int totalScore;
    private int finishScore;

    [SerializeField]
    AttackManager attackManager;
    [SerializeField]
    SoloModeCountDown countDown;
    [SerializeField]
    UIAnimation uiAnim;

    // Start is called before the first frame update
    void Start()
    {
        //スコアリセット
        //PlayerPrefs.SetInt("HighScore", 0);
        highScore.text = PlayerPrefs.GetInt("HighScore") + "";
    }

    // Update is called once per frame
    void Update()
    {
        if(countDown.GetTime() < 0)
        {
            uiAnim.SceneAnimFalse();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Attack")
        {
            Transform currentTransform = other.transform;
            ColAnimationEvent attackerEvent = null;
            while (currentTransform != null)
            {
                attackerEvent = currentTransform.GetComponent<ColAnimationEvent>();
                if (attackerEvent != null)
                {
                    break;
                }
                currentTransform = currentTransform.parent;
            }

            
        }
    }

    public void AddScore(ColAnimationEvent attacker)
    {
        if (attacker.anim != null)
        {
            foreach (string key in
                attackManager.attackInfo.Keys)
            {
                if (attacker.anim.GetBool(key))
                {
                    scoreIndex = attackManager.GetAttackDetails(key).damage;
                    scoreCurrentIndex = scoreCurrentIndex + scoreIndex;
                }
            }
            score.text = scoreCurrentIndex + "";
        }
    }
    public void AddProjectileScore(int damage)
    {
        scoreIndex = damage;
        scoreCurrentIndex = scoreCurrentIndex + scoreIndex;
        score.text = scoreCurrentIndex + "";
    }

    public void TotalScore(int carBreakScore)
    {
        timeScore = countDown.GetTime() * 1000;
        this.carBreakScore = carBreakScore;
        totalScore = timeScore + this.carBreakScore;
        timeScoreText.text = timeScore + "";
        perfectScoreText.text = carBreakScore + "";
        totalScoreText.text = totalScore + "";
        finishScore = totalScore + scoreCurrentIndex + scoreIndex;
        scoreText.text = finishScore + "";

        if (PlayerPrefs.GetInt("HighScore") < finishScore)
        {
            PlayerPrefs.SetInt("HighScore", finishScore);
        }
    }
}
