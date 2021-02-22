using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsion : Constraint
{
    public override void ComputeConstraint()
    {
        foreach (var col in PhysicManager.Instance.m_currentCollision)
        {
            if (col.m_body == m_body)
            {
                //first rollback positions to contact pos
                col.m_body.transform.position += - (Vector3)col.Force;

                float angle = 2 * Mathf.Acos(Vector3.Dot(m_body.Velocity.normalized, col.Force.normalized));

                Vector3 newV = (2 * col.m_bodyA.Masse * col.m_bodyA.Velocity +
                                (col.m_body.Masse - col.m_bodyA.Masse) * m_body.Velocity)
                               / (col.m_body.Masse + col.m_bodyA.Masse);

                newV = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.back) * newV;
                //elastic contact
                m_body.NVelocity = Vector3.zero;
                if (newV.magnitude > 0.1)
                    m_body.NVelocity = newV;

                var shape = GetComponent<PhysicShape>();
                if (shape && shape.enabled)
                {
                    var arm = (Vector2) col.Point - shape.GetCentroid();
                    float torque = (arm.x * newV.y - arm.y * newV.x) ;
                    float angularAcceleration = torque / shape.GetInertia();
                    m_body.NAngularVelocity = angularAcceleration;
                }
            }
        }
    }
}