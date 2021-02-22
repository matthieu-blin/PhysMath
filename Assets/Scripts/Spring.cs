using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Constraint
{

    public Transform m_transform;
    public float m_stiffness = 0.2f;
    public float m_dampingCoefficient = 1.4f;
    public override void ComputeConstraint()
    {
        Vector3  dist = transform.position - m_transform.position;
        m_body.NForce =  -dist * m_stiffness - m_body.Velocity *  m_dampingCoefficient ;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(m_transform.position, new Vector3(0.2f,0.2f, 0.2f));
    }

  }
