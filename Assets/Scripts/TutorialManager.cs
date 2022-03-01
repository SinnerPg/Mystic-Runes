using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{   
    public Image tutorialColor;
    Text textComponent;

    void Awake()
    {
        textComponent = GetComponent<Text>();
    }

    public void showTutorial(string text)
    {
        textComponent.text = text;
        StartCoroutine(fadeTutorial(tutorialColor.color.a, 1));
        StartCoroutine(fadeText(textComponent.color.a, 1));
    }

    public void hideTutorial()
    {
        StartCoroutine(fadeTutorial(tutorialColor.color.a, 0));
        StartCoroutine(fadeText(textComponent.color.a, 0));
    }

    IEnumerator fadeTutorial(float start, float end, float lerpTime = 1)
    {
        float timeStartFade = Time.time;
        float timeSinceStart = Time.time - timeStartFade;
        float percentage = timeSinceStart / lerpTime;

        while(true)
        {
            timeSinceStart = Time.time - timeStartFade;
            percentage = timeSinceStart / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentage);
            tutorialColor.color = new Color(1,1,1, currentValue);

            if(percentage >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator fadeText(float start, float end, float lerpTime = 1)
    {
        float timeStartFade = Time.time;
        float timeSinceStart = Time.time - timeStartFade;
        float percentage = timeSinceStart / lerpTime;

        while(true)
        {
            timeSinceStart = Time.time - timeStartFade;
            percentage = timeSinceStart / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentage);
            textComponent.color = new Color(1,1,1, currentValue);

            if(percentage >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
}
