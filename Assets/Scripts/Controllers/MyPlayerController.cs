using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
    protected override void Init()
    {
        base.Init();
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
        }

        base.UpdateController();
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

    private void LateUpdate() {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
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
