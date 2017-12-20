using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CompositeBlock : MonoBehaviour
{
    List<BlockShape> blockShapes;

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
        for(int i = 0; i< blockShapes.Count; ++i)
        {
            blockShapes[i].BaseOrder = blockShapes[0].BaseOrder + i;
            blockShapes[i].underComposite = true;
            blockShapes[i].transform.localPosition = blockShapes[i].transform.localPosition.SetZ(blockShapes[0].transform.localPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
