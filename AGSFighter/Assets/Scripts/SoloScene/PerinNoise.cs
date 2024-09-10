using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerinNoise : MonoBehaviour
{
    //振動
    private bool isVibrating = false;
    private float vibrationDuration = 0.3f;
    private float vibrationTimer = 0f;
    [SerializeField]
    PerlinNoiseShaker noiseShaker;
    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    Animator playerAnim;
    [SerializeField]
    private int frameCount;
    private bool isCountingFrames;

    [SerializeField]
    CarDamage cardamage;
    [SerializeField]
    Score score;
    // ProjectileDamageDictionaryへの参照
    [SerializeField] private ProjectileDamageDictionary projectileDamageDictionary;
    (int, string, AttackLevel) damages = (0, ""
         , AttackLevel.NullLevel); // 初期値としてダメージ、音声の名前、攻撃レベルのタプルを設定します
    public (int, string, AttackLevel) Damage
    {
        get { return this.damages; }  // 取得用
        private set { this.damages = value; } // 値入力用
    }
    private void Start()
    {
    }

    public void OnTriggerEnter(Collider col)
	{
        if (col.tag == "Attack")
        {
            Transform currentTransform = col.transform;
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

            isVibrating = true;
            vibrationTimer = 0f;
            Debug.Log("当たってる");
            // 衝突した場所にエフェクトを生成
            Vector3 position = col.ClosestPoint(transform.position); // 衝突した場所の近くの点を取得
            Instantiate(impactEffect, position, Quaternion.identity);

            // Animatorを停止
            playerAnim.enabled = false;

            // フレームカウントをリセット
            frameCount = 0;

            // フレームカウントを開始
            isCountingFrames = true;

            if (SceneManager.GetActiveScene().name == "SoloGameScene")
            {
                cardamage.ApplyDamage(attackerEvent);
                score.AddScore(attackerEvent);
            }
        }
        else if (col.CompareTag("Projectile"))
        {
            ProjectileAttack(col);
        }
    }

    private void ProjectileAttack(Collider other)
    {
        string projectileName = other.gameObject.name.Replace("(Clone)", "").Trim();

        if (projectileDamageDictionary.projectileInfo.TryGetValue(
            projectileName, out ProjectileDamageDictionary.ProjectileInfo damage))
        {

            Damage = (damage.damage, damage.soundEffect, damage.attackLevel);
            // 衝突した場所にエフェクトを生成
            Vector3 position = other.ClosestPoint(transform.position);
            Instantiate(impactEffect, position, Quaternion.identity);

            //振動処理
            isVibrating = true;
            vibrationTimer = 0f;

            // ダメージ処理
            TakeDamageProjectile(Damage);

            // 飛び道具を削除
            Destroy(other.gameObject);
        }
    }

    public void TakeDamageProjectile((int, string, AttackLevel) damage)
    {
        if (SceneManager.GetActiveScene().name == "SoloGameScene")
        {
            cardamage.ApplyProjectileDamage(damage.Item1);
            score.AddProjectileScore(damage.Item1);
        }
    }

    private void Update()
    {
        //振動させる
        if (isVibrating)
        {
            if(vibrationTimer < vibrationDuration)
            {
                vibrationTimer += Time.deltaTime;
                noiseShaker.StartNoise(vibrationTimer);
            }
            else
            {
                isVibrating = false;
            }
        }
        
        // フレームカウントが開始されている場合
        if (isCountingFrames)
        {
            // フレームカウントをインクリメント
            frameCount++;

            // 10フレームに到達したら
            if (frameCount >= 10)
            {
                // Animatorを再開
                playerAnim.enabled = true;

                // フレームカウントをリセット
                frameCount = 0;

                // フレームカウントを停止
                isCountingFrames = false;
            }
        }
    }
}
