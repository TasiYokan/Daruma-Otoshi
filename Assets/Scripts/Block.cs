using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private bool hasBeenMarkedDestroyed = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -1 && hasBeenMarkedDestroyed == false)
        {
            print("Fall from desk " + name);
            StartCoroutine(DestroyGameobject());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Desk"))
        {
            print("Fall on the desk " + name);
            StartCoroutine(DestroyGameobject());
        }
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
