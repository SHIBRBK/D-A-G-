using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float impactEffectDuration = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            // プレイヤーにダメージを与える処理
            PlayerHit playerHit = other.GetComponent<PlayerHit>();
            if (playerHit != null)
            {
                playerHit.TakeDamageProjectile((damageAmount, "火炎魔法1(波動拳)", AttackLevel.Mid, AttackType.Middle));
            }

            // エフェクトを生成して一定時間後に消す
            Vector3 position = other.ClosestPoint(transform.position);
            Instantiate(impactEffect, position, Quaternion.identity);
            Destroy(impactEffect, impactEffectDuration);

            // 飛び道具を消す処理
            Destroy(gameObject);
        }
    }
}
