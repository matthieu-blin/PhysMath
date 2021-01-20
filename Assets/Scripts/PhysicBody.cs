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

    public void Integrate(float _dt) {
        m_velocity += m_force / m_masse * _dt;
        transform.position += m_velocity * _dt;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhysicManager.Instance.RegisterPhysicBody(this); 
    }
}
