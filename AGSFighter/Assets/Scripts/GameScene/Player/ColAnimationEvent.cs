using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ColAnimationEvent : MonoBehaviour
{

    public event Action<bool> OnAttackEnded;

    //当たり判定
    //右手、左手
    [SerializeField]
    private CapsuleCollider rightArmCapsuleCol, leftArmCapsuleCol;
    //太もも、ふくらはぎ
    [SerializeField]
    private CapsuleCollider rightLegUpCapsuleCol, rightLegCapsuleCol;
    [SerializeField]
    private CapsuleCollider leftLegUpCapsuleCol, leftLegCapsuleCol;
    //お尻
    [SerializeField]
    private BoxCollider rightHipCapsuleCol, leftHipCapsuleCol;
    //お尻
    [SerializeField]
    private CapsuleCollider rightHasyoCol, leftHasyoCol;
    //お尻
    [SerializeField]
    private CapsuleCollider rightSyoryuCol, leftSyoryuCol;

    private Dictionary<string, Collider> colliders;

    [SerializeField]
    private bool mirror;

    [NonSerialized]
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        // コライダーを辞書に登録する
        colliders = new Dictionary<string, Collider>()
        {
            { "RightArm", rightArmCapsuleCol },
            { "LeftArm", leftArmCapsuleCol },
            { "RightLegUp", rightLegUpCapsuleCol },
            { "RightLeg", rightLegCapsuleCol },
            { "LeftLegUp", leftLegUpCapsuleCol },
            { "LeftLeg", leftLegCapsuleCol },
            { "RightHip", rightHipCapsuleCol },
            { "LeftHip", leftHipCapsuleCol },
            { "RightHasyogeki", rightHasyoCol },
            { "LeftHasyogeki", leftHasyoCol },
            { "RightSyoryu", rightSyoryuCol },
            { "LeftSyoryu", leftSyoryuCol },
        };

        foreach (var collider in colliders.Values)
        {
            if (collider == null)
            {
                Debug.LogError("カプセルコライダーが設定されていない箇所があります。");
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        mirror = anim.GetBool("Mirror");
    }

    /// <summary>
    /// コライダーの有無
    /// </summary>
    /// <param name="colliderName">コライダーの名前</param>
    /// <param name="enable">コライダーの有無</param>
    private void EnableCollider(string colliderName, bool enable)
    {
        if (colliders.TryGetValue(colliderName, out Collider collider))
        {
            collider.enabled = enable;
        }
        else
        {
            Debug.LogError($"'{colliderName}' のコライダーが見つかりません。");
        }
    }

    void RightArmAttack()
    {
        EnableCollider(mirror ? "LeftArm" : "RightArm", true);
    }

    void LeftArmAttack()
    {
        EnableCollider(mirror ? "RightArm" : "LeftArm", true);
    }

    void RightLegUpAttack()
    {
        EnableCollider(mirror ? "LeftLegUp" : "RightLegUp", true);
    }

    void RightLegAttack()
    {
        EnableCollider(mirror ? "LeftLeg" : "RightLeg", true);
    }
    
    void LeftLegUpAttack()
    {
        EnableCollider(mirror ? "RightLegUp" : "LeftLegUp", true);
    }

    void LeftLegAttack()
    {
        EnableCollider(mirror ? "RightLeg" : "LeftLeg", true);
    }

    void LeftHipAttack()
    {
        EnableCollider(mirror ? "RightHip" : "LeftHip", true);
    }

    void Hasyogeki()
    {
        EnableCollider(mirror ? "LeftHasyogeki" : "RightHasyogeki", true);
    }

    void SyoryuAttack()
    {
        EnableCollider(mirror ? "LeftSyoryu" : "RightSyoryu", true);
    }

    void AttackFinish()
    {
        foreach (var collider in colliders.Values)
        {
            collider.enabled = false;
        }
        OnAttackEnded?.Invoke(false);
    }

    public void Finish()
    {
        AttackFinish();
    }
    
}
