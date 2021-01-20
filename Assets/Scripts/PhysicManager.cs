using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicManager : MonoBehaviour
{
    public static PhysicManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    List<PhysicBody> m_physicBodies = new List<PhysicBody>();

    public void RegisterPhysicBody(PhysicBody _obj)
    {
        m_physicBodies.Add(_obj);
    }

    void FixedUpdate()
    {
        foreach(var po in m_physicBodies)
        {
            po.Integrate(Time.fixedDeltaTime);
        }
    }
}
