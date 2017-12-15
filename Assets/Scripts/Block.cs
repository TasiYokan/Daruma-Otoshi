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

    // Use this for initialization
    void Start()
    {

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
        else if (collision.CompareTag("Block"))
        {
            //if (collision.transform.GetComponent<Block>().blockType == blockType)
            //{
                print("it's the same type " + blockType);
                if (isStable == false && collision.transform.position.y < transform.position.y)
                    StartCoroutine(WaitTillStable());
            //}
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

    private Rigidbody2D FindBlockBeneath()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.down * 0.8f).SetZ(0), Vector2.down);
        // Debug.DrawLine(startPos, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (hit.collider != null && hit.transform.CompareTag("Block"))
        {
            //print("hit " + hit.transform.name);

            return hit.rigidbody;
        }
        return null;
    }

    private IEnumerator WaitTillStable()
    {
        //yield return new WaitForSeconds(1);
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        //Collider2D[] colliders = new Collider2D[10];
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        int contactNum = rig.GetContacts(contacts);
        //int colliderNum = rig.GetContacts(colliders);
        int stepPoint = 0;
        Rigidbody2D rigidbodyBeneath;
        rigidbodyBeneath = FindBlockBeneath();
        if (rigidbodyBeneath == null)
            yield break;

        for (int i = 0; i < contacts.Length; ++i)
        {
            if (contacts[i].rigidbody == rigidbodyBeneath)
                stepPoint++;
        }
        while (stepPoint < 2)
        {
            yield return null;
            stepPoint = 0;
            contactNum = rig.GetContacts(contacts);
            rigidbodyBeneath = FindBlockBeneath();
            //colliderNum = rig.GetContacts(colliders);
            for (int i = 0; i < contacts.Length; ++i)
            {
                if (contacts[i].rigidbody == rigidbodyBeneath)
                    stepPoint++;
            }
        }
        print("finally " + stepPoint + " " + name);
        isStable = true;
    }
}
