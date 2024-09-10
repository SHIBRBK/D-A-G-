using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] players; // プレイヤーのTransform
    public float minDistance = 5f; // カメラがプレイヤーに最も近づく距離
    public float moveSpeed = 10f; // カメラの移動速度
    public float maxDistance = 20f; // カメラがプレイヤーから最も遠ざかる距離
    public float distanceThreshold = 1f; // プレイヤー間の距離がこの閾値を超えたらカメラが動き出す

    private Vector3 defaultPosition; // カメラのデフォルトの位置

    void Start()
    {
        // カメラの初期位置を記憶
        defaultPosition = transform.position;
    }

    void LateUpdate()
    {
        // プレイヤー間の距離を計算
        float distance = Vector3.Distance(players[0].position, players[1].position);

        // プレイヤー間の距離が閾値を超えたらカメラを移動
        if (Mathf.Abs(distance - minDistance) > distanceThreshold)
        {
            // プレイヤーの中心位置を計算
            Vector3 centerPosition = (players[0].position + players[1].position) / 2f;

            // デフォルト位置からのオフセットを計算
            Vector3 offset = transform.position - defaultPosition;

            // プレイヤー間の距離に基づいてカメラの位置を設定
            float clampedDistance = Mathf.Clamp(distance, minDistance, maxDistance);
            Vector3 targetPosition = centerPosition - transform.forward * clampedDistance + offset;

            // カメラ位置をスムーズに移動
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
