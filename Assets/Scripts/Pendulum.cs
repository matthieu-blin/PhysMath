using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{

    public Transform m_transform;
    public Vector3 ComputeConstraint()
    {
        Vector3 point = transform.position;// + new Vector3(-m_transform.position.x, -m_transform.position.y, 0);

        float c = (Vector3.Dot(-m_body.Force, point) - m_body.Masse * Vector3.Dot(m_body.Velocity, m_body.Velocity))
                    / Vector3.Dot(point, point);
        return c * point;
    }

    PhysicBody m_body;
    // Start is called before the first frame update
    void Start()
    {
        m_body =GetComponent<PhysicBody>();
    }

    // Update is called once per frame
    void Update()
    {
        Gizmos.DrawSphere(new Vector3(2, 2, 0), 1f);
        
    }
}
