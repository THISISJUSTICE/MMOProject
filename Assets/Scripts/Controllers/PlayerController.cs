using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PlayerController : CreatureController
{
    protected Coroutine _coSkill;
    protected bool _rangeSkill = false;

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateAnimation(){
        if(State == CreatureState.Idle){
            switch(_lastDir){
                case MoveDir.Up:
                    _animator.Play("Idle_Back");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("Idle_Front");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("Idle_Right");
                    _sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("Idle_Right");
                    _sprite.flipX = false;
                    break;
            }
        }
        else if(State == CreatureState.Moving){
            switch(Dir){
                case MoveDir.Up:
                    _animator.Play("Walk_Back");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("Walk_Front");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("Walk_Right");
                    _sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("Walk_Right");
                    _sprite.flipX = false;
                    break;

            }
        }
        else if (State == CreatureState.Skill){
            switch(_lastDir){
                case MoveDir.Up:
                    _animator.Play(_rangeSkill ? "Attack_Weapon_Back" : "Attack_Back");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play(_rangeSkill ? "Attack_Weapon_Front" : "Attack_Front");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play(_rangeSkill ? "Attack_Weapon_Right" : "Attack_Right");
                    _sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play(_rangeSkill ? "Attack_Weapon_Right" : "Attack_Right");
                    _sprite.flipX = false;
                    break;

            }
        }
        else{

        }
    }

    protected override void UpdateController()
    {
        base.UpdateController();
    }

    protected override void UpdateIdle(){
        //이동 상태로 갈 지 확인
        if(Dir != MoveDir.None){
            State = CreatureState.Moving;
            return;
        }
    }

    public void UseSkill(int skillID)
    {
        if(skillID == 1){
            _coSkill = StartCoroutine(CoStartPunch());
        }
    }

    protected virtual void CheckUpdatedFlag(){

    }

    protected IEnumerator CoStartPunch(){
        //대기 시간
        _rangeSkill = false;
        State = CreatureState.Skill;
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coSkill = null;
        CheckUpdatedFlag();
    }

    protected IEnumerator CoStartShootArrow(){
        GameObject go = Managers.Resource.Instantiate("Creatures/Arrow");
        ArrowController ac = go.GetComponent<ArrowController>();

        ac.Dir = _lastDir;
        ac.CellPos = CellPos;

        //대기 시간
        _rangeSkill = true;
        yield return new WaitForSeconds(0.3f);
        State = CreatureState.Idle;
        _coSkill = null;
    }

    public override void OnDamaged(){
        Debug.Log("Player Hit");
    }

}
