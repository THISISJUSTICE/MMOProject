using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class MonsterController : CreatureController
{
    Coroutine _coSkill;

    private void Start() {
        Init();
    }

    protected override void Init()
    {
        base.Init();
    }

    void Update()
    {
        UpdateController();       
    }

    protected override void UpdateIdle()
    {
        base.UpdateIdle();
    }

    public override void OnDamaged(){
        // Managers.Obj.Remove(id);
        // Managers.Resource.Destroy(gameObject); 
    }

    public override void UseSkill(int skillID)
    {
        if(skillID == 1){
            State = CreatureState.Skill;
        }
    }

}
