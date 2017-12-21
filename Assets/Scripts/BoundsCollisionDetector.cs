using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class BoundsCollisionDetector : MonoBehaviour
{
    public float height = 1;
    public float width = 1;
    public CornerCollider[] cornerColliders;
    private Vector3 origin;

    private Dictionary<Transform, List<int>> contacts;
    public bool IsCollideWithOneSide;
    public List<Transform> transCollidedWithOneSide;
    public Action<Transform> onContactWithOneSide;
    public Action<Transform> onExitContactWithOneSide;

    public Dictionary<Transform, List<int>> Contacts
    {
        get
        {
            if (contacts == null)
                contacts = new Dictionary<Transform, List<int>>();
            return contacts;
        }

        set
        {
            contacts = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (cornerColliders == null)
            cornerColliders = new CornerCollider[4];

        origin = transform.position - new Vector3(width * 0.5f, height * 0.5f);
        for (int i = 0; i < 4; ++i)
        {
            if (cornerColliders[i] == null)
            {
                cornerColliders[i] =
                    (new GameObject("Collider (" + (i >> 1) + ", " + (i & 1) + ")")).AddComponent<CornerCollider>();
                cornerColliders[i].transform.SetParent(this.transform);
            }
            cornerColliders[i].Init(this, i, origin + new Vector3((i >> 1) * width, (i & 1) * height));
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying == false)
        {
            origin = transform.position - new Vector3(width * 0.5f, height * 0.5f);
            for (int i = 0; i < 4; ++i)
            {
                cornerColliders[i].transform.position = origin + new Vector3((i >> 1) * width, (i & 1) * height);
            }
        }
#endif
    }

    public void RegisterNewContact(Transform _trans, int _cornerId)
    {
        //print("Register " + _trans.name + " " + _cornerId);
        if (Contacts.ContainsKey(_trans) == false)
        {
            Contacts.Add(_trans, new List<int>() { _cornerId });
        }
        else
        {
            if (Contacts[_trans].Contains(_cornerId))
            {
                // Do nothing
            }
            else
            {
                Contacts[_trans].Add(_cornerId);
                // Find the index instead of the real value because the default one is 0
                // We should not find the corner not on the same side (after XOR, it should be 1 or 2)
                int anotherCornerId = Contacts[_trans].FindIndex(x => (x ^ _cornerId) % 3 > 0);
                if (anotherCornerId >= 0)
                {
                    if (transCollidedWithOneSide.Contains(_trans) == false)
                        transCollidedWithOneSide.Add(_trans);
                    IsCollideWithOneSide = transCollidedWithOneSide.Count > 0;

                    //print("New contact with one side " + _trans.name);
                    if (onContactWithOneSide != null)
                        onContactWithOneSide(_trans);
                }
            }
        }
    }

    public void UnregisterContact(Transform _trans, int _cornerId)
    {
        //print("Unregister " + _trans.name + " " + _cornerId);
        if (Contacts.ContainsKey(_trans) == false || Contacts[_trans].Contains(_cornerId) == false)
        {
            return;
        }
        else
        {
            Contacts[_trans].Remove(_cornerId);

            // Remove the transform if no more corner is contacting with it
            if (Contacts[_trans].Count == 0)
            {
                Contacts.Remove(_trans);
            }

            if (transCollidedWithOneSide.Contains(_trans))
            {
                // By default we shoudl remove the trans.
                bool shouldRemove = true;

                if (Contacts.ContainsKey(_trans))
                {
                    // If there's only one corner left, then trans definitely not more collided with bounds
                    for (int i = 1; i < Contacts[_trans].Count; ++i)
                    {
                        int anotherCornerId = Contacts[_trans].FindIndex(x => (x ^ Contacts[_trans][i]) % 3 > 0);
                        if (anotherCornerId >= 0)
                        {
                            shouldRemove = false;
                            break;
                        }
                    }
                }

                if(shouldRemove)
                {
                    transCollidedWithOneSide.Remove(_trans);
                    IsCollideWithOneSide = transCollidedWithOneSide.Count > 0;

                    //print("Remove contact with one side " + _trans.name);
                    if (onExitContactWithOneSide != null)
                        onExitContactWithOneSide(_trans);
                }
            }
        }
    }
}
