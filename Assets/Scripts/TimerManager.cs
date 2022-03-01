using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public static float m_timer = 0;

    void Update()
    {
        m_timer += Time.deltaTime;

        float n = Mathf.Ceil(m_timer);


        float minutes = Mathf.Floor(m_timer / 60);
        float seconds = Mathf.Ceil(m_timer - (60 * minutes));

        this.GetComponent<Text>().text = "Time : " + minutes.ToString() + " : " + seconds.ToString();
    }
}
