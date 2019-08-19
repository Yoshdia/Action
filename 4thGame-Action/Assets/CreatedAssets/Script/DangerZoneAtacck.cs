using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneAtacck : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        DeleatingZone();
        if (deletedDangerZone)
        {
            //予兆の表示が終了したらタグを変更し1f後に削除される
            this.tag = "AttackingZone";
            Destroy(this.gameObject,1f);
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
