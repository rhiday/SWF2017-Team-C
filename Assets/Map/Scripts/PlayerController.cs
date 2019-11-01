//Simple controller to move player in test using keyboard (Player object must have rigidbody and this script)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float Speed = 0f;
    private float movex = 0f;
    private float movey = 0f;

    // Use this for initialization
    void Start () {
		
	}

    void FixedUpdate()
    {
        movex = Input.GetAxis("Horizontal");
        movey = Input.GetAxis("Vertical");
        GetComponent<Rigidbody>().velocity = new Vector3(movex * Speed, 0f, movey * Speed);
    }
        // Update is called once per frame
        //void Update () {
        //       var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        //       var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        //       //transform.Rotate(0, x, 0);
        //       //transform.Translate(0, 0, z);
        //       transform.ve
        //   }
    }
