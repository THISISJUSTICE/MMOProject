using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour
{
    public float speed_ = 5.0f;
    public Grid grid_;

    Vector3Int cellPos_ = Vector3Int.zero;
    bool isMoving_ = false;
    Animator animator_;

    MoveDir dir_ = MoveDir.Right;
    public MoveDir Dir{
        get{ return dir_; }
        set{
            if(dir_ == value) return;
            switch(value){
                case MoveDir.Up:
                    animator_.Play("Walk_Back");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
                case MoveDir.Down:
                    animator_.Play("Walk_Front");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
                case MoveDir.Left:
                    animator_.Play("Walk_Right");
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    break;
                case MoveDir.Right:
                    animator_.Play("Walk_Right");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                case MoveDir.None:
                if(dir_ == MoveDir.Up){
                    animator_.Play("Idle_Back");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if(dir_ == MoveDir.Down){
                    animator_.Play("Idle_Front");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if(dir_ == MoveDir.Left){
                    animator_.Play("Idle_Right");
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else{
                    animator_.Play("Idle_Right");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                    break;
            }
            dir_ = value;
        }
    }

    private void Start() {
        animator_ = GetComponent<Animator>();
        Vector3 pos = grid_.CellToWorld(cellPos_) + new Vector3(0.5f, 0.5f, 0);
        transform.position = pos;
    }
    
    void Update()
    {
        GetDirectionInput();
        UpdatePosition();
        UpdateIsMoving();        
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

    //방향을 받아 이동
    void UpdatePosition(){
        if(!isMoving_) return;
        Vector3 destPos = grid_.CellToWorld(cellPos_) + new Vector3(0.5f, 0.5f, 0);
        Vector3 moveDir = destPos - transform.position;
        
        //도착 여부 확인
        float dist = moveDir.magnitude;
        if(dist < speed_ * Time.deltaTime){
            transform.position = destPos;
            isMoving_ = false;
        }
        else{
            transform.position += moveDir.normalized * speed_ * Time.deltaTime;
            isMoving_ = true;
        }
    }

    //현재 방향을 확인하여 목표 위치 연산
    void UpdateIsMoving(){
        if(!isMoving_){
            switch(Dir){
                case MoveDir.Up:
                cellPos_ += Vector3Int.up;
                isMoving_ = true;
                break;
                case MoveDir.Down:
                cellPos_ += Vector3Int.down;
                isMoving_ = true;
                break;
                case MoveDir.Left:
                cellPos_ += Vector3Int.left;
                isMoving_ = true;
                break;
                case MoveDir.Right:
                cellPos_ += Vector3Int.right;
                isMoving_ = true;
                break;
            }
            
        }
    }

}
