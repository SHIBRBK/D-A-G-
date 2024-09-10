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
            // ãˆÊŠK‘w‚ğ‘k‚Á‚ÄColAnimationEvent‚ğ’T‚·
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
                // ©•ª©g‚ÌUŒ‚‚È‚ç–³‹‚·‚é
                if (attackerEvent == colAnimationEvent)
                {
                    Debug.Log("©•ª©g‚ÌUŒ‚‚È‚Ì‚Å–³‹‚µ‚Ü‚·B");
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
            // Õ“Ë‚µ‚½êŠ‚ÉƒGƒtƒFƒNƒg‚ğ¶¬
            Vector3 position = col.ClosestPoint(transform.position); // Õ“Ë‚µ‚½êŠ‚Ì‹ß‚­‚Ì“_‚ğæ“¾
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
