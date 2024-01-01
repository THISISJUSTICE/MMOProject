using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static Define;

public class CreatureController : MonoBehaviour
{
    [SerializeField] public float _speed = 5.0f;

    public Vector3Int CellPos {get; set;} = Vector3Int.zero;
    protected Animator _animator;
    protected SpriteRenderer _sprite;

    [SerializeField] protected CreatureState _state = CreatureState.Idle;
    public virtual CreatureState State{
        get{return _state;}
        set{
            if(_state == value) return;
            _state = value;
            UpdateAnimation();
        }
    }

    protected MoveDir lastDir_ = MoveDir.Down;
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

    public MoveDir GetDirFromVec(Vector3Int dir){
        if(dir.x > 0){
            return MoveDir.Right;
        }
        else if(dir.x < 0){
            return MoveDir.Left;
        }
        else if(dir.y > 0){
            return MoveDir.Up;
        }
        else if(dir.y < 0){
            return MoveDir.Down;
        }
        else{
            return MoveDir.None;
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
        if(_state == CreatureState.Idle){
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
        else if(_state == CreatureState.Moving){
            switch(dir_){
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
        else if (_state == CreatureState.Skill){
            switch(lastDir_){
                case MoveDir.Up:
                    _animator.Play("Attack_Back");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("Attack_Front");
                    _sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("Attack_Right");
                    _sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("Attack_Right");
                    _sprite.flipX = false;
                    break;

            }
        }
        else{

        }
    }

    protected virtual void Init(){
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
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

    protected virtual void UpdateIdle(){
        
    }

    //방향을 받아 이동
    protected virtual void UpdateMoving(){
        if(State != CreatureState.Moving) return;
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 moveDir = destPos - transform.position;
        
        //도착 여부 확인
        float dist = moveDir.magnitude;
        if(dist < _speed * Time.deltaTime){
            transform.position = destPos;
            MoveToNextPos();
        }
        else{
            transform.position += moveDir.normalized * _speed * Time.deltaTime;
            State = CreatureState.Moving;
        }
    }

    protected virtual void MoveToNextPos(){
        if(Dir == MoveDir.None){
            State = CreatureState.Idle;
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
    }

    protected virtual void UpdateSkill(){

    }

    protected virtual void UpdateDead(){

    }

    public virtual void OnDamaged(){

    }

}
