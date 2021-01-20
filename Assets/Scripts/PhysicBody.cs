using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBody : MonoBehaviour
{
    [SerializeField]
    Vector3 m_velocity = Vector3.zero;

    public void Integrate(float _dt) {
         transform.position += m_velocity * _dt;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhysicManager.Instance.RegisterPhysicBody(this); 
    }
}
