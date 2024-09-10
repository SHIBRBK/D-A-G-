using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("LeftHandCollider")) // プレイヤー1の左手コライダーのタグをチェック
        {
            Debug.Log("プレイヤー2がプレイヤー1のパンチを受けました！");
        }
    }
}