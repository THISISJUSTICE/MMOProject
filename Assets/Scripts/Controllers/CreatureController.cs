using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static Define;

public class CreatureController : MonoBehaviour
{
    public float speed_ = 5.0f;

    public Vector3Int CellPos {get; set;} = Vector3Int.zero;
    protected Animator animator_;
    protected SpriteRenderer sprite_;

    CreatureState state_ = CreatureState.Idle;
    public CreatureState State{
        get{return state_;}
        set{
            if(state_ == value) return;
            state_ = value;
            UpdateAnimation();
        }
    }

    MoveDir lastDir_ = MoveDir.Down;
    MoveDir dir_ = MoveDir.Down;
    public MoveDir Dir{
        get{ return dir_; }
        set{
            if(dir_ == value) return;

            dir_ = value;
            if(value != MoveDir.None){
                lastDir_ = value;
            }
            UpdateAnimation();
        }
    }

    public Vector3Int GetFrontPos(){
        Vector3Int cellPos = CellPos;

        switch(lastDir_){
            case MoveDir.Up:
                cellPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                cellPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                cellPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                cellPos += Vector3Int.right;
                break;
        }
        return cellPos;
    }

    protected virtual void UpdateAnimation(){
        if(state_ == CreatureState.Idle){
            switch(lastDir_){
                case MoveDir.Up:
                    animator_.Play("Idle_Back");
                    sprite_.flipX = false;
                    break;
                case MoveDir.Down:
                    animator_.Play("Idle_Front");
                    sprite_.flipX = false;
                    break;
                case MoveDir.Left:
                    animator_.Play("Idle_Right");
                    sprite_.flipX = true;
                    break;
                case MoveDir.Right:
                    animator_.Play("Idle_Right");
                    sprite_.flipX = false;
                    break;
            }
        }
        else if(state_ == CreatureState.Moving){
            switch(dir_){
                case MoveDir.Up:
                    animator_.Play("Walk_Back");
                    sprite_.flipX = false;
                    break;
                case MoveDir.Down:
                    animator_.Play("Walk_Front");
                    sprite_.flipX = false;
                    break;
                case MoveDir.Left:
                    animator_.Play("Walk_Right");
                    sprite_.flipX = true;
                    break;
                case MoveDir.Right:
                    animator_.Play("Walk_Right");
                    sprite_.flipX = false;
                    break;

            }
        }
        else if (state_ == CreatureState.Skill){
            switch(lastDir_){
                case MoveDir.Up:
                    animator_.Play("Attack_Back");
                    sprite_.flipX = false;
                    break;
                case MoveDir.Down:
                    animator_.Play("Attack_Front");
                    sprite_.flipX = false;
                    break;
                case MoveDir.Left:
                    animator_.Play("Attack_Right");
                    sprite_.flipX = true;
                    break;
                case MoveDir.Right:
                    animator_.Play("Attack_Right");
                    sprite_.flipX = false;
                    break;

            }
        }
        else{

        }
    }

    protected virtual void Init(){
        animator_ = GetComponent<Animator>();
        sprite_ = GetComponent<SpriteRenderer>();
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        transform.position = pos;
    }

    protected virtual void UpdateController(){
        switch(State){
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Moving:
                UpdateMoving();
                break;
            case CreatureState.Skill:
                UpdateSkill();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    //현재 방향을 확인하여 목표 위치 연산
    protected virtual void UpdateIdle(){
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
                if(Managers.Obj.Find(destPos) == null){
                    CellPos = destPos;
                    
                }
            }
        }
    }

    //방향을 받아 이동
    protected virtual void UpdateMoving(){
        if(State != CreatureState.Moving) return;
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 moveDir = destPos - transform.position;
        
        //도착 여부 확인
        float dist = moveDir.magnitude;
        if(dist < speed_ * Time.deltaTime){
            transform.position = destPos;
            //예외적으로 애니메이션을 직접 컨트롤
            state_ = CreatureState.Idle;
            if(dir_ == MoveDir.None){
                UpdateAnimation();
            }
        }
        else{
            transform.position += moveDir.normalized * speed_ * Time.deltaTime;
            State = CreatureState.Moving;
        }
    }

    protected virtual void UpdateSkill(){

    }

    protected virtual void UpdateDead(){

    }

}
