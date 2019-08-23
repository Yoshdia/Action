using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    //入力情報が格納される変数
  float inputJump;
  float inputHorizontal = 0;
  float inputVertical = 0;
  bool inputAttack = false;
    float inputFire = 0;
    float inputHeal = 0;
    float inputStrong = 0;
    

    //カメラから見てどの方向に移動するかのベクトル
    [HideInInspector] private Vector3 moveInput;

    void Inputs()
    {
        inputJump = Input.GetAxis("Jump");
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputFire = Input.GetAxis("Fire");
        inputHeal = Input.GetAxis("Heal");
        inputAttack = Input.GetMouseButtonDown(0)||Input.GetKey(KeyCode.Alpha0);
        inputStrong = Input.GetAxis("Strong");
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        moveInput = CameraRelativeInput(inputHorizontal, inputVertical);
    }

    /// <summary>
    /// カメラの向きに基づいて動かす
    /// </summary>
    /// <param name="inputX">平行方向への入力情報</param>
    /// <param name="inputZ">水平方向への入力情報</param>
    /// <returns>移動方向</returns>
    Vector3 CameraRelativeInput(float inputX, float inputZ)
    {
        //x,z方向にのみに沿ったカメラに対するベクトル  
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        //正規化
        forward = forward.normalized;
        //カメラに対する右ベクトル
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 relativeVelocity = inputHorizontal * right + inputVertical * forward;
        //全ての方向への正規化を行う
        if (relativeVelocity.magnitude > 1)
        {
            relativeVelocity.Normalize();
        }
        return relativeVelocity;
    }

    public Vector3 GetMovement()
    {
        return moveInput;
    }

    /// <summary>
    /// 移動関連のボタンを入力しているか
    /// </summary>
    public bool HasMoveInput()
    {
        if (moveInput != Vector3.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float HasJumpInput()
    {
        return inputJump;
    }

    public bool HasAttackInput()
    {
        return inputAttack;
    }

    public float HasFireInput()
    {
        return inputFire;
    }
    public float HasHealInput()
    {
        return inputHeal;
    }
    public float HasStrongInput()
    {
        return inputStrong;
    }
}
