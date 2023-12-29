using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : CreatureController
{
    Coroutine coSkill_;

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

    protected override void UpdateController()
    {
        switch(State){
            case CreatureState.Idle:
                GetDirectionInput();
                GetIdleInput();
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

    void GetIdleInput(){
        if(Input.GetKey(KeyCode.Space)){
            State = CreatureState.Skill;
            coSkill_ = StartCoroutine(CoStartPunch());
        }
    }

    IEnumerator CoStartPunch(){
        //피격 판정
        GameObject go = Managers.Obj.Find(GetFrontPos());
        if(go != null){
            Debug.Log($"{go.name} 피격");
        }

        //대기 시간
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        coSkill_ = null;
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
