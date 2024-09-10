using UnityEngine;
using Cinemachine;

public class DynamicCameraController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineTargetGroup targetGroup;

    public float minDistance = 5f; // カメラがプレイヤーに最も近づく距離
    public float maxDistance = 20f; // カメラがプレイヤーから最も遠ざかる距離
    public float minFOV = 40f; // 最小FOV
    public float maxFOV = 60f; // 最大FOV
    public float moveSpeed = 10f; // カメラの移動速度

    private void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        if (targetGroup == null)
        {
            targetGroup = GetComponent<CinemachineTargetGroup>();
        }

        // プレイヤーをターゲットグループに追加
        targetGroup.AddMember(player1, 1f, 0f);
        targetGroup.AddMember(player2, 1f, 0f);
    }

    private void LateUpdate()
    {
        // プレイヤー間の距離を計算
        float distance = Vector3.Distance(player1.position, player2.position);

        // 距離に基づいてカメラのFOVを調整
        float clampedDistance = Mathf.Clamp(distance, minDistance, maxDistance);
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, (clampedDistance - minDistance) / (maxDistance - minDistance));
        virtualCamera.m_Lens.FieldOfView = targetFOV;

        // ターゲットグループの中心位置を計算
        Vector3 centerPosition = (player1.position + player2.position) / 2f;
        centerPosition.z = virtualCamera.transform.position.z; // Z軸は固定

        // カメラ位置をスムーズに移動
        virtualCamera.transform.position = Vector3.MoveTowards(virtualCamera.transform.position, centerPosition, moveSpeed * Time.deltaTime);
    }
}
