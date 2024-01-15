using System;
using Google.Protobuf.Protocol;
using UnityEngine;

public class CreatureController : BaseController
{
    HpBar _hpBar;

    public override StatInfo Stat {
        get {return base.Stat;} 
        set {
            base.Stat = value;
            UpdateHpBar();
        }
    }

    public override int Hp{
        get{return base.Hp;}
        set
        {
            base.Hp = value;
            UpdateHpBar();
        }
    }

    protected void AddHpBar(){
        GameObject go = Managers.Resource.Instantiate("UI/HpBar", transform);
        go.transform.localPosition = new Vector3(0, 0.5f, 0);
        go.name = "HpBar";
        _hpBar = go.GetComponent<HpBar>();
        UpdateHpBar();
    }

    void UpdateHpBar(){
        if(_hpBar == null) return;

        float ratio = 0.0f;
        if(Stat.MaxHP > 0){
            ratio = (float)Hp / (float)Stat.MaxHP;
        }
        _hpBar.SetHpBar(ratio);
    }

    protected override void Init(){
        base.Init();
        AddHpBar();
    }

    public virtual void OnDamaged(){

    }

    public virtual void OnDead()
    {
        State = CreatureState.Dead;

        GameObject effect = Managers.Resource.Instantiate("Effects/DieEffect");
        effect.transform.position = transform.position;
        effect.GetComponent<Animator>().Play("Play");
        GameObject.Destroy(effect, 0.5f);
    }

}
