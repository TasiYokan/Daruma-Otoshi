﻿using System.Collections;
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
        GameObject block = GameObject.Instantiate(Resources.Load("ComBlock") as GameObject, Vector3.up * 0.5f, Quaternion.identity, this.transform);
		block.GetComponent<SpriteRenderer>().sortingOrder = minLayer;

        BlockManager.Instance.blocks.Add(block.GetComponent<Block>());
        BlockManager.Instance.UpdateBottomBlock();
    }
}
