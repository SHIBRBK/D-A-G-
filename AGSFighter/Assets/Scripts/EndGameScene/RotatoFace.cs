using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatoFace : MonoBehaviour
{
    public Transform neckBone;
    private GameObject watchTarget;
    private void Start()
    {
        watchTarget = GameObject.Find("Main Camera");
    }

    protected virtual void LateUpdate()
    {
        if (neckBone != null && watchTarget != null)
        {
            Vector3 targetPosition = watchTarget.transform.position;
            Vector3 selfPosition = transform.position;
            Vector3 direction = targetPosition - selfPosition;

            // Y軸の回転角度を計算
            float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // X軸の回転角度を計算（上下の視線移動）
            float angleX = Mathf.Atan2(direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;

            // ネックボーンの回転をリセット
            neckBone.rotation = Quaternion.identity;

            // Y軸とX軸の回転を適用
            neckBone.Rotate(-angleX, angleY, 0f);
        }
    }
}
