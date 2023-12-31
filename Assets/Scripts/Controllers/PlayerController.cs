using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : CreatureController
{
    Coroutine _coSkill;
    bool _rangeSkill = false;

    private void Start() {
        Init();
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateAnimation(){
        if(State == CreatureState.Idle){
            switch(lastDir_){
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
            switch(lastDir_){
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

    void Update()
    {
        UpdateController();       
    }

    protected override void UpdateController()
    {
        switch(State){
            case CreatureState.Idle:
                GetDirectionInput();
                break;
            case CreatureState.Moving:
                GetDirectionInput();
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Dead:
                break;
        }

        

        base.UpdateController();
    }

    private void LateUpdate() {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    protected override void UpdateIdle(){
        //이동 상태로 갈    지 확인
        if(Dir != MoveDir.None){
            State = CreatureState.Moving;
            return;
        }

        //스킬을 사용할 지 확인
        if(Input.GetKey(KeyCode.Space)){
            State = CreatureState.Skill;
            //coSkill_ = StartCoroutine(CoStartPunch());
            _coSkill = StartCoroutine(CoStartShootArrow());
        }
    }

    IEnumerator CoStartPunch(){
        //피격 판정
        GameObject go = Managers.Obj.Find(GetFrontPos());
        if(go != null){
            CreatureController cc = go.GetComponent<CreatureController>();
            if(cc != null)
                cc.OnDamaged();
        }

        //대기 시간
        _rangeSkill = false;
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coSkill = null;
    }

    IEnumerator CoStartShootArrow(){
        GameObject go = Managers.Resource.Instantiate("Creatures/Arrow");
        ArrowController ac = go.GetComponent<ArrowController>();

        ac.Dir = lastDir_;
        ac.CellPos = CellPos;

        //대기 시간
        _rangeSkill = true;
        yield return new WaitForSeconds(0.3f);
        State = CreatureState.Idle;
        _coSkill = null;
    }

    //입력을 받아 방향 설정
    void GetDirectionInput(){
        if(Input.GetKey(KeyCode.W)){
            Dir = MoveDir.Up;
        }
        else if(Input.GetKey(KeyCode.S)){
            Dir = MoveDir.Down;
        }
        else if(Input.GetKey(KeyCode.A)){
            Dir = MoveDir.Left;
        }
        else if(Input.GetKey(KeyCode.D)){
            Dir = MoveDir.Right;
        }
        else{
            Dir = MoveDir.None;
        }
    }

}
