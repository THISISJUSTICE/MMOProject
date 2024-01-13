using System.Collections;
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
        if(_animator == null || _sprite == null) return;
        if(State == CreatureState.Idle){
            switch(Dir){
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
            switch(Dir){
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

    public void UseSkill(int skillID)
    {
        if(skillID == 1){
            _coSkill = StartCoroutine(CoStartPunch());
        }

        else if(skillID == 2){
            _coSkill = StartCoroutine(CoStartShootArrow());
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
        //대기 시간
        _rangeSkill = true;
        State = CreatureState.Skill;
        yield return new WaitForSeconds(0.3f);
        State = CreatureState.Idle;
        _coSkill = null;
        CheckUpdatedFlag();
    }

    public override void OnDamaged(){
        Debug.Log("Player Hit");
    }

}
