using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ColAnimationEvent : MonoBehaviour
{

    public event Action<bool> OnAttackEnded;

    //�����蔻��
    //�E��A����
    [SerializeField]
    private CapsuleCollider rightArmCapsuleCol, leftArmCapsuleCol;
    //�������A�ӂ���͂�
    [SerializeField]
    private CapsuleCollider rightLegUpCapsuleCol, rightLegCapsuleCol;
    [SerializeField]
    private CapsuleCollider leftLegUpCapsuleCol, leftLegCapsuleCol;
    //���K
    [SerializeField]
    private BoxCollider rightHipCapsuleCol, leftHipCapsuleCol;
    //���K
    [SerializeField]
    private CapsuleCollider rightHasyoCol, leftHasyoCol;
    //���K
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
        // �R���C�_�[�������ɓo�^����
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
                Debug.LogError("�J�v�Z���R���C�_�[���ݒ肳��Ă��Ȃ��ӏ�������܂��B");
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        mirror = anim.GetBool("Mirror");
    }

    /// <summary>
    /// �R���C�_�[�̗L��
    /// </summary>
    /// <param name="colliderName">�R���C�_�[�̖��O</param>
    /// <param name="enable">�R���C�_�[�̗L��</param>
    private void EnableCollider(string colliderName, bool enable)
    {
        if (colliders.TryGetValue(colliderName, out Collider collider))
        {
            collider.enabled = enable;
        }
        else
        {
            Debug.LogError($"'{colliderName}' �̃R���C�_�[��������܂���B");
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
