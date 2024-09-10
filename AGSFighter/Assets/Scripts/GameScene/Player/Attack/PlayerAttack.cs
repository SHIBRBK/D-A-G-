using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private CapsuleCollider leftArmCapsuleCol;
    [SerializeField] private CapsuleCollider rightArmCapsuleCol;
    public bool mirror;
    [SerializeField] private Animator anim;

    private string currentAttackType;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        mirror = anim.GetBool("Mirror");
    }

    void EnableAttack(string attackType)
    {
        currentAttackType = attackType;

        // コリジョンを有効にする
        if (mirror == false)
        {
            rightArmCapsuleCol.enabled = true;
        }
        else
        {
            leftArmCapsuleCol.enabled = true;
        }
    }

    void AttackFinish()
    {
        rightArmCapsuleCol.enabled = false;
        leftArmCapsuleCol.enabled = false;
    }

    // 現在の攻撃タイプを取得する
    public string GetCurrentAttackType()
    {
        return currentAttackType;
    }

    // アニメーションイベント用のメソッド
    public void SetWeakAttack()
    {
        EnableAttack("N_Weak");
    }

    public void SetMediumAttack()
    {
        EnableAttack("N_Mid");
    }

    public void SetStrongAttack1()
    {
        EnableAttack("N_Stro");
    }

    public void SetStrongAttack2()
    {
        EnableAttack("NA_Stro");
    }

    // 新しく追加された左腕の攻撃を有効にするメソッド
    public void LeftArmAttack()
    {
        // 左腕の攻撃タイプを設定（適切な攻撃タイプに置き換えてください）
        EnableAttack("N_Weak");
        Debug.Log("左腕の攻撃が有効になりました。");
    }
}
