using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : Constraint
{

    public Transform m_transform;
    public override void  ComputeConstraint()
    {
        Vector3 point = transform.position + new Vector3(-m_transform.position.x, -m_transform.position.y, 0);

        float c = (Vector3.Dot(-m_body.Force, point) - m_body.Masse * Vector3.Dot(m_body.Velocity, m_body.Velocity))
                    / Vector3.Dot(point, point);
        m_body.NForce = c * point;
    }
  
    // Update is called once per frame
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(m_transform.position, new Vector3(0.2f,0.2f, 0.2f));
        
    }
}
