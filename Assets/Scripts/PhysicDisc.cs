using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicDisc : PhysicShape
{
    [SerializeField]
    float m_Radius = 1;
    public override float GetInertia()
    {
        return 0.5f *  m_physicBody.Masse * m_Radius * m_Radius;
    }
    public override Vector2 GetCentroid()
    {
        return transform.position;
    }

    public float Radius { get => m_Radius;  }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero,  m_Radius);
    }

}
