using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hadoken : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;
    float moveDistance;

    // 方向を示す変数
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private string hitPlayer;

    private void Start()
    {
    }

    // 初期化メソッド
    public void Initialize(Vector3 direction,string hitPlayer)
    {
        this.direction = direction; // 方向を正規化することで、速度を保持する
        this.hitPlayer = hitPlayer;
        Debug.LogWarning("当たるべき相手は" + this.hitPlayer);
    }

    private void FixedUpdate()
    {
        moveDistance = speed * Time.deltaTime;
        HadokenMove();
    }

    public void HadokenMove()
    {
        // 方向に応じて移動
        transform.Translate(direction * moveDistance);
    }

    private  void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Attack" || other.tag == "Car" || other.tag == "Edge" || other.tag == hitPlayer)
        {
            Debug.Log("からあげで火傷"+other.name);

            Destroy(this.gameObject);
        }
    }
}
