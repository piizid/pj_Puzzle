using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_StateNumber : MonoBehaviour {
    int _preStageNum;
    int _preMaxStageNum;
    

	// Use this for initialization
	void Start () {
        _preStageNum = BattleManager._Instance._CurrentStateNum;
        _preMaxStageNum = BattleManager._Instance._MaxStateNum;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (BattleManager._Instance._MaxStateNum > 0)
        {
            if (_preStageNum != BattleManager._Instance._CurrentStateNum ||
                _preMaxStageNum != BattleManager._Instance._MaxStateNum)
            {
                _preStageNum = BattleManager._Instance._CurrentStateNum;
                _preMaxStageNum = BattleManager._Instance._MaxStateNum;
                updateText(_preStageNum, _preMaxStageNum);
            }
        }
	}

    void updateText( int currentStageNum, int maxStageNum )
    {
        GetComponent<Text>().text = string.Format("{0} / {1}", currentStageNum, maxStageNum);
    }

    
}
