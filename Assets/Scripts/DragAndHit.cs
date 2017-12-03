using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndHit : MonoBehaviour
{
    private Vector3 startPos;

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
        }
        else if (Input.GetMouseButtonUp(0))
        {
            print("Release force " + (startPos - Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            FindTargetBlock();
        }

        if (Input.GetMouseButton(0))
        {
            Debug.DrawLine(startPos, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
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
			hit.transform.GetComponent<Rigidbody2D>().AddForce(forceDir * 3, ForceMode2D.Impulse);
        }
    }
}
