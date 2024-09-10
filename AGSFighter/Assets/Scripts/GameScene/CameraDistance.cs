using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistance : MonoBehaviour
{
    public Transform player1; // プレイヤー1のTransform
    public Transform player2; // プレイヤー2のTransform
    public float minDistance = 5f; // カメラがプレイヤーに最も近づく距離
    public float maxDistance = 20f; // カメラがプレイヤーから最も遠ざかる距離
    public float minZOffset = -10f; // デフォルトのZ位置からの最小オフセット
    public float maxZOffset = -30f; // デフォルトのZ位置からの最大オフセット
    public float moveSpeed = 10f; // カメラの移動速度
    public float distanceThreshold = 1f; // プレイヤー間の距離がこの閾値を超えたらカメラが動き出す
    private float defaultZ = -60f; // カメラのデフォルトのZ座標
    private Vector3 previousTargetPosition;

    public float minFOV = 60f; // プレイヤーが近づいたときの最小FOV
    public float maxFOV = 70f; // プレイヤーが離れたときの最大FOV

    void Start()
    {
        // カメラの初期位置を設定
        previousTargetPosition = new Vector3(transform.position.x, transform.position.y, defaultZ);
    }

    void LateUpdate()
    {
        // プレイヤー間の距離を計算
        float distance = Vector3.Distance(player1.position, player2.position);

        // プレイヤーの中心位置を計算
        Vector3 centerPosition = (player1.position + player2.position) / 2f;

        // プレイヤー間の距離が閾値を超えたらカメラを移動
        if (Mathf.Abs(distance - minDistance) > distanceThreshold)
        {
            // 距離に基づいてカメラのZ座標オフセットを計算
            float clampedDistance = Mathf.Clamp(distance, minDistance, maxDistance);
            float targetZOffset = Mathf.Lerp(minZOffset, maxZOffset, (clampedDistance - minDistance) / (maxDistance - minDistance));

            // デフォルトのZ座標にオフセットを加えたターゲットZ座標を計算
            float targetZ = defaultZ + targetZOffset;

            // ターゲット位置を計算（中心位置とターゲットZ座標を使用）
            Vector3 targetPosition = new Vector3(centerPosition.x, transform.position.y, targetZ);

            // カメラ位置をスムーズに移動
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // FOVを距離に応じて調整（縦方向の変化を最小限に）
            float targetFOV = Mathf.Lerp(minFOV, maxFOV, (clampedDistance - minDistance) / (maxDistance - minDistance));
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, targetFOV, moveSpeed * Time.deltaTime);

            // 前回のターゲット位置を更新
            previousTargetPosition = targetPosition;
        }
    }
}