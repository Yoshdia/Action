using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{   
    //追従するオブジェクト
    public GameObject player;
    //追従させるカメラオブジェクト
    public GameObject mainCamera;
    //カメラを動かすキー(右
    const int RotateBotton = 1;
    //回転角度の最小最大値
    const float AngleLimitMax = 60f;
    const float AngleLimitMin = -60f;
    //回転速度
    [SerializeField]
    private float rotate_speed = 1.0f;

    void Awake()
    {
        mainCamera = Camera.main.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //ターゲットの座標を追従
        transform.position = player.transform.position;

        //移動用キーが押されていたら
        if (Input.GetMouseButton(RotateBotton))
        {
            rotateCameraAngle();
        }

        float angle_x = 180f <= transform.eulerAngles.x ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
        transform.eulerAngles = new Vector3(
            Mathf.Clamp(angle_x, AngleLimitMin, AngleLimitMax),
            transform.eulerAngles.y,
            transform.eulerAngles.z
        );
    }
    private void rotateCameraAngle()
    {
        Vector3 angle = new Vector3(
            Input.GetAxis("Mouse X") * rotate_speed,
            Input.GetAxis("Mouse Y") * -rotate_speed,
            0
        );

        transform.eulerAngles += new Vector3(angle.y, angle.x);
    }
    
}