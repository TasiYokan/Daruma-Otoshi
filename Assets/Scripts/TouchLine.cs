using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLine : MonoBehaviour
{
    public Transform startCircle;
    public Transform curCircle;
    public Transform firstCirlce;
    public Transform secondCircle;
    public Transform thirdCircle;

    private bool m_isVisible;
    public float alpha;
    public MeshRenderer renderer;

    public bool IsVisible
    {
        get
        {
            return m_isVisible;
        }

        set
        {
            if (m_isVisible != value)
            {
                Action setVisible = () =>
                {
                    startCircle.gameObject.SetActive(value);
                    curCircle.gameObject.SetActive(value);
                    firstCirlce.gameObject.SetActive(value);
                    secondCircle.gameObject.SetActive(value);
                    //thirdCircle.gameObject.SetActive(m_isVisible);
                };

                if (value == true)
                {
                    StopAllCoroutines();
                    setVisible();
                    StartCoroutine(AlphaFadeIn());
                    StartCoroutine(ScaleInTween(startCircle, 1.7f));
                    StartCoroutine(ScaleInTween(firstCirlce, 0.7f, 1.5f));
                    StartCoroutine(ScaleInTween(secondCircle, 0.7f, 1.5f));
                    StartCoroutine(ScaleInTween(curCircle, 1.1f));
                }
                else
                {
                    StopAllCoroutines();
                    Vector3 dir = startCircle.position - curCircle.position;
                    Vector3 dst = startCircle.position + dir * 0.5f;
                    StartCoroutine(ScaleOutTween(startCircle, dst));
                    StartCoroutine(ScaleOutTween(firstCirlce, dst));
                    StartCoroutine(ScaleOutTween(secondCircle, dst));
                    StartCoroutine(ScaleOutTween(curCircle, dst));

                    StartCoroutine(AlphaFadeOut(setVisible));
                }

            }

            m_isVisible = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = curCircle.position - startCircle.position;
        firstCirlce.position = 2f / 3 * offset + startCircle.position;
        secondCircle.position = 1f / 3 * offset + startCircle.position;
        //thirdCircle.position = 1f / 4 * offset + startCircle.position;
    }

    IEnumerator ScaleInTween(Transform _trans, float _scale, float _speed = 1)
    {
        _trans.localScale = Vector3.one * 0.5f * _scale;
        while (_trans.localScale.x < 0.9f * _scale)
        {
            _trans.localScale = Vector3.Lerp(_trans.localScale, Vector3.one * _scale, 2f * Time.deltaTime * _speed);
            yield return null;
        }
    }

    IEnumerator ScaleOutTween(Transform _trans, Vector3 _dst)
    {
        while (_trans.localScale.x > 0.1f)
        {
            _trans.position = Vector3.Lerp(_trans.position, _dst, 9f * Time.deltaTime);
            _trans.localScale = Vector3.Lerp(_trans.localScale, Vector3.zero, 2f * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator AlphaFadeIn()
    {
        Material mat = renderer.material;
        float alpha = 0f;
        mat.SetFloat("_Alpha", alpha);
        while (alpha < 0.95f)
        {
            alpha = Mathf.Lerp(alpha, 1f, 3f * Time.deltaTime);
            mat.SetFloat("_Alpha", alpha);
            yield return null;
        }
    }

    IEnumerator AlphaFadeOut(Action _callback)
    {
        Material mat = renderer.material;
        float alpha = 1f;
        mat.SetFloat("_Alpha", alpha);
        while (alpha > 0.1f)
        {
            alpha = Mathf.Lerp(alpha, 0f, 1f * Time.deltaTime);
            mat.SetFloat("_Alpha", alpha);
            yield return null;
        }

        if (_callback != null)
            _callback();
    }
}
