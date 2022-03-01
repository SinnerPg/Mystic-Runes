using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1Manager : MonoBehaviour
{
    private int triggerCount;
    public Animator  door;
    public AudioSource[] sounds;
    [Header("Animators")]
    public Animator platform;
    public Animator platform2;
    [Header("Cinematics")]
    public GameObject[] timelines;
    [Header("Glyphs")]
    public GameObject[] mainGlyphs;
    public GameObject[] subGlyphs;
    public Material glyphsOn;
    private bool timelinePlayed, timelinePlayed2, timelinePlayed3;
    private int step = 1;

    void Update()
    {
        if(triggerCount == 2)
        {
            platform.enabled = true;
            if(!timelinePlayed)
            {
                timelines[1].SetActive(true);
            }
            timelinePlayed = true;
            platform.SetBool("running", true);
        }
        else if (triggerCount <= 2)
        {
            platform.SetBool("running", false);
        }

        if(triggerCount == 4)
        {
            platform2.enabled = true;
            if(!timelinePlayed2)
            {
                timelines[2].SetActive(true);
            }
            timelinePlayed2 = true;
        }

        if(triggerCount == 6)
        {
            if(!timelinePlayed3)
            {
                timelines[3].SetActive(true);
            }
            timelinePlayed3 = true;
        }
    }

    public void enableTrigger()
    {
       triggerCount++;
    }

    public void disableTrigger()
    {
        triggerCount--;
    }

    public void enableInteractionGlyphUp(int x)
    {
        switch(step)
        {
            case 1:
                if(x == 4)
                {
                    step++;
                    sounds[1].Play();
                }
                else
                {
                    step = 1;
                    if(!sounds[0].isPlaying)
                    {
                        sounds[0].Play();
                    }
                }
                break;
            case 2:
                if(x == 5)
                {
                    step++;
                    sounds[1].Play();
                }
                else
                {
                    step = 1;
                    if(!sounds[0].isPlaying)
                    {
                        sounds[0].Play();
                    }
                }
                break;
            case 3:
                if(x == 6)
                {
                    step++;
                    sounds[1].Play();
                }
                else
                {
                    step = 1;
                    if(!sounds[0].isPlaying)
                    {
                        sounds[0].Play();
                    }
                }
                break;
            case 4:
                if(x == 7)
                {
                    step++;
                    timelines[4].SetActive(true);
                }
                else
                {
                    step = 1;
                    if(!sounds[0].isPlaying)
                    {
                        sounds[0].Play();
                    }
                }
                break;
            default: 
                break;
        }
    }

    public void enableInteractionGlyph(int x)
    {
        switch(x)
        {
            case 1:
                mainGlyphs[0].GetComponent<MeshRenderer>().material = glyphsOn;
                subGlyphs[0].GetComponent<MeshRenderer>().material = glyphsOn;
                break;
            case 2:
                mainGlyphs[1].GetComponent<MeshRenderer>().material = glyphsOn;
                subGlyphs[1].GetComponent<MeshRenderer>().material = glyphsOn;
                break;
            case 3:
                mainGlyphs[2].GetComponent<MeshRenderer>().material = glyphsOn;
                subGlyphs[2].GetComponent<MeshRenderer>().material = glyphsOn;
                door.enabled = true;
                break;
            default: 
                break;
        }
    }
}
