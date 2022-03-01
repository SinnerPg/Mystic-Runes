using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    private Text text;
    private float time;
    void Awake()
    {
        text = GetComponent<Text>();
        time = TimerManager.m_timer;
    }

    void Start()
    {
        float n = Mathf.Ceil(time);
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Ceil(time - (60 * minutes));
        text.text = "Hai completato il livello in " + minutes.ToString() + " : " + seconds.ToString() + " minuti";
    }
}
