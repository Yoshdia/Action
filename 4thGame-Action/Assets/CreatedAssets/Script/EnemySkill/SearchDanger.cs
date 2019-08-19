using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchDanger : EnemySkillBasic
{
    /// <summary>
    /// スキルの予兆が表示される時間 終了と同時に攻撃判定が生じる
    /// </summary>
    public const float dangerTime = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetSkillPrehub(Transform parent)
    {
        GameObject skill = Instantiate(skillPrehub, parent.position, new Quaternion());
    }
}
