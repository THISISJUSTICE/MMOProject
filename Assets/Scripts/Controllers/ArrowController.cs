using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public class ArrowController : CreatureController
{
    private void Start() {
        Init();
    }
    protected override void Init()
    {
        switch(lastDir_){
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

        base.Init();
    }

    private void Update() {
        UpdateController();
    }

    protected override void UpdateAnimation()
    {
    }

    //현재 방향을 확인하여 목표 위치 연산
    protected override void UpdateIdle(){
        if(Dir != MoveDir.None){
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
                    Debug.Log($"{go.name} 피격");
                    Managers.Resource.Destroy(gameObject);    
                }
            }

            else{
                Managers.Resource.Destroy(gameObject);
            }
        }
    }
}
