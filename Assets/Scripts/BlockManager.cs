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
            // In case it has been destroied.
            if (x.gameObject == null || y.gameObject == null)
                return 1;
            return (x.transform.position.y - y.transform.position.y).Sgn();
        });

        for(int i = 0; i< blocks.Count;++i)
        {
            blocks[i].transform.GetComponentInChildren<BlockShape>().BaseOrder = i;
        }
        if (blocks.Count == 0)
        {
            print("no more blocks");
            GenerateNewSet();
        }
    }

    private void GenerateNewSet()
    {
        for (int i = 0; i < 6; ++i)
        {
            GenerateNewBlock(i);
        }
    }

    private void GenerateNewBlock(int _id)
    {
        string blockType;
        blockType = Random.Range(0f, 1f) > 0.5f ? "ComBlock_Purple" : "ComBlock_Grey";
        GameObject block = GameObject.Instantiate(Resources.Load(blockType) as GameObject, Vector3.up * (0.75f * _id - 1.2f), Quaternion.identity, this.transform);
        block.name = "ComBlock " + _id;
        block.GetComponent<BlockShape>().BaseOrder = _id;

        BlockManager.Instance.blocks.Add(block.GetComponent<Block>());
        BlockManager.Instance.UpdateBottomBlock();
    }
}
