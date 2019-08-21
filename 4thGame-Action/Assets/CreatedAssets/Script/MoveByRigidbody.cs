using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 受け取った方向に進むクラス
/// </summary>
public class MoveByRigidbody : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField]
    ///<summary>
    /// 移動速度
    ///</summary>
    float moveSpeed = 10;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// directionの方向に移動する関数
    /// </summary>
    /// <param name="direction">移動する向き</param>
    public void Move(Vector3 direction)
    {
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }  
    
    /// <summary>
    /// directionの方向に移動する関数
    /// 移動速度が動的に変化するオブジェクトが利用する
    /// </summary>
    /// <param name="direction">移動する向き</param>
    /// <param name="moveSpeed">移動する速度</param>
    public void Move(Vector3 direction,float speed)
    {
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }
}
