using System.Collections.Generic;
using Bk;
using UnityEngine;
using Gizmos = UnityEngine.Gizmos;

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

    public List<Bk.Collision> m_currentCollision = new List<Bk.Collision>();


    private void OnDrawGizmos()
    {
        foreach (Bk.Collision col in m_currentCollision)
        {
            Bk.Gizmos.DrawArrow(col.Point, col.Force, Color.red);
        }
    }

    void FixedUpdate()
    {
        //Simulation
        foreach (var po in m_physicBodies)
        {
            po.Integrate(Time.fixedDeltaTime);
        }

        //Collision Detection
        m_currentCollision.Clear();
        foreach (var psrc in m_physicBodies)
        {
            foreach (var ptarget in m_physicBodies)
            {
                if (psrc == ptarget) continue;
                Bk.Collision col = Collisions.CheckCollision(psrc, ptarget);
                if (col != null)
                {
                    col.m_body = psrc;
                    col.m_bodyA = ptarget;
                    m_currentCollision.Add(col);
                }
            }
        }


        foreach (var po in m_physicBodies)
        {
            //Collision solver
            foreach (var constraint in po.GetComponents<Constraint>())
                constraint.ComputeConstraint();
        }
    }
}