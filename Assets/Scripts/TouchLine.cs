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

    public bool IsVisible
    {
        get
        {
            return m_isVisible;
        }

        set
        {
            m_isVisible = value;
            startCircle.gameObject.SetActive(m_isVisible);
            curCircle.gameObject.SetActive(m_isVisible);
            firstCirlce.gameObject.SetActive(m_isVisible);
            secondCircle.gameObject.SetActive(m_isVisible);
            //thirdCircle.gameObject.SetActive(m_isVisible);
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
}
