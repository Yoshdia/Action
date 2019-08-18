using UnityEngine;
using System.Collections;

namespace RPGCharacterAnims{
	
	public class RPGCharacterInputControllerFREE : MonoBehaviour{

		//入力情報が格納される変数
		[HideInInspector] public bool inputJump;
		[HideInInspector] public bool inputLightHit;
		[HideInInspector] public bool inputDeath;
		[HideInInspector] public bool inputAttackL;
		[HideInInspector] public bool inputAttackR;
		[HideInInspector] public bool inputSwitchUpDown;
		[HideInInspector] public bool inputStrafe;
		[HideInInspector] public float inputAimVertical = 0;
		[HideInInspector] public float inputAimHorizontal = 0;
		[HideInInspector] public float inputHorizontal = 0;
		[HideInInspector] public float inputVertical = 0;
		[HideInInspector] public bool inputRoll;

		//変数

		[HideInInspector] public bool allowedInput = true;
        //カメラから見てどの方向に移動するかのベクトル
		[HideInInspector] public Vector3 moveInput;
        //入力状態0,1のみを格納
		[HideInInspector] public Vector2 aimInput;

        ///<param name="Int1">引数１</param>
        ///<returns></returns>
        
        /// <summary>
        ///Input情報の取得
        /// </summary>    
        void Inputs(){
			inputJump = Input.GetButtonDown("Jump");
			//inputLightHit = Input.GetButtonDown("LightHit");
			//inputDeath = Input.GetButtonDown("Death");
			//inputAttackL = Input.GetButtonDown("AttackL");
			//inputAttackR = Input.GetButtonDown("AttackR");
			//inputSwitchUpDown = Input.GetButtonDown("SwitchUpDown");
			inputStrafe = Input.GetKey(KeyCode.LeftShift);
			//inputAimVertical = Input.GetAxisRaw("AimVertical");
			//inputAimHorizontal = Input.GetAxisRaw("AimHorizontal");
			inputHorizontal = Input.GetAxisRaw("Horizontal");
			inputVertical = Input.GetAxisRaw("Vertical");
			//inputRoll = Input.GetButtonDown("L3");
		}

		void Awake(){
			allowedInput = true;
		}

		void Update(){
			Inputs();
			moveInput = CameraRelativeInput(inputHorizontal, inputVertical);
			aimInput = new Vector2(inputAimHorizontal, inputAimVertical);
		}

        /// <summary>
        /// カメラの向きに基づいて動かす
        /// </summary>
        /// <param name="inputX">平行方向への入力情報</param>
        /// <param name="inputZ">水平方向への入力情報</param>
        /// <returns>移動方向</returns>
		Vector3 CameraRelativeInput(float inputX, float inputZ){
			//x,z方向にのみに沿ったカメラに対するベクトル  
			Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
			forward.y = 0;
            //正規化
			forward = forward.normalized;
			//カメラに対する右ベクトル
			Vector3 right = new Vector3(forward.z, 0, -forward.x);
			Vector3 relativeVelocity = inputHorizontal * right + inputVertical * forward;
			//全ての方向への正規化を行う
			if(relativeVelocity.magnitude > 1){
				relativeVelocity.Normalize();
			}
			return relativeVelocity;
		}

        /// <summary>
        /// 何らかのボタンを入力しているか
        /// </summary>
		public bool HasAnyInput(){
			if(allowedInput && moveInput != Vector3.zero && aimInput != Vector2.zero && inputJump != false){
				return true;
			}
			else{
				return false;
			}
		}
		
        /// <summary>
        /// 移動関連のボタンを入力しているか
        /// </summary>
		public bool HasMoveInput(){
			if(allowedInput && moveInput != Vector3.zero){
				return true;
			}
			else{
				return false;
			}
		}
		
        /// <summary>
        /// 
        /// </summary>
		public bool HasAimInput(){
			if(allowedInput && (aimInput.x < -0.8f || aimInput.x > 0.8f) || (aimInput.y < -0.8f || aimInput.y > 0.8f)){
				return true;
			}
			else{
				return false;
			}
		}
	}
}