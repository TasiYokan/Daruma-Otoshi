using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShape : MonoBehaviour
{
    public Transform top;
    public Transform topInverse;
    public Transform bottom;
    public Transform bottomInverse;
    public Transform body;

    public float scale;
    private float lastScale;

    private float initTopY;
    private float initTopInverseY;
    private float initBottomY;
    private float initBottomInverseY;

    [SerializeField]
    private int m_baseOrder;

    public bool underComposite;

    public int BaseOrder
    {
        get
        {
            return m_baseOrder;
        }

        set
        {
            m_baseOrder = value;
            UpdateAllSpritesOrder();
        }
    }

    // Use this for initialization
    void Start()
    {
        initTopY = top.localPosition.y;
        initTopInverseY = topInverse.localPosition.y;
        initBottomY = bottom.localPosition.y;
        initBottomInverseY = bottomInverse.localPosition.y;

        BaseOrder = m_baseOrder;
        if (underComposite == false)
            SetDepthBasedOnY();
    }

    public void SetDepthBasedOnY()
    {
        transform.localPosition = transform.localPosition.SetZ(-transform.localPosition.y * 0.1f);
    }

    private void UpdateAllSpritesOrder()
    {
        top.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 4;
        topInverse.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 2;
        body.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 1;
        bottom.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 4;
        bottomInverse.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 2;
    }

    // Update is called once per frame
    void Update()
    {
        //print((transform.localEulerAngles.z + 90) % 360 > 180);
        float amplify = 0.06f;

        scale = Mathf.Sin(((transform.localEulerAngles.z + 90) % 360) / 180f * Mathf.PI); //Mathf.Clamp(scale, -1, 1);
        top.localScale = top.localScale.SetY(Mathf.Clamp(scale, 0, 1));
        top.localPosition = top.localPosition.SetY(initTopY + top.localScale.y * amplify);

        topInverse.localScale = topInverse.localScale.SetY(Mathf.Clamp(-scale, 0, 1));
        topInverse.localPosition = topInverse.localPosition.SetY(initTopInverseY + top.localScale.y * amplify);

        bottom.localScale = bottom.localScale.SetY(Mathf.Clamp(scale, 0, 1));
        bottom.localPosition = bottom.localPosition.SetY(initBottomY - top.localScale.y * amplify);

        bottomInverse.localScale = bottomInverse.localScale.SetY(Mathf.Clamp(-scale, 0, 1));
        bottomInverse.localPosition = bottomInverse.localPosition.SetY(initBottomInverseY - top.localScale.y * amplify);


        if (scale.Sgn() * lastScale.Sgn() < 0)
        {
            //print("inverse");
            top.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 3 + scale.Sgn();
            topInverse.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 3 - scale.Sgn();
            bottom.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 3 + scale.Sgn();
            bottomInverse.GetComponent<SpriteRenderer>().sortingOrder = m_baseOrder * 5 + 3 - scale.Sgn();
        }
        lastScale = scale;

        // For test only
        //transform.Rotate(Vector3.forward, 1f);
    }
}
