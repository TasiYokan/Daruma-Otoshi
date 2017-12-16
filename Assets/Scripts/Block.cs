using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Purple,
    Grey
}

public class Block : MonoBehaviour
{
    private bool hasBeenMarkedDestroyed = false;
    public BlockType blockType;

    public bool isStable = true;
    public BoundsCollisionDetector colDetector;

    // Use this for initialization
    void Start()
    {
        if (colDetector)
        {
            colDetector.onContactWithOneSide = (_trans) =>
            {
                if (_trans.position.y < transform.position.y)
                    print(name + " Land on " + _trans.name);
            };

            colDetector.onExitContactWithOneSide = (_trans) =>
            {
                //if (_trans.position.y < transform.position.y)
                //    print("No longer land on " + _trans.name);
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1 && hasBeenMarkedDestroyed == false)
        {
            //print("Fall from desk " + name);
            StartCoroutine(DestroyGameobject());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Desk"))
        {
            //print("Fall on the desk " + name);
            StartCoroutine(DestroyGameobject());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.position.y < transform.position.y)
            isStable = false;
    }

    private IEnumerator DestroyGameobject()
    {
        hasBeenMarkedDestroyed = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

        BlockManager.Instance.blocks.Remove(this);
        BlockManager.Instance.UpdateBottomBlock();
    }
}
