using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicRect : PhysicShape
{
    [SerializeField]
    float m_Width = 1;
    [SerializeField]
    float m_Height = 1;
    public override float GetInertia()
    {
        return m_physicBody.Masse * (m_Width * m_Width + m_Height * m_Height) / 12;
    }
    public override Vector2 GetCentroid()
    {
        return Vector2.zero;
    }

    public float Height { get => m_Height; }
    public float Width { get => m_Width; }

    public Vector2 this[int index]
    {
        get => GetPoint(index);
    }

    Vector2 GetPoint(int index)
    {
        float halfH = m_Height / 2;
        float halfW = m_Width / 2;
        switch (index)
        {
            case 0:
                return transform.TransformPoint(new Vector3(-halfW, -halfH, 0));
            case 1:
                return transform.TransformPoint(new Vector3(+halfW, -halfH, 0));
            case 2:
                return transform.TransformPoint(new Vector3(+halfW, +halfH, 0));
            case 3:
                return transform.TransformPoint(new Vector3(-halfW, +halfH, 0));
        }
        return Vector2.zero;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube( Vector3.zero, new Vector3(m_Width , m_Height));        
    }
}

