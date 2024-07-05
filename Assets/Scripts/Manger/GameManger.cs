using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : Singleton<GameManger>
{
    private bool _isDayGone = false;
    private int _hour = 9;
    private int _minute = 0;


    private void Start()
    {
        UIManger.Instance.SetHourTimeText(_hour);
        UIManger.Instance.SetMinuteTimeText(_minute);
        StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        
        while (_isDayGone == false)
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
            
        }

    }

}
