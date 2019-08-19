using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    Animator animator;
    private Rigidbody rigidbody;

    InputController input;
    /// <summary>
    ///カメラに基づいた移動方向 inputから受け取る
    /// </summary>
    Vector3 inputMovement;

    [SerializeField]
    ///<summary>
    /// 移動速度
    ///</summary>
    float moveSpeed = 10;

    /// <summary>
    /// ジャンプ中かどうか
    /// </summary>
    bool isJumping = false;

    ///<summary>
    ///    ジャンプの強さ
    ///</summary>
    [SerializeField] private float jumpPower = 5f;

    ///<summary>
    ///    接地してから何フレーム経過したか
    ///    接地してない間は常にゼロとする
    ///</summary>
    private int isGround = 0;

    ///<summary>
    ///    接地してない間、何フレーム経過したか
    ///    接地している間は常にゼロとする
    ///</summary>
    private int notGround = 0;

    ///<summary>
    ///    このフレーム数分接地していたらor接地していなかったら
    ///    状態が変わったと認識する（ジャンプ開始したor着地した）
    ///    接地してからキャラの状態が安定するまでに数フレーム用するため、
    ///    キャラが安定する前に再ジャンプ入力を受け付けてしまうとバグる（ジャンプ出来なくなる）
    ///</summary>
    const int JumpingEndCount = 8;

    ///<summary>
    ///    プレイヤーと地面の間の距離
    ///    IsGround()が呼ばれるたびに更新される
    ///</summary>
    private float groundDistance = 0f;

    ///<summary>
    ///    _groundDistanceがこの値以下の場合接地していると判定する
    ///</summary>
    private const float GroundDistanceLimit = 0.5f;

    ///<summary>
    ///    判定元の原点が地面に極端に近いとrayがヒットしない場合があるので、
    ///    オフセットを設けて確実にヒットするようにする
    ///</summary>
    private Vector3 _raycastOffset = new Vector3(0f, 0.005f, 0f);

    ///<summary>
    ///    プレイヤーキャラから下向きに地面判定のrayを飛ばす時の上限距離
    ///</summary>
    private const float _raycastSearchDistance = 100f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<InputController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        isJumping = false;
    }

    void Update()
    {
        inputMovement = input.GetMovement();

        if (input.HasJumpInput() > 0&&!isJumping)
        {
            isJumping = true;
            animator.SetBool("Jump", true);
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    //接触中にtagが変わるオブジェクトがあるためOnTriggerStay
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AttackingZone")
        {
            Debug.Log("Outi!");
            //AttackingZone側でも発生してから1f後に削除されるような仕様になっているが、複数ヒットしてしまうことがあるためこちらからもDestroy関数を呼ぶ
            Destroy(other.gameObject);
        }
    }


    private void FixedUpdate()
    {
        Move();
        Turn();
        Jumping();
    }

    private void Move()
    {
        Vector3 movement = inputMovement * moveSpeed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }

    private void Turn()
    {
        if (input.HasMoveInput()&&inputMovement!=Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(inputMovement);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    private void Jumping()
    {
        if (!isJumping)
            return;
        CheckGroundDistance();
    }

    private void CheckGroundDistance()
    {
        var layerMask = LayerMask.GetMask("Ground");
        RaycastHit hit;

        // プレイヤーの位置から下向きにRaycastを飛ばし一番近いものがヒットする
        var isGroundHit = Physics.Raycast(
                transform.position + _raycastOffset,
                transform.TransformDirection(Vector3.down),
                out hit,
                _raycastSearchDistance,
                layerMask
            );

        if (isGroundHit)
        {
            groundDistance = hit.distance;
        }
        else
        {
            // ヒットしなかった場合はキャラの下方に地面が存在しないものとして扱う
            groundDistance = float.MaxValue;
        }

        // 地面とキャラの距離は環境によって様々で
        // 完全にゼロにはならない時もあるため、
        // ジャンプしていない時の値に多少のマージンをのせた
        // 一定値以下を接地と判定する
        // 通常あり得ないと思われるが、オーバーフローされると再度アクションが実行されてしまうので、越えたところで止める
        if (groundDistance < GroundDistanceLimit)
        {
            if (isGround <= JumpingEndCount)
            {
                isGround += 1;
                notGround = 0;
            }
            animator.SetBool("Jump", false);
        }
        else
        {
            if (notGround <= JumpingEndCount)
            {
                isGround = 0;
                notGround += 1;
            }
        }

        // 接地後またはジャンプ後、特定フレーム分状態の変化が無ければ、
        // 状態が安定したものとして接地処理またはジャンプ処理を行う
        if (JumpingEndCount == isGround && notGround == 0)
        {
            isJumping = false;
        }
        else
        if (JumpingEndCount == notGround && isGround == 0)
        {
            isJumping = false;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void AddDamage(float damage)
    {

    }

    public void FootR()
    {
    }

    public void FootL()
    {
    }
}
