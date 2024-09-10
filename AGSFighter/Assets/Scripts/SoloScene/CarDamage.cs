using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour
{

    [SerializeField]
    PlayerAction player;
    //破壊の閾値
    private int destructionThreshold = 1000;
    // 蓄積されたダメージ
    private int accumulatedDamage = 0;
    private int damage = 0;

    //飛んでいく力
    [SerializeField]
    private float flyingForce = 0.0f;

    //飛んでいく車のパーツ
    [SerializeField]
    private GameObject[] carParts;
    private int partsIndex = 0;

    [SerializeField]
    AttackManager attackManager;

    [SerializeField]
    SoloModeCountDown countDown;
    Score score;
    private int carBreakScore = 0;

    //UIのアニメーション
    [SerializeField]
    UIAnimation uiAnim;

    //SE、BGM
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip se1;
    [SerializeField]
    AudioClip se2;

    void Start()
    {
        score = GetComponent<Score>();
    }

    private void Update()
    {
        
    }

    //ダメージを与える
    public void ApplyDamage(ColAnimationEvent attacker)
    {
        //攻撃してきた相手のアニメーションを取得
        if (attacker.anim != null)
        {
            foreach (string key in
                attackManager.attackInfo.Keys)
            {
                if (attacker.anim.GetBool(key))
                {
                    damage = attackManager.GetAttackDetails(key).damage;
                }
            }
        }

        //車殴り
        audioSource.PlayOneShot(se1);

        //ダメージ蓄積
        accumulatedDamage += damage;

        //一定量超えたらリセット
        if (accumulatedDamage >= destructionThreshold)
        {
            ShatterScatterPart();
            accumulatedDamage = accumulatedDamage - destructionThreshold;
            Debug.Log(accumulatedDamage);
        }
    }

    public void ApplyProjectileDamage(int damage)
    {
        //車殴り
        audioSource.PlayOneShot(se1);

        //ダメージ蓄積
        accumulatedDamage += damage;

        //一定量超えたらリセット
        if (accumulatedDamage >= destructionThreshold)
        {
            ShatterScatterPart();
            accumulatedDamage = accumulatedDamage - destructionThreshold;
            Debug.Log(accumulatedDamage);
        }
    }

    //車のパーツを飛ばす
    void ShatterScatterPart()
    {
        //車破壊
        audioSource.PlayOneShot(se2);

        if(carParts[partsIndex] == null)
        {
            Debug.Log("もうパーツはありません");
            carBreakScore = 30000;
            player.State = PlayerAction.MyState.Freeze;
            countDown.StartStopTime(true);
            score.TotalScore(carBreakScore);
            uiAnim.SceneAnimFalse();
            return;
        }
        if (carParts.Length > 0)
        {
            GameObject randomPart = carParts[partsIndex];

            Rigidbody partRb = randomPart.GetComponent<Rigidbody>();
            if (partRb != null)
            {
                partRb.useGravity = true;
                partRb.isKinematic = false;
                partRb.AddForce(Random.onUnitSphere * flyingForce, ForceMode.Impulse); // ランダムな方向に力を加える
            }
            carBreakScore = 0;
            partsIndex++;
        }
    }

    public int GetCarScore()
    {
        return carBreakScore;
    }
}
