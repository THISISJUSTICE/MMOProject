using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour
{
    public float speed_ = 5.0f;
    public Grid grid_;

    Vector3Int cellPos_ = Vector3Int.zero;
    MoveDir dir_ = MoveDir.None;

    bool isMoving_ = false;

    private void Start() {
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
            dir_ = MoveDir.Up;
        }
        else if(Input.GetKey(KeyCode.S)){
            dir_ = MoveDir.Down;
        }
        else if(Input.GetKey(KeyCode.A)){
            dir_ = MoveDir.Left;
        }
        else if(Input.GetKey(KeyCode.D)){
            dir_ = MoveDir.Right;
        }
        else{
            dir_ = MoveDir.None;
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
            switch(dir_){
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
