using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{

    public Color[] TeamColors;
    private int _ScoreBlue = 0;
    private int _ScoreRed =0;
    private int _ScoreYellow =0;

    protected override void SingletonAwake()
    {
    }

    private void OnEnable()
    {
        UIScreenManager.Instance.ShowScreen<GamePlayScreen>();
    }
    private void Update()
    {
        if(_ScoreBlue == 3 || _ScoreRed == 3 || _ScoreYellow == 3){
            UIScreenManager.Instance.ShowScreen<MainMenuScreen>();
        }
    }
    public float GetBlue(){
        return _ScoreBlue;
    }
    public float GetRed(){
        return _ScoreRed;
    }
    public float GetYellow(){
        return _ScoreYellow;
    }
    public void PlusBlue(){
        _ScoreBlue += 1;
    }
     public void PlusRed(){
        _ScoreRed += 1;
    }
     public void PlusYellow(){
        _ScoreYellow += 1;
    }
    public void MinusBlue(){
        _ScoreBlue -= 1;
    }
     public void MinusRed(){
        _ScoreRed -= 1;
    }
     public void MinusYellow(){
        _ScoreYellow -= 1;
    }
}
