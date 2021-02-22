using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBody : MonoBehaviour
{
    Vector3 m_acceleration = Vector3.zero;
    [SerializeField]
    public Vector3 m_force = Vector3.zero;
    [SerializeField]
    public Vector3 m_velocity = Vector3.zero;
    
    [SerializeField]
    float m_masse = 1;
    [SerializeField]
    public Vector2 m_ApplicationPoint = new Vector2(0, 0);
    

    [SerializeField]
    float m_angle = 0f;
    [SerializeField]
    float m_angularVelocity = 0f;

    public float Masse { get => m_masse;  }
    public Vector3 Force { get => m_force;  }
    public Vector3 NForce { get ; set; }
    
    
    public Vector3 Velocity { get => m_velocity;  } 
     public Vector3 NVelocity { get ; set; } 
    public float AngularVelocity { get => m_angularVelocity; } 
     public float NAngularVelocity { get; set; } = 0f;
     

    public void Integrate(float _dt) {
        Vector3 force = m_force + NForce;
        m_acceleration = force / m_masse;
        m_velocity = NVelocity;
        m_velocity += m_acceleration  * _dt;
        transform.position += m_velocity * _dt;
        
        m_angularVelocity = NAngularVelocity;
        var shape = GetComponent<PhysicShape>();
        if (shape && shape.enabled)
        {
            var arm = (Vector2)transform.TransformPoint(m_ApplicationPoint) - shape.GetCentroid();
            float torque = arm.x * m_force.y - arm.y * m_force.x;
            float angularAcceleration = torque / shape.GetInertia();
            m_angularVelocity +=  angularAcceleration * _dt;
        }
        m_angle += AngularVelocity * _dt;
        transform.RotateAround(shape.GetCentroid(), new Vector3(0, 0, 1), m_angle);
        
        
        NVelocity = m_velocity;
        NAngularVelocity = m_angularVelocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhysicManager.Instance.RegisterPhysicBody(this); 
    }
}
