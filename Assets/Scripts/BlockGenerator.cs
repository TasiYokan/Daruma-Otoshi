using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
	public int minLayer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			GenerateNewBlock();
		}
    }

    private void GenerateNewBlock()
    {
		minLayer--;
        GameObject block = GameObject.Instantiate(Resources.Load("block") as GameObject, Vector3.zero, Quaternion.identity, this.transform);
		block.GetComponent<SpriteRenderer>().sortingOrder = minLayer;
    }
}
