using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseController : MonoBehaviour
{
    [SerializeField]
    private float maxLevelNoise = 10;
    [SerializeField]
    private float upNoiseInSecond = 3;
    [SerializeField]
    private float downNoiseInSecond = 0.5f;
    [SerializeField]
    private float noise;
    private Image noiseBar;
    public bool NoiseClip;

    private void Start()
    {
        noiseBar = GetComponentInChildren<Image>();
        noiseBar.fillAmount = 0;
    }
    void Update()
    {
        DownNoise();
        noiseBar.fillAmount = noise;
    }
    public void PlayerSteep(int SteepInSecond = 1)
    {
        if (noise < 1)
        {
            noise += (upNoiseInSecond / SteepInSecond) / maxLevelNoise;
        }
        else
        {
            NoiseClip = true;
        }
    }

     void DownNoise()
    {
        if (noise > 0)
        {
            noise -= (Time.deltaTime * downNoiseInSecond) / maxLevelNoise;
        }
    }
}
