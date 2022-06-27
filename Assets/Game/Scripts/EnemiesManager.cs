using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesManager : MonoBehaviour
{
    private float noise;

    public Image NoiseBar;


    private void Update()
    {
        NoiseBar.fillAmount = noise;
    }
    public void Walk()
    {
        if (noise < 1 )
        {
            noise += Time.deltaTime * 3 / 10;
        }
    }
    public void Stoped()
    {
        if (noise > 0)
        {
            noise -= Time.deltaTime * 0.5f / 10;
        }
    }

}
