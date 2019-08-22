using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    /// <summary>
    /// エフェクトの設置に必要な情報
    /// </summary>
    [System.Serializable]
    class DangerZoneEffect
    {
        public Vector3 plusPosition;
        public Vector3 rotate;
        public GameObject effect;
    }

    [SerializeField]
    DangerZoneEffect[] effects;

    bool effectSet;

    /// <summary>
    /// 予兆が発生してから消え始めるまでの時間
    /// </summary>
    [SerializeField]
    private float waitingInterval = 100;

    /// <summary>
    /// a値が減少する速度
    /// </summary>
    [SerializeField]
    float alfaSpeed = 0.01f;
    float alfa = 1;

    bool deletedDangerZone;

    // Start is called before the first frame update
    void Start()
    {
        deletedDangerZone = false;
        alfa = GetComponent<Renderer>().material.color.a;
        waitingInterval = 200;
        effectSet = false;
    }

    void FixedUpdate()
    {
        DeleatingZone();
        if (deletedDangerZone)
        {
            //予兆の表示が終了したらタグを変更し1f後に削除される
            this.tag = "AttackingZone";
            Destroy(this.gameObject, 0.1f);
        }
    }

    void DeleatingZone()
    {
        if (waitingInterval < 0)
        {
            alfa -= alfaSpeed;
            if (alfa < 0)
            {
                deletedDangerZone = true;
                if (!effectSet)
                {
                    effectSet = true;


                    foreach (DangerZoneEffect effect in effects)
                    {
                        Vector3 effectPos = transform.position + effect.plusPosition;
                        Quaternion effectQua = new Quaternion();
                        effectQua = Quaternion.Euler(effect.rotate.x, effect.rotate.y, effect.rotate.z);
                        Instantiate(effect.effect, effectPos, effectQua);
                    }
                }
                return;
            }
            GetComponent<Renderer>().material.color = new Color(255, 0, 0, alfa);
        }
        else
        {
            waitingInterval--;
        }
    }
}
