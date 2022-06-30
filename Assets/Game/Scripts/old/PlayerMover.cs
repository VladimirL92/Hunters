using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    private GameObject NoiseBar;
    [SerializeField]
    private string noiseBarName;

    private void Start()
    {
        NoiseBar = GameObject.Find(noiseBarName);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
            transform.up = Vector3.up;
            NoiseBar.SendMessage("Walk");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Vector3.up * Time.deltaTime * speed;
            transform.up = Vector3.down;
            NoiseBar.SendMessage("Walk");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right* Time.deltaTime * speed;
            transform.up = Vector3.right;
            NoiseBar.SendMessage("Walk");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Vector3.right * Time.deltaTime * speed;
            transform.up = Vector3.left;
            NoiseBar.SendMessage("Walk");
        }
        else
        {
            NoiseBar.SendMessage("Stoped");
        }

    }


}
