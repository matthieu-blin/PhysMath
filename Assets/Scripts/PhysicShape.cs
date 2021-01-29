using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicBody))]
public class PhysicShape : MonoBehaviour
{
    protected PhysicBody m_physicBody = null;
    protected virtual void Start()
    {
        m_physicBody = GetComponent<PhysicBody>();
    }
    public virtual float GetInertia() { return 0; }
    public virtual Vector2 GetCentroid() { return Vector2.zero; }
}
