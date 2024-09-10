using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckInversion : MonoBehaviour
{
    private PlayerAction player;
    //接地判定
    //Rayの長さ
    [SerializeField]
    private float rayLength = 0.1f;

    //Rayをどれくらい体にめり込ませるか
    [SerializeField]
    private float rayOffset;

    //Rayの判定に用いるLayer
    [SerializeField]
    private LayerMask layerMask = default;
    [SerializeField]
    private bool isInversion = false;
    Vector3 vec = Vector3.right;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        //レイを逆方向に飛ばす
        if(!isInversion)
        {
            vec *= -1;
        }
    }

    private void FixedUpdate()
    {
        //ミラー用のオブジェクトに着いているか
        isInversion = CheckInversion();
    }

    public bool GetIsInversion()
    {
        return isInversion;
    }

    private bool CheckInversion()
    {
        if(player.GetIsGround())
        {
            //放つ光線の初期位置と姿勢
            //若干身体にめり込ませた一から発射しないと正しく判定できない時がある
            ray = new Ray(origin: transform.position + Vector3.up * rayOffset, direction: vec);
        }
        //Raycastがhitするかどうかで判定
        //レイヤの指定を忘れずに
        return Physics.Raycast(ray, rayLength, layerMask);
    }

    private void OnDrawGizmos()
    {
        //基本的にレイが当たっている状態なので確認しなくてもいい
        Gizmos.color = isInversion ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, vec);
    }
}
