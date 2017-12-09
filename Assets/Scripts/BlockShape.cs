using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShape : MonoBehaviour
{
    public Transform top;
    public Transform topInverse;
    public Transform bottom;
    public Transform bottomInverse;

    public float scale;
    private float lastScale;

    private float initTopY;
    private float initTopInverseY;
    private float initBottomY;
    private float initBottomInverseY;

    // Use this for initialization
    void Start()
    {
        initTopY = top.localPosition.y;
        initTopInverseY = topInverse.localPosition.y;
        initBottomY = bottom.localPosition.y;
        initBottomInverseY = bottomInverse.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        print((transform.localEulerAngles.z + 90) % 360 > 180);
        float amplify = 0.05f;

        scale = Mathf.Sin(((transform.localEulerAngles.z + 90) % 360) / 180f * Mathf.PI); //Mathf.Clamp(scale, -1, 1);
        top.localScale = top.localScale.SetY(Mathf.Clamp(scale, 0, 1));
        top.localPosition = top.localPosition.SetY(initTopY + Mathf.Abs(Mathf.Cos(transform.localEulerAngles.z / 180f * Mathf.PI) * amplify));

        topInverse.localScale = topInverse.localScale.SetY(Mathf.Clamp(-scale, 0, 1));
        topInverse.localPosition = topInverse.localPosition.SetY(initTopInverseY + Mathf.Abs(Mathf.Cos(transform.localEulerAngles.z / 180f * Mathf.PI) * amplify));

        bottom.localScale = bottom.localScale.SetY(Mathf.Clamp(scale, 0, 1));
        bottom.localPosition = bottom.localPosition.SetY(initBottomY + Mathf.Abs(Mathf.Cos(transform.localEulerAngles.z / 180f * Mathf.PI) * amplify));

        bottomInverse.localScale = bottomInverse.localScale.SetY(Mathf.Clamp(-scale, 0, 1));
        bottomInverse.localPosition = bottomInverse.localPosition.SetY(initBottomInverseY + Mathf.Abs(Mathf.Cos(transform.localEulerAngles.z / 180f * Mathf.PI) * amplify));


        if (scale.Sgn() * lastScale.Sgn() < 0)
        {
            print("inverse");
            top.GetComponent<SpriteRenderer>().sortingOrder = scale.Sgn();
            topInverse.GetComponent<SpriteRenderer>().sortingOrder = -scale.Sgn();
            bottom.GetComponent<SpriteRenderer>().sortingOrder = scale.Sgn();
            bottomInverse.GetComponent<SpriteRenderer>().sortingOrder = -scale.Sgn();
        }
        lastScale = scale;

        transform.Rotate(Vector3.forward, 1f);
    }
}
