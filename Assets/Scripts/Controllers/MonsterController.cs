using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class MonsterController : CreatureController
{
    Coroutine _coSkill;
    [SerializeField] bool _rangedSkill = false;

    private void Start() {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        State = CreatureState.Idle;
        Dir = MoveDir.Down;
        _rangedSkill = Random.Range(0, 2) == 0 ? true : false;
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

    IEnumerator CoStartPunch(){
        //피격 판정
        GameObject go = Managers.Obj.FindCreature(GetFrontPos());
        if(go != null){
            CreatureController cc = go.GetComponent<CreatureController>();
            if(cc != null)
                cc.OnDamaged();
        }

        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coSkill = null;
    }

    IEnumerator CoStartShootArrow(){
        GameObject go = Managers.Resource.Instantiate("Creatures/Arrow");
        ArrowController ac = go.GetComponent<ArrowController>();

        ac.Dir = Dir;
        ac.CellPos = CellPos;

        yield return new WaitForSeconds(0.3f);
        State = CreatureState.Moving;
        _coSkill = null;
    }

}
