using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Google.Protobuf.Protocol;
using UnityEngine;

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
        base.UpdateIdle();

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

    protected override void MoveToNextPos(){
        if(Dir == MoveDir.None){
            State = CreatureState.Idle;
            CheckUpdatedFlag();
            return;
        }

        Vector3Int destPos = CellPos;
        switch(Dir){
            case MoveDir.Up:
            destPos += Vector3Int.up;
            break;
            case MoveDir.Down:
            destPos += Vector3Int.down;
            break;
            case MoveDir.Left:
            destPos += Vector3Int.left;
            break;
            case MoveDir.Right:
            destPos += Vector3Int.right;
            break;
        }

        if(Managers.Map.CanGo(destPos)){
            if(Managers.Obj.Find(destPos) == null){
                CellPos = destPos;
                
            }
        }

        CheckUpdatedFlag();
    }

    void CheckUpdatedFlag(){
        if(_updated){
            C_Move movePacket = new C_Move();
            movePacket.PosInfo = PosInfo;
            Managers.Network.Send(movePacket);
            _updated = false;
        }
    }

}
