using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGuard : MonoBehaviour
{
    [SerializeField]
    private ColAnimationEvent enemyColAnim;
    [SerializeField]
    private ColAnimationEvent colAnimationEvent;
    public bool isGuard = false;
    private float guard = 0.0f;
    [SerializeField]
    private GameObject guardEffect;
    private Collider col;
    int n = 1;

    private void Update()
    {
        if (isGuard)
        {
            guard++;
        }
        if (guard >= 50.0f)
        {
            guard = 0.0f;
            isGuard = false;
        }

        if (SceneManager.GetActiveScene().name != "SoloGameScene")
        {
            enemyColAnim.OnAttackEnded += get_flag =>
            {
                if (get_flag)
                {

                }
                else
                {
                    isGuard = false;
                }
            };
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack" || other.tag == "Projectile")
        {
            col = other;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Attack")
        {
            // ��ʊK�w��k����ColAnimationEvent��T��
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

            if (attackerEvent != null)
            {
                // �������g�̍U���Ȃ疳������
                if (attackerEvent == colAnimationEvent)
                {
                    Debug.Log("�������g�̍U���Ȃ̂Ŗ������܂��B");
                    return;
                }
            }

         if (attackerEvent != null && attackerEvent == enemyColAnim)
        {
            isGuard = true;
        }
        }
        else if(other.tag == "Projectile")
        {
            isGuard = true;
        }
    }


    public void InstantiateGuardEffect()
    {
        if(n == 1)
        {
            // �Փ˂����ꏊ�ɃG�t�F�N�g�𐶐�
            Vector3 position = col.ClosestPoint(transform.position); // �Փ˂����ꏊ�̋߂��̓_���擾
            Instantiate(guardEffect, position, Quaternion.identity);
            n = 0;
            StartCoroutine(NUM());
        }
    }

    IEnumerator NUM()
    {
        yield return new WaitForSeconds(0.1f);
        n = 1;
    }

    public bool IsGuard
    {
        get { return isGuard; }
        set { isGuard = value; }
    }
}
