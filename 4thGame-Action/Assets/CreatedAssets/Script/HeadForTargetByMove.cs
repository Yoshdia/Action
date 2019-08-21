using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadForTargetByMove : MonoBehaviour
{
    MoveByRigidbody moveComponent;

    private void Awake()
    {
        moveComponent = GetComponent<MoveByRigidbody>();
    }

    /// <summary>
    /// targetPosへ向かう関数
    /// </summary>
    /// <param name="targetPos">目的の座標</param>
    public void HeadForTarget(Vector3 targetPos)
    {
        Vector3 targetForword = transform.forward;
        targetForword.y = 0;
        moveComponent.Move(targetForword);
        //transform.position += targetForword * moveSpeed;
    }

    /// <summary>
    /// targetPosへ向かう関数
    /// </summary>
    /// <param name="targetPos">目的の座標</param>
    /// <param name="speed">移動速度</param>
    public void HeadForTarget(Vector3 targetPos,float speed)
    {
        Vector3 targetForword = transform.forward;
        targetForword.y = 0;
        moveComponent.Move(targetForword,speed);
        //transform.position += targetForword * moveSpeed;
    }
}
