using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CompositeBlock : MonoBehaviour
{
    public List<BlockShape> blockShapes;

    // Use this for initialization
    void Start()
    {
        GetAllShapesInChildren();
        SetAllChildShapeInOneLayer();
    }

    public void GetAllShapesInChildren()
    {
        blockShapes = GetComponentsInChildren<BlockShape>().ToList();
    }

    public void SetAllChildShapeInOneLayer()
    {
        blockShapes.Sort((lhs, rhs) => (lhs.transform.localPosition.y - rhs.transform.localPosition.y).Sgn());
        for (int i = 0; i < blockShapes.Count; ++i)
        {
            blockShapes[i].BaseOrder = blockShapes[0].BaseOrder + i;
            blockShapes[i].transform.localPosition = blockShapes[i].transform.localPosition.SetZ(blockShapes[0].transform.localPosition.z);
        }
    }

    public void SetupFromBlock(Block _block)
    {
        if (_block.GetComponent<CompositeBlock>() == null)
        {
            GetComponent<Rigidbody2D>().mass = 1;
            CopyComponent(_block, this.gameObject);
            BlockManager.Instance.blocks.Remove(_block);
            Destroy(_block);

            Destroy(_block.GetComponent<CompositeCollider2D>());
            Destroy(_block.GetComponent<Rigidbody2D>());
            CopyComponent(_block.GetComponent<BoxCollider2D>(), this.gameObject);
            Destroy(_block.GetComponent<BoxCollider2D>());
            PolygonCollider2D[] polyCols = _block.GetComponentsInChildren<PolygonCollider2D>();
            for (int i = 0; i < polyCols.Length; ++i)
            {
                Destroy(polyCols[i]);
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().mass = 0;
            foreach (var blockShape in _block.GetComponent<CompositeBlock>().blockShapes)
            {
                GetComponent<Rigidbody2D>().mass += 1;
                CopyComponent(blockShape.GetComponent<BoxCollider2D>(), this.gameObject);
                Destroy(blockShape.GetComponent<BoxCollider2D>());
                Destroy(blockShape.GetComponent<CompositeBlock>());
            }
        }

        GetComponent<Block>().blockType = _block.blockType;

        _block.transform.SetParent(this.transform);

        GetAllShapesInChildren();
        SetAllChildShapeInOneLayer();


        BlockManager.Instance.blocks.Add(this.GetComponent<Block>());
        BlockManager.Instance.UpdateBottomBlock();
    }

    public void CombineAnotherBlock(Block _block)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }

}
