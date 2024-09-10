using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class PlayerHit : MonoBehaviour
{
    private int test = 0;
    private AttackManager attackManager;

    [SerializeField]
    private PlayerAction player;
    //ヒットストップ変数
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private int frameCount;
    private bool isCountingFrames;

    [SerializeField]
    private PlayerInputSystem inputSystem;
    [SerializeField] public HPGauge hpGauge; // 相手プレイヤーのHPをアタッチする
    [SerializeField] private ColAnimationEvent colAnimationEvent; // 自分のColAnimationEventをアタッチする
    [SerializeField]
    private GameObject camEdge, camEdge2;
    Collider attackCol;
    [SerializeField] private GameObject initializePosition;

    //衝撃エフェクト
        [SerializeField]
    GameObject impactEffect;

    // ProjectileDamageDictionaryへの参照
    [SerializeField] private ProjectileDamageDictionary projectileDamageDictionary;
    (int, string, AttackLevel, AttackType) damages = (0, "", AttackLevel.NullLevel, AttackType.NullLevel); // 初期値としてダメージ、音声の名前、攻撃レベルのタプルを設定します

    public (int, string, AttackLevel,AttackType) Damage
    {
        get { return this.damages; }  // 取得用
        private set { this.damages = value; } // 値入力用
    }

    // クールダウン期間（秒）
    public float hitCooldown = 1.0f;
    public float hitDuration = 0.5f;
    private bool canBeHit = true;

    private bool hasHit = false; // すでに当たり判定が処理されたかどうかを管理するフラグ

    [SerializeField] private bool isDefeated = false; // プレイヤーが倒されたかどうかを示すフラグ



    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "TutorialScene"
            && SceneManager.GetActiveScene().name != "SoloGameScene")
        {
            //実体生成
            RoundManager.Instance.Create();

            attackManager = FindObjectOfType<AttackManager>(); // AttackManagerを見つける
            if (attackManager == null)
            {
                Debug.LogError("AttackManagerが見つかりませんでした。");
            }

            if (colAnimationEvent == null)
            {
                Debug.LogError("ColAnimationEventが見つかりませんでした。");
            }
        }
    }

    private void Update()
    {
        // フレームカウントが開始されている場合
        if (isCountingFrames)
        {
            // フレームカウントをインクリメント
            frameCount++;

            // 10フレームに到達したら
            if (frameCount >= 10)
            {
                // Animatorを再開
                playerAnimator.enabled = true;

                // フレームカウントをリセット
                frameCount = 0;

                // フレームカウントを停止
                isCountingFrames = false;
            }
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (isDefeated) return; // 追加: 倒された場合は当たり判定を無視

        //発生が同時すぎてガード出来ていてもダメージを食らってしまう
        // Attackタグのオブジェクトにのみ反応
        if (other.CompareTag("Attack") && canBeHit && !hasHit)
        {
            await UniTask.Delay(50);

            if (!player.playerInput.guardSFlag && !player.playerInput.guardCFlag)
            {
                NormalAttack(other);
                hasHit = true;

                // 1フレーム後にhasHitをfalseにする
                StartCoroutine(ResetHasHit());
            }
        }
        else if (other.CompareTag("Projectile") && canBeHit && !hasHit)
        {
            //遠距離攻撃
            if (!player.playerInput.guardSFlag && !player.playerInput.guardCFlag)
            {
                if(other!=null)
                {
                    ProjectileAttack(other);
                    
                    hasHit = true;
                    // 1フレーム後にhasHitをfalseにする
                    StartCoroutine(ResetHasHit());
                }
                else
                {
                    Debug.LogError("入ってねぇぞ殺すぞ");
                }
            }
        }
    }

    private IEnumerator ResetHasHit()
    {
        yield return null; // 1フレーム待機

        hasHit = false;
    }

    //Damageの情報取って、反応をさせる
    private bool AttackLevelFlag()
    {
        // 効果音・アニメーションを再生
        Dictionary<AttackLevel, (int, bool)> attackLevelToTestValue = new Dictionary<AttackLevel, (int, bool)>()
    {
        { AttackLevel.High, player.IsCrouching ? (21, false) : (11, true) },
        { AttackLevel.Mid, player.IsCrouching ? (22, true) : (12, true) },
        { AttackLevel.Low, player.IsCrouching ? (23, true) : (13, true) }
    };

        var (testValue, shouldApplyDamage) = attackLevelToTestValue[Damage.Item3];
        test = testValue;

        if ((Damage.Item3 == AttackLevel.Low || Damage.Item3 == AttackLevel.Mid || Damage.Item3 == AttackLevel.High))
        {
            player.animState.ResetDamageAnim();
            if(Damage.Item4 == AttackType.Strong)
            {
                ExecuteHit("SStrongHit");
            }
            else if(Damage.Item4 == AttackType.Middle)
            {
                ExecuteHit("SMidHit");
            }
            else if(Damage.Item4 == AttackType.Weak)
            {
                ExecuteHit("SWeakHit");
            }
            else if(Damage.Item4 == AttackType.Huttobi)
            {
                ExecuteHit("Huttobi");
            }
        }
        return shouldApplyDamage;
    }
    private void ExecuteHit(string attackName)
    {
        if (player.anim.GetBool(attackName))
        {
            player.animState.SetAnimFalse(attackName);
            StartCoroutine(DelayAnim(attackName));
            return;
        }

        player.animState.ResetDamageAnim();

        player.animState.SetAnimTrue(attackName);
        player.playerInput.AttackFlag = true;
    }
    IEnumerator DelayAnim(string attackName)
    {
        yield return new WaitForSeconds(0.001f);

        player.animState.SetAnimTrue(attackName);
        player.playerInput.AttackFlag = true;
    }

    private void NormalAttack(Collider other)
    {
        //Debug.Log("Attackタグのオブジェクトが検出されました: " + other.gameObject.name);

        // 上位階層を遡ってColAnimationEventを探す
        Transform currentTransform = other.transform;
        ColAnimationEvent attackerEvent = null;

        attackCol = other;
        while (currentTransform != null)
        {
            attackerEvent = currentTransform.GetComponent<ColAnimationEvent>();
            if (attackerEvent != null)
            {
                break;
            }
            currentTransform = currentTransform.parent;
        }

        if (attackerEvent != null)//どうもこうも波動拳だけここがnullになる
        {
            Debug.Log("ColAnimationEventコンポーネントが見つかりました。");
            // 自分自身の攻撃なら無視する
            if (attackerEvent == colAnimationEvent)
            {
                Debug.Log("自分自身の攻撃なので無視します。");
                return;
            }
            Debug.Log("攻撃がヒットしました。");
            // 攻撃者の情報を取得
            TakeDamage(attackerEvent);
        }
        else
        {
            //TakeDamage(colAnimationEvent);
            Debug.LogWarning("ColAnimationEventコンポーネントが見つかりませんでした。");
        }
    }

    private void ProjectileAttack(Collider other)
    {
        //Debug.Log("Projectileタグのオブジェクトが検出されました: " + other.gameObject.name);
        string projectileName = other.gameObject.name.Replace("(Clone)", "").Trim();
        Debug.LogWarning("投げものネーム"+ projectileName);
        attackCol = other;
        if (projectileDamageDictionary.projectileInfo.TryGetValue(
            projectileName, out ProjectileDamageDictionary.ProjectileInfo damage))
        {
            Damage = (damage.damage, damage.soundEffect,damage.attackLevel, damage.attackType);
            // 衝突した場所にエフェクトを生成
            Vector3 position = other.ClosestPoint(transform.position);
            Instantiate(impactEffect, position, Quaternion.identity);

            // ダメージ処理
            TakeDamageProjectile(Damage);

        }
        else
        {
            Debug.LogWarning("ProjectileDamageDictionaryに存在しない飛び道具です: " + other.gameObject.name+"\n");
        }
    }

    // ダメージを受ける
    public void TakeDamage(ColAnimationEvent attacker)
    {
        //Debug.Log("TakeDamageが呼ばれました。");

        if (attacker.anim != null)
        {
            foreach (string key in 
                attackManager.attackInfo.Keys)
            {
                if(attacker.anim.GetBool(key))
                { // AttackManagerからタプルを取得し、ProjectileDamageDictionary.ProjectileInfoに変換
                
                    Damage = attackManager.GetAttackDetails(key);
                    Debug.Log(Damage);
                    SoundManager.Instance.PlaySFX(Damage.Item2);
                }
            }
        }
        else
        {
            Debug.LogWarning("攻撃アニメーターが見つかりませんでした。");
            return;
        }

        //Damageの情報を取る
        if (!AttackLevelFlag()) return;

        // 衝突した場所にエフェクトを生成
        Vector3 position = attackCol.ClosestPoint(transform.position); // 衝突した場所の近くの点を取得
        Instantiate(impactEffect, position, Quaternion.identity);


        //HP減少処理
        if (hpGauge != null)
        {
            hpGauge.BeInjured(Damage.Item1);
        }
        else
        {
            Debug.LogWarning("opponentHpGaugeが見つかりませんでした。");
        }

        //ヒットストップ
        // Animatorを停止
        playerAnimator.enabled = false;

        // フレームカウントをリセット
        frameCount = 0;

        // フレームカウントを開始
        isCountingFrames = true;

        if (RoundManager.Instance.isRoundOver) return; // 追加
        PlayerDead();

        
        Debug.Log("相手のHPが" + Damage + "減少しました。");
    }

    public void TakeDamageProjectile((int,string,AttackLevel,AttackType) damage)
    {
        Debug.Log("TakeDamageProjectileが呼ばれました。");

        //projectileDamageDictionary[]
        Damage =  damage;

        if (hpGauge != null)
        {
            hpGauge.BeInjured(Damage.Item1);
            SoundManager.Instance.PlaySFX(Damage.Item2);
        }
        else
        {
            Debug.LogWarning("hpGaugeが見つかりませんでした。");
        }

        if (RoundManager.Instance.isRoundOver) return;

        PlayerDead();

        Debug.Log("相手のHPが" + Damage + "減少しました。");
    }



    private IEnumerator ResetDefeatedFlag()
    {
        yield return new WaitForSeconds(3f); // 3秒の猶予を置く
    }

    public void ResetPlayer()
    {
        ResetPlayerCoroutine();

    }

    private void PlayerDead()
    {
        if (hpGauge._currentHP <= 0 && !isDefeated)
        {
            Debug.LogWarning("死にました");
            isDefeated = true;
            hpGauge._currentHP = 0;
            RoundManager.Instance.NotifyPlayerDefeated(gameObject);
            StartCoroutine(ResetDefeatedFlag());
        }
    }

    private void ResetPlayerCoroutine()
    {
        ResetCamera(false);
        player.SetDefaultPos();

        if (hpGauge != null)
        {
            hpGauge.ResetHP();

            isDefeated = false; // フラグをリセット
        }
        // プレイヤーの位置やその他のステータスをリセットする処理を追加
        StartCoroutine(Cam());
    }

    public void ResetCamera(bool flag)
    {
        camEdge.SetActive(flag);
        camEdge2.SetActive(flag);
    }

    public IEnumerator Cam()
    {
        yield return new WaitForSeconds(0.5f);
        ResetCamera(true);
    }
}
