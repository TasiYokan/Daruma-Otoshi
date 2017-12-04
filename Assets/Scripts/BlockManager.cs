using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
	public List<Block> blocks;
    private static BlockManager m_instance;

    public static BlockManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType<BlockManager>();
            return m_instance;
        }
    }

    // Use this for initialization
    void Start()
    {
		blocks = GetComponentsInChildren<Block>().ToList();
        UpdateBottomBlock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBottomBlock()
    {
        blocks.Sort((x, y) =>
        {
            return (x.transform.position.y - y.transform.position.y).Sgn();
        });
    }
}
