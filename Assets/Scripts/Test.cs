using System;
using System.Collections;
using System.Collections.Generic;
using Bk;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    public PhysicRect m_rect;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDrawGizmos()
    {
        Vector3 dir = (transform.position - m_rect.transform.position).normalized;
        Vector3 normal;
        Vector3 point = Collisions.FindPointOnEdge(dir, m_rect, out normal);
        Bk.Gizmos.DrawArrow((point), dir, Color.blue);
        // Bk.Gizmos.DrawArrow(m_rect.transform.position, dir, Color.red);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
