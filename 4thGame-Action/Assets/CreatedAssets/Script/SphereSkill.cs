﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSkill : EnemySkillBasic
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
        Vector3 pos = parent.position;
        pos.y += 0.1f;
        GameObject skill = Instantiate(skillPrehub, pos, new Quaternion());
    }
}