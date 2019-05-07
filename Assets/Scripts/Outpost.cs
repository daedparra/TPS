using System.Collections.Generic;
using UnityEngine;

public class Outpost : MonoBehaviour
{
    public static List<Outpost> OutpostList = new List<Outpost>();
    [SerializeField] private GameObject _makeman;
    private float _waitingTime =3f;
    private float _counter;
    private bool _OneTime = false;
    private bool _other = false;
    private bool _OneTimeMinus = false;
    private bool _otherMinus = false;
    private int Tempteam;

    public float CaptureValue { get; private set; }
    public int CurrentTeam { get; private set; }

    [SerializeField, Tooltip("Time in Seconds")]
    private float _CaptureTime = 5f;

    private SkinnedMeshRenderer _FlagRenderer;

    private void OnEnable()
    {
        _FlagRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        CurrentTeam = -1;
        OutpostList.Add(this);
    }

    private void OnDisable()
    {
        OutpostList.Remove(this);
    }

    private void OnTriggerStay(Collider other)
    {
        AUnit u = other.GetComponent<AUnit>();
        if(u == null || u.IsAlive == false)
        {
            return;
        }


        if(u.TeamNumber == CurrentTeam)
        {
            CaptureValue += Time.fixedDeltaTime / _CaptureTime;
            if(CaptureValue > 1f) CaptureValue = 1f;
            
        }
        else
        {
            Tempteam = u.TeamNumber;
            CaptureValue -= Time.fixedDeltaTime / _CaptureTime;
            if(CaptureValue <= 0f)
            {
                CaptureValue = 0f;
                CurrentTeam = u.TeamNumber;
                _OneTime = false;
                _other = false;
               _otherMinus = false;
               _OneTimeMinus = false;
            }
        }
    }

    private void Update()
    {   
        if(CaptureValue == 1f && _other == false) _OneTime = true;
        if(Tempteam != CurrentTeam && CaptureValue < 1f && _otherMinus == false) _OneTimeMinus = true;
        _counter += Time.deltaTime;
        if(_counter >= _waitingTime && CaptureValue == 1f){
            _counter = 0;
            _makeman.GetComponent<AUnit>().TeamNumber = CurrentTeam;
            Instantiate(_makeman,transform.position,transform.rotation);
        }
        if(_OneTimeMinus){
            if(CurrentTeam == 0){
                GameManager.Instance.MinusRed();
                _otherMinus = true;
                _OneTimeMinus = false;
            }else if(CurrentTeam == 1){
                GameManager.Instance.MinusYellow();
                _OneTimeMinus = false;
                _otherMinus = true;
            }
            else if(CurrentTeam == 2){
                GameManager.Instance.MinusBlue();
               _OneTimeMinus = false;
               _otherMinus = true;
            }
        }
        if(_OneTime){
            if(CurrentTeam == 0 && CaptureValue == 1f){
                GameManager.Instance.PlusRed();
                _other = true;
                _OneTime = false;
            }else if(CurrentTeam == 1 && CaptureValue == 1f){
                GameManager.Instance.PlusYellow();
                _OneTime = false;
                _other = true;
            }
            else if(CurrentTeam == 2 && CaptureValue == 1f){
                GameManager.Instance.PlusBlue();
               _OneTime = false;
               _other = true;
            }
        }
        if(CurrentTeam != -1){
        Color teamColor = GameManager.Instance.TeamColors[CurrentTeam];
        _FlagRenderer.material.color = Color.Lerp(Color.white, teamColor, CaptureValue);
        }
    }

}

public static class ListExntensions
{
    public static Outpost GetRandomOutpost(this List<Outpost> lst)
    {
        if(lst.Count > 0)
        {
            int rnd = Random.Range(0, lst.Count);
            return lst[rnd];
        }
        return null;
    }
}
