using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneBasic : MonoBehaviour
{
    /// <summary>
    /// 予兆が発生してから消え始めるまでの時間
    /// </summary>
    [SerializeField]
    protected private float waitingInterval = 100;

    /// <summary>
    /// a値が減少する速度
    /// </summary>
    [SerializeField]
    protected float alfaSpeed = 0.01f;
    protected float alfa = 1;

    protected bool deletedDangerZone;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        deletedDangerZone = false;
        alfa = GetComponent<Renderer>().material.color.a;
        waitingInterval = SphereSkill.dangerTime;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    private void FixedUpdate()
    {
        DeleatingZone();
        if (deletedDangerZone)
        {
            //予兆の表示が終了したらタグを変更し1f後に削除される
            this.tag = "AttackingZone";
            Destroy(this.gameObject, 0.1f);
        }
    }

    protected virtual void DeleatingZone()
    {
        Debug.Log("Didn't write danger's function!");
    }
}
