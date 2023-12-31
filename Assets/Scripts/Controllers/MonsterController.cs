using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterController : CreatureController
{
    Coroutine coPatrol_;
    Vector3Int destCellPos_;

    public override CreatureState State{
        get{return state_;}
        set{
            if(state_ == value) return;
            base.State = value;

            if(coPatrol_ != null){
                StopCoroutine(coPatrol_);
                coPatrol_ = null;
            }
        }
    }

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

    protected override void UpdateIdle()
    {
        base.UpdateIdle();

        if(coPatrol_ == null){
            coPatrol_ = StartCoroutine(CoPatrol());
        }
    }

    protected override void MoveToNextPos()
    {
        Vector3Int moveCellDir = destCellPos_ - CellPos;
        if(moveCellDir.x > 0){
            Dir = MoveDir.Right;
        }
        else if(moveCellDir.x < 0){
            Dir = MoveDir.Left;
        }
        else if(moveCellDir.y > 0){
            Dir = MoveDir.Up;
        }
        else if(moveCellDir.y < 0){
            Dir = MoveDir.Down;
        }
        else{
            Dir = MoveDir.None;
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

        if(Managers.Map.CanGo(destPos) && Managers.Obj.Find(destPos) == null){
            CellPos = destPos;
        }
        else{
            State = CreatureState.Idle;
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

    IEnumerator CoPatrol(){
        float waitSeconds = Random.Range(1, 4);
        yield return new WaitForSeconds(waitSeconds);

        for(int i=0; i<10; i++){
            int xRange = Random.Range(-5, 6);
            int yRange = Random.Range(-5, 6);
            Vector3Int randPos = CellPos + new Vector3Int(xRange, yRange, 0);

            if(Managers.Map.CanGo(randPos) && Managers.Obj.Find(randPos) == null){
                destCellPos_ = randPos;
                State = CreatureState.Moving;
                yield break;
            }
        }

        State = CreatureState.Idle;
    }

}
