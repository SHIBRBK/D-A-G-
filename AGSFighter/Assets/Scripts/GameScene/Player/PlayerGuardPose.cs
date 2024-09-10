using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGuardPose : MonoBehaviour
{
    [SerializeField]
    private ColAnimationEvent enemyColAnim;
    public bool isGuard = false;
    private float guard = 0.0f;
    private void Update()
    {
        if(isGuard)
        {
            guard++;
        }
        if(guard >= 50.0f)
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

            if (attackerEvent != null && attackerEvent == enemyColAnim)
            {
                isGuard = true;
            }
        }
        else if (other.tag == "Projectile")
        {
            IsGuard = false;
        }
    }

    public bool IsGuard
    {
        get { return isGuard; }
        set { isGuard = value; }
    }
}
