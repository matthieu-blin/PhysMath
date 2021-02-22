using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : Constraint
{

    public override void ComputeConstraint()
    {
                m_body.NVelocity = Vector3.zero;
                m_body.NAngularVelocity = 0 ;
    }

  }
