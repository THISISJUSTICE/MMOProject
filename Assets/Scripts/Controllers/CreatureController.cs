using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public int id {get; set;}
    [SerializeField] public float _speed = 5.0f;

    protected bool _updated = false;
    protected PositionInfo _posInfo = new PositionInfo();
    public PositionInfo PosInfo{
        get{return _posInfo;}
        set{
            if(_posInfo.Equals(value)){
                return;
            }
            CellPos = new Vector3Int(value.PosX, value.PosY, 0);
            State = value.State;
            Dir = value.MoveDir;
        }
    }

    public void SyncPos(){
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        transform.position = destPos;
    }

    public Vector3Int CellPos {
        get{
            return new Vector3Int(PosInfo.PosX, PosInfo.PosY, 0);
        }
        
        set{
            if(PosInfo.PosX == value.x && PosInfo.PosY == value.y) return;

            PosInfo.PosX = value.x;
            PosInfo.PosY = value.y;
            _updated = true;
            
        }
    }
    protected Animator _animator;
    protected SpriteRenderer _sprite;

    public virtual CreatureState State{
        get{return PosInfo.State;}
        set{
            if(PosInfo.State == value) return;
            PosInfo.State = value;
            _updated = true;
            UpdateAnimation();
        }
    }

    protected MoveDir _lastDir = MoveDir.Down;
    public MoveDir Dir{
        get{ return PosInfo.MoveDir; }
        set{
            if(PosInfo.MoveDir == value) return;

            PosInfo.MoveDir = value;
            if(value != MoveDir.None){
                _lastDir = value;
            }
            UpdateAnimation();
            _updated = true;
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

        switch(_lastDir){
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

    void Awake()
	{
		Init();
	}

	void Update()
	{
		UpdateController();
	}

    protected virtual void Init(){
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        transform.position = pos;

        State = CreatureState.Idle;
        Dir = MoveDir.None;
        UpdateAnimation();
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
    protected void UpdateMoving(){
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
        
    }

    protected virtual void UpdateSkill(){

    }

    protected virtual void UpdateDead(){

    }

    public virtual void OnDamaged(){

    }

}
