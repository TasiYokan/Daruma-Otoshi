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

    private ContactPoint2D[] contacts = new ContactPoint2D[10];
    private Rigidbody2D m_rig;

    private bool m_isPlayingImpactAnim = false;
    private int m_remainImpactTime;

    private List<GameObject> m_playingParticle;
    private List<BlockShape> m_shapes;
    private bool isUp = true;

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

    public List<GameObject> PlayingParticle
    {
        get
        {
            if (m_playingParticle == null)
                m_playingParticle = new List<GameObject>();
            return m_playingParticle;
        }
    }

    // Use this for initialization
    void Start()
    {
        m_rig = GetComponent<Rigidbody2D>();
        SetupProperties();
    }

    private void SetupProperties()
    {
        CleanExistingComponents();

        m_shapes = GetComponentsInChildren<BlockShape>().ToList();

        int mass = 0;
        m_shapes.Sort((x, y) =>
            (x.transform.localPosition.y - y.transform.localPosition.y).Sgn());

        for (int i = 0; i < m_shapes.Count; ++i)
        {
            m_shapes[i].BaseOrder = i;
            m_shapes[i].transform.localPosition = m_shapes[0].transform.localPosition.AddY(0.55f * i).SetX(m_shapes[i].transform.localPosition.x);
            BoxCollider2D boxCol = gameObject.AddComponent<BoxCollider2D>();
            boxCol.offset = m_shapes[i].transform.localPosition;
            boxCol.size = new Vector2(1.3f, 0.6f);
            boxCol.usedByComposite = true;
            mass++;
        }
        m_rig.mass = mass;
    }

    private void CleanExistingComponents()
    {
        foreach (var boxCollider in GetComponents<BoxCollider2D>().ToList())
        {
            Destroy(boxCollider);
        }
    }

    private void CombineAnotherBlock(Block _block)
    {
        foreach (var shape in _block.m_shapes)
        {
            shape.transform.SetParent(transform);
            shape.ParentTrans = transform;
        }
        SetupProperties();
        _block.CompletelyDestroy();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -2 && hasBeenMarkedDestroyed == false)
        {
            //print("Fall from desk " + name);
            StartCoroutine(DestroyGameobject());
        }
        CheckContacts();
        CheckDepthOrder();
    }

    private void CheckDepthOrder()
    {
        if ((transform.localEulerAngles.z + 90) % 360 > 180 && isUp)
        {
            isUp = false;
            m_shapes.Sort((x, y) =>
            -(x.transform.localPosition.y - y.transform.localPosition.y).Sgn());
            for (int i = 0; i < m_shapes.Count; ++i)
            {
                m_shapes[i].BaseOrder = i;
            }
        }
        else if ((transform.localEulerAngles.z + 90) % 360 < 180 && isUp == false)
        {
            m_shapes.Sort((x, y) =>
            (x.transform.localPosition.y - y.transform.localPosition.y).Sgn());
            for (int i = 0; i < m_shapes.Count; ++i)
            {
                m_shapes[i].BaseOrder = i;
            }
        }
    }

    private void CheckContacts()
    {
        // Incase rigidbody has been removed from child block
        if (m_rig == null)
            return;

        int contactNum = m_rig.GetContacts(contacts);
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
                            if (impact > 5f
                                || contacts[i].relativeVelocity.magnitude + contacts[j].relativeVelocity.magnitude < 0.01f)
                            {
                                //print(name + " get impluse " +
                                //     impact + " from " + contacts[i].rigidbody.name);

                                if (m_isPlayingImpactAnim == false)
                                {
                                    //StartCoroutine(PlayImpactAnim());
                                    StartCoroutine(PlayPointImpactAnim(contacts[i].point));
                                    StartCoroutine(PlayPointImpactAnim(contacts[j].point));

                                    if (contacts[i].rigidbody.CompareTag("Desk"))
                                    {
                                        print("Fall on the desk " + name);
                                        StartCoroutine(DestroyGameobject());
                                    }
                                    else if (contacts[i].rigidbody.GetComponent<Block>()
                                        && contacts[i].rigidbody.GetComponent<Block>().blockType == this.blockType)
                                    {
                                        print(name + " Fall on the same type with " + contacts[i].rigidbody.name + " " + contacts[i].rigidbody.GetComponent<Block>().blockType);
                                        //CompositeBlock comBlock = 
                                        //    (GameObject.Instantiate(Resources.Load("CompositeBlock"), transform.position, Quaternion.identity, transform.parent)
                                        //    as GameObject).GetComponent<CompositeBlock>();
                                        //comBlock.SetupFromBlock(this);
                                        if (CheckIfCombinable(contacts[i].rigidbody.GetComponent<Block>()))
                                        {
                                            CombineAnotherBlock(contacts[i].rigidbody.GetComponent<Block>());
                                        }
                                    }
                                }
                                else
                                {
                                    m_remainImpactTime += 20;
                                }
                            }
                            IsStable = true;
                        }
                        else if (contacts[i].relativeVelocity.magnitude + contacts[j].relativeVelocity.magnitude < 0.01f)
                        {
                            if (contacts[i].rigidbody.CompareTag("Desk"))
                            {
                                print("Fall on the desk " + name);
                                StartCoroutine(DestroyGameobject());
                            }
                            else if (contacts[i].rigidbody.GetComponent<Block>()
                                && contacts[i].rigidbody.GetComponent<Block>().blockType == this.blockType
                                && CheckIfCombinable(contacts[i].rigidbody.GetComponent<Block>()))
                            {
                                if (m_isPlayingImpactAnim == false)
                                {
                                    //StartCoroutine(PlayImpactAnim());
                                    StartCoroutine(PlayPointImpactAnim(contacts[i].point));
                                    StartCoroutine(PlayPointImpactAnim(contacts[j].point));
                                }
                                CombineAnotherBlock(contacts[i].rigidbody.GetComponent<Block>());
                            }
                        }
                        return;
                    }
                }
            }
        }
        IsStable = false;
        return;
    }

    private bool CheckIfCombinable(Block _block)
    {
        if (_block.IsStable == false)
            return false;

        if (Mathf.Abs(transform.localEulerAngles.z - _block.transform.localEulerAngles.z) < 5
            || Mathf.Abs(transform.localEulerAngles.z - _block.transform.localEulerAngles.z) > 355)
            return true;
        else
            return false;
    }

    private IEnumerator PlayPointImpactAnim(Vector3 _pos)
    {
        m_isPlayingImpactAnim = true;
        m_remainImpactTime = 100;
        GameObject particle = GameObject.Instantiate(Resources.Load("CircleSmoke"), _pos.SetZ(transform.localPosition.z), Quaternion.identity, null) as GameObject;
        PlayingParticle.Add(particle);
        particle.SetActive(true);
        //print("Set active " + name);
        while (m_remainImpactTime > 0)
        {
            m_remainImpactTime--;

            yield return null;
        }

        particle.SetActive(false);
        Destroy(particle.gameObject);
        //print("Impact finished");
        m_isPlayingImpactAnim = false;
    }

    private IEnumerator DestroyGameobject()
    {
        hasBeenMarkedDestroyed = true;
        yield return new WaitForSeconds(1);
        CompletelyDestroy();
    }

    private void CompletelyDestroy()
    {
        foreach (var particle in PlayingParticle)
        {
            Destroy(particle.gameObject);
        }
        Destroy(gameObject);

        BlockManager.Instance.blocks.Remove(this);
        BlockManager.Instance.UpdateBottomBlock();
    }
}
