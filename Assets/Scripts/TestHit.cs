using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHit : MonoBehaviour
{
	public float delay = 1;
	public float forceX = -20;
	public float forceY = 0;

    // Use this for initialization
    void Start()
    {
        Invoke("GetHit", delay);
    }

    private void GetHit()
    {
        print("get hit");
        GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
