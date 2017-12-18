using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private bool m_isStable = true;
    public BoundsCollisionDetector colDetector;

    ContactPoint2D[] contacts = new ContactPoint2D[10];
    Rigidbody2D rig;

    private bool m_isPlayingImpactAnim = false;
    private int m_remainImpactTime;
    public GameObject impactPlaceHolder;

    public bool IsStable
    {
        get
        {
            return m_isStable;
        }

        set
        {
            if (value != m_isStable)
            {
                //print(name + " change to stable " + value + " with impact speed " + rig.velocity);
            }
            m_isStable = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (colDetector)
        {
            colDetector.onContactWithOneSide = (_trans) =>
            {
                //if (_trans.position.y < transform.position.y)
                //    print(name + " Land on " + _trans.name);
            };

            colDetector.onExitContactWithOneSide = (_trans) =>
            {
                //if (_trans.position.y < transform.position.y)
                //    print("No longer land on " + _trans.name);
            };
        }
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1 && hasBeenMarkedDestroyed == false)
        {
            //print("Fall from desk " + name);
            StartCoroutine(DestroyGameobject());
        }
        CheckContacts();

        float rot = (transform.localEulerAngles.z + 90) % 360;
        if (rot > 315 || rot < 45)
        {
            impactPlaceHolder.transform.localPosition = new Vector3(0.5f, 0f, 0);
            impactPlaceHolder.transform.localRotation = Quaternion.Euler(-180, 70, 0);
        }
        else if(rot > 225)
        {
            impactPlaceHolder.transform.localPosition = new Vector3(0, 0.25f, 0);
            impactPlaceHolder.transform.localRotation = Quaternion.Euler(110, 0, 0);
        }
        else if(rot > 135)
        {
            impactPlaceHolder.transform.localPosition = new Vector3(-0.5f, 0f, 0);
            impactPlaceHolder.transform.localRotation = Quaternion.Euler(-180, 75, 0);
        }
        else
        {
            impactPlaceHolder.transform.localPosition = new Vector3(0, -0.49f, 0);
            impactPlaceHolder.transform.localRotation = Quaternion.Euler(-110, 0, 0);
        }
    }

    private void CheckContacts()
    {
        int contactNum = rig.GetContacts(contacts);
        if (contactNum < 2)
        {
            IsStable = false;
            return;
        }

        for (int i = 0; i < contacts.Length; ++i)
        {
            // If the contacting rigidbody is under this transform
            if (contacts[i].rigidbody != null
                && contacts[i].rigidbody.position.y < transform.position.y)
            {
                // Search if there's another step point
                for (int j = i + 1; j < contacts.Length; ++j)
                {
                    if (contacts[j].rigidbody != null
                        && contacts[j].rigidbody == contacts[i].rigidbody)
                    {
                        if (IsStable == false)
                        {
                            float impact =
                                (contacts[j].relativeVelocity * contacts[j].normalImpulse + contacts[i].relativeVelocity * contacts[i].normalImpulse).magnitude;
                            if (impact > 9f)
                            {
                                print(name + " get impluse " +
                                     impact + " from " + contacts[i].rigidbody.name);

                                if(m_isPlayingImpactAnim == false)
                                {
                                    StartCoroutine(PlayImpactAnim());
                                }
                                else
                                {
                                    m_remainImpactTime += 20;
                                }
                            }
                            IsStable = true;
                        }
                        return;
                    }
                }
            }
        }
        IsStable = false;
        return;
    }

    private IEnumerator PlayImpactAnim()
    {
        m_isPlayingImpactAnim = true;
        m_remainImpactTime = 100;
        impactPlaceHolder.SetActive(true);
        //print("Set active " + name);
        while (m_remainImpactTime > 0)
        {
            m_remainImpactTime--;

            yield return null;
        }

        impactPlaceHolder.SetActive(false);
        //print("Impact finished");
        m_isPlayingImpactAnim = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Desk"))
        {
            //print("Fall on the desk " + name);
            StartCoroutine(DestroyGameobject());
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.transform.position.y < transform.position.y)
    //        isStable = false;
    //}

    private IEnumerator DestroyGameobject()
    {
        hasBeenMarkedDestroyed = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

        BlockManager.Instance.blocks.Remove(this);
        BlockManager.Instance.UpdateBottomBlock();
    }
}
