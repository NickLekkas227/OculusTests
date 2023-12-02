using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : OVRGrabber
{
    protected override void Awake()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        { 
            m_grabVolumes = new Collider[] { collider };     
        }
        m_gripTransform = transform;

        base.Awake();
    }

    public void Grab()
    {
        GrabBegin();
    }

    public void Release()
    {
        GrabEnd();
    }

    public void Throw()
    {
        GrabbableRelease(Vector3.forward, Vector3.zero);
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.C))
        {
            Grab();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Release();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Throw();
        }
    }
}
