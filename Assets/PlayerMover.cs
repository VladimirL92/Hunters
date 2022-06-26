using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private float noise;
    public Image NoiseBar;


    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
            transform.up = Vector3.up;
            NoiseUp();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Vector3.up * Time.deltaTime * speed;
            transform.up = Vector3.down;
            NoiseUp();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right* Time.deltaTime * speed;
            transform.up = Vector3.right;
            NoiseUp();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Vector3.right * Time.deltaTime * speed;
            transform.up = Vector3.left;
            NoiseUp();
        }
        else
        {
            NoiseDown();
        }

        NoiseBar.fillAmount = noise;

    }

    void NoiseUp()
    {
        if (noise < 1)
        {
            noise += Time.deltaTime * 3;
        }
    }
    void NoiseDown()
    {
        if ( noise > 0)
        {
            noise -= Time.deltaTime * 0.5f;
        }
    }
}
