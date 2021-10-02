using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLight : MonoBehaviour
{
    public Color[] Colors;
    private Light lamp;


    // Start is called before the first frame update
    void Start()
    {
        lamp = GetComponent<Light>();
        int rand = Random.Range(0, Colors.Length);
        lamp.color = Colors[rand];

        float RadiusModifier = Random.Range(0, 1f);
        lamp.range += RadiusModifier;

        float IntensityModifier = Random.Range(-1f, 1f);
        lamp.intensity += IntensityModifier;

        lamp.shadowStrength = Random.Range(0.75f, 1f);
    }


}
