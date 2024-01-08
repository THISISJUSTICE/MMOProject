using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class TempDebug : MonoBehaviour
{
    Text text;
    List<string> lines;
    const int MAX_LINE = 8;
    static TempDebug _tempDebug;
    public static TempDebug TD {get {return _tempDebug;}}

    private void Awake() {
        if(_tempDebug == null){
            _tempDebug = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start() {
        text = GetComponent<Text>();  
        text.text = "";  
        lines = new List<string>();
    }

    public void Log(string txt){
        lines.Add(txt);

        if(lines.Count >= MAX_LINE){
            lines.RemoveAt(0);
        }
        LogPrint();
    }

    void LogPrint(){
        text.text = "";
        for(int i=0; i<lines.Count; i++){
            text.text += lines[i] +"\n";
        }
    }
    
}
