using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : CreatureController
{
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
        GetDirectionInput();
        base.UpdateController();
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
