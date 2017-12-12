using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndHit : MonoBehaviour
{
    private Vector3 startPos;
    public SpriteRenderer hammerSprite;
    private bool m_isMoving = false;
    public GameObject StartCircle;
    public GameObject curCircle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCircle.SetActive(true);
            StartCircle.transform.position = startPos.SetZ(-2);
            curCircle.SetActive(true);
            curCircle.transform.position = StartCircle.transform.position;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            print("Release force " + (startPos - Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            FindTargetBlock();
            StartCircle.SetActive(false);
            curCircle.SetActive(false);
        }

        if (Input.GetMouseButton(0))
        {
            Debug.DrawLine(startPos, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
            //hammerSprite.enabled = true;
            hammerSprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).SetZ(0);
            curCircle.transform.position = hammerSprite.transform.position;
        }
        else if (m_isMoving == false)
        {
            hammerSprite.enabled = false;
        }
    }

    private void FindTargetBlock()
    {
        Vector3 forceDir = startPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(startPos, forceDir);
        // Debug.DrawLine(startPos, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null && hit.transform.CompareTag("Block"))
        {
            print("hit " + hit.transform.name);

            StartCoroutine(SwapHammerTo(hit.point - Vector2.left * 0.85f, () =>
              {
                  hit.transform.GetComponent<Rigidbody2D>().AddForce(forceDir * 3, ForceMode2D.Impulse);
              }));
        }
    }

    private IEnumerator SwapHammerTo(Vector3 _targetPos, Action _callback)
    {
        m_isMoving = true;
        while ((hammerSprite.transform.position - _targetPos).SetZ(0).magnitude > 0.1f)
        {
            Vector3 dis = (_targetPos - hammerSprite.transform.position).SetZ(0);
            print("hammer pos " + hammerSprite.transform.position);
            print("dis " + dis.magnitude);
            hammerSprite.transform.Translate(dis * 0.5f);

            yield return null;
        }

        m_isMoving = false;
        if (_callback != null)
            _callback();
    }
}
