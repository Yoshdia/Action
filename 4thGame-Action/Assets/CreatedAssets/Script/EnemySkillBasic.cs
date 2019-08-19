using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillBasic : MonoBehaviour
{
    /// <summary>
    /// スキルプレハブ
    /// </summary>
    [SerializeField]
    public GameObject skillPrehub;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 予兆、後に攻撃が発生するプレハブを設置する
    /// </summary>
    /// <param name="parent"> 親オブジェクトのTransform情報</param>
    virtual public void SetSkillPrehub(Transform parent)
    {
        Debug.Log("Did'nt point a skill prehub!");
    }
}
