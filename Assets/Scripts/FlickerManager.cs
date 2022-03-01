using UnityEngine;
using System.Collections;
 
public class FlickerManager : MonoBehaviour
{
    public float MaxReduction;
    public float MaxIncrease;
    public float Frequence;
    public float Strength;
 
    private Light lightSource;
    private float baseIntensity;
    private bool flickering;
 
    public void Reset()
    {
        MaxReduction = 0.1f;
        MaxIncrease = 0.2f;
        Frequence = 0.1f;
        Strength = 300;
    }
 
    public void Start()
    {
        lightSource = GetComponent<Light>();
        baseIntensity = lightSource.intensity;
        StartCoroutine(DoFlicker());
    }
 
    void Update()
    {
        if (!flickering)
        {
            StartCoroutine(DoFlicker());
        }
    }
 
    private IEnumerator DoFlicker()
    {
        flickering = true;
        lightSource.intensity = Mathf.Lerp(lightSource.intensity, Random.Range(baseIntensity - MaxReduction, baseIntensity + MaxIncrease), Strength * Time.deltaTime);
        yield return new WaitForSeconds(Frequence);
        flickering = false;
     }
 }
