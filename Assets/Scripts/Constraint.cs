using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constraint : MonoBehaviour
{
    public virtual void ComputeConstraint()
    {
    }

    protected PhysicBody m_body;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_body = GetComponent<PhysicBody>();
    }
}
