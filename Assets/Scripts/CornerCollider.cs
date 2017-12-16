using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
public class CornerCollider : MonoBehaviour
{
    public BoundsCollisionDetector parentDetctor;
    public int id;
    private BoxCollider2D m_collider;

    public BoxCollider2D Collider
    {
        get
        {
            if (m_collider == null)
            {
                m_collider = GetComponent<BoxCollider2D>();
            }
            return m_collider;
        }

        private set
        {
            m_collider = value;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    public void Init(BoundsCollisionDetector _parent, int _id, Vector3 _pos)
    {
        parentDetctor = _parent;
        id = _id;
        transform.position = _pos;
        Collider.size = Vector2.one * 0.1f;
        Collider.isTrigger = true;

        //GetComponent<Rigidbody2D>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == false)
            parentDetctor.RegisterNewContact(collision.transform, id);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger == false)
            parentDetctor.UnregisterContact(collision.transform, id);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    print("is stay in " + collision.transform.name);
    //}
}
