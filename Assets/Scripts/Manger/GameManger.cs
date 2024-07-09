using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : Singleton<GameManger>
{
    private bool _isDayGone = false;
    private int _hour = 9;
    private int _minute = 0;
    private float _revenue = 0;
    Coroutine _Timer;

    public float Revenu { get { return _revenue; } }
    public bool IsDayGone { get { return _isDayGone; } }

    private void Awake()
    {
        RegisterDayEvent();
    }
    private void Start()
    {
        UIManger.Instance.SetHourTimeText(_hour);
        UIManger.Instance.SetMinuteTimeText(_minute);
        _Timer = StartCoroutine(Timer());
    }
    private void OnDisable()
    {
        UnRegisterDayEvent();
    }
    private IEnumerator Timer()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(10f);

            _minute++;
            UIManger.Instance.SetMinuteTimeText(_minute);

            if (_minute >= 60)
            {
                _minute = 0;
                _hour++;
                UIManger.Instance.SetHourTimeText(_hour);
                UIManger.Instance.SetMinuteTimeText(_minute);
            }

            if( _hour >= 24)
            {
                EventManger.Instance.OnDayGoneEventInvoke();
            }
            
        }

    }
    public void ResetValue()
    {
        _revenue = 0;
        _hour = 9;
        _minute = 0;
    }
    public void DayStart()
    {
        ResetValue();
        _isDayGone = false;
        UIManger.Instance.SetHourTimeText(_hour);
        UIManger.Instance.SetMinuteTimeText(_minute);
        _Timer = StartCoroutine(Timer());

    }
    private void DayGone()
    {
        PlayerController.Instance.PlusMoney(_revenue);
        _isDayGone = true;
        StopCoroutine(_Timer);
    }
    private void RegisterDayEvent()
    {
        EventManger.Instance.DayGone += DayGone;
        EventManger.Instance.DayStart += DayStart;
    }
    private void UnRegisterDayEvent()
    {
        EventManger.Instance.DayGone -= DayGone;
        EventManger.Instance.DayStart -= DayStart;
    }
    public void PluseIncomeMoney(float price)
    {
        _revenue += price;
    }
    //private void RegisterDayStartEvent()
    //{
    //    EventManger.Instance.DayStart += ResetRevenue;

    //}
    //private void UnRegisterDayStartEvent()
    //{
    //    EventManger.Instance.DayStart -= ResetRevenue;
    //}

}
