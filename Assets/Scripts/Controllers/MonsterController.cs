using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterController : CreatureController
{
    private void Start() {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        State = CreatureState.Idle;
        Dir = MoveDir.None;
    }

    void Update()
    {
        UpdateController();       
    }

    protected override void UpdateController()
    {
        //GetDirectionInput();
        base.UpdateController();
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

    public override void OnDamaged(){
        GameObject effect = Managers.Resource.Instantiate("Effects/DieEffect");
        effect.transform.position = transform.position;
        effect.GetComponent<Animator>().Play("Play");
        GameObject.Destroy(effect, 0.5f);

        Managers.Obj.Remove(gameObject);
        Managers.Resource.Destroy(gameObject); 
    }

}
