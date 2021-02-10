using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBody : MonoBehaviour
{
    [SerializeField]
    Vector3 m_velocity = Vector3.zero;
    [SerializeField]
    Vector3 m_force = Vector3.zero;
    [SerializeField]
    float m_masse = 1;

    [SerializeField]
    Vector2 m_ApplicationPoint = new Vector2(0, 0);
    [SerializeField]
    float m_angularVelocity = 0f;
    [SerializeField]
    float m_angle = 0f;

    public float Masse { get => m_masse;  }
    public Vector3 Force { get => m_force;  }
    public Vector3 Velocity { get => m_velocity;  }

    public void Integrate(float _dt) {
        var constraint = GetComponent<Pendulum>();
        Vector3 force = m_force;
        if (constraint)
            force += constraint.ComputeConstraint();

        m_velocity += force   / m_masse * _dt;
        transform.position += m_velocity * _dt;
        var shape = GetComponent<PhysicShape>();
        if (shape != null)
        {
            var arm = m_ApplicationPoint - shape.GetCentroid();
            float torque = arm.x * m_force.y - arm.y * m_force.x;
            float angularAcceleration = torque / shape.GetInertia();
            m_angularVelocity = m_angularVelocity + angularAcceleration * _dt;
            m_angle += m_angularVelocity * _dt;
            transform.RotateAround(transform.TransformPoint(shape.GetCentroid()), new Vector3(0, 0, 1), m_angle);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PhysicManager.Instance.RegisterPhysicBody(this); 
    }
}
