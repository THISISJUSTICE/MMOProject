using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;



public class ArrowController : CreatureController
{
    private void Start() {
        Init();
    }
    protected override void Init()
    {
        switch(_lastDir){
            case MoveDir.Up:
                transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case MoveDir.Down:
                transform.rotation = Quaternion.Euler(0,0,-180);
                break;
            case MoveDir.Left:
                transform.rotation = Quaternion.Euler(0,0,90);
                break;
            case MoveDir.Right:
                transform.rotation = Quaternion.Euler(0,0,-90);
                break;
        
        }

        State = CreatureState.Moving;
        _speed = 15.0f;

        base.Init();
    }

    private void Update() {
        UpdateController();
    }

    protected override void UpdateAnimation()
    {
    }

    //현재 방향을 확인하여 목표 위치 연산
    protected override void MoveToNextPos(){
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
        
        State = CreatureState.Moving;

        if(Managers.Map.CanGo(destPos)){
            GameObject go = Managers.Obj.Find(destPos);
            if(go == null){
                CellPos = destPos;
                
            }
            else{
                CreatureController cc = go.GetComponent<CreatureController>();
                if(cc != null)
                    cc.OnDamaged();

                Managers.Resource.Destroy(gameObject);    
            }
        }

        else{
            Managers.Resource.Destroy(gameObject);
        }
        
    }
}
