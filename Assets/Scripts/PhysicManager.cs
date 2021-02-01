using System;
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

    List<Collision> m_currentCollision = new List<Collision>();


    public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }

    private void OnDrawGizmos()
    {
        foreach (Collision col in m_currentCollision)
        {
            ForGizmo(col.Point, col.Force, Color.red);
        }
    }

    void FixedUpdate()
    {
        //Simulation
        foreach(var po in m_physicBodies)
        {
            po.Integrate(Time.fixedDeltaTime);
        }

        //Collision Detection
        m_currentCollision.Clear();
        foreach(var psrc in m_physicBodies)
        {
            foreach (var ptarget in m_physicBodies)
            {
                if (psrc == ptarget) continue;
                Collision col = CheckCollision(psrc, ptarget);
                if(col != null)
                {
                    m_currentCollision.Add(col);
                }
            }
        }
    }

    class Collision
    {
        public Collision (Vector2 _p, Vector2 _f) { Point = _p; Force = _f; }
        public readonly Vector2 Point;
        public readonly Vector2 Force;
    }

    Collision CheckCollision(PhysicBody _a, PhysicBody _b)
    {
        PhysicShape a = _a.GetComponent<PhysicShape>();
        PhysicShape b = _b.GetComponent<PhysicShape>();
        if (a == null || b == null)
            return  null;
        if(a is PhysicDisc && b is PhysicDisc)
        {
            return CheckDiscCollision(a as PhysicDisc, b as PhysicDisc);
        }
        if(a is PhysicRect && b is PhysicRect)
        {
            return CheckRectCollision(a as PhysicRect, b as PhysicRect);
        }
        if(a is PhysicRect && b is PhysicDisc)
        {
            return CheckRectDiscCollision(a as PhysicRect, b as PhysicDisc);
        }
        if(a is PhysicDisc && b is PhysicRect)
        {
            return CheckDiscRectCollision(a as PhysicDisc, b as PhysicRect);
        }
        return  null ;
    }

    Collision CheckDiscCollision(PhysicDisc _a, PhysicDisc _b)
    {
        Vector2 dist = _b.transform.position - _a.transform.position;
        float distanceMax = _a.Radius + _b.Radius;
        float currentdist = dist.magnitude;
        if (currentdist < distanceMax)
        {
            return new Collision(
                (Vector2)_a.transform.position + dist.normalized * _a.Radius,
                -dist.normalized * (distanceMax - currentdist));
        }
        return null;
    }

    Collision CheckRectDiscCollision(PhysicRect _a, PhysicDisc _b)
    {
        Vector2 pos = _b.transform.position ;
        //transform our problem into an axis aligned rectangle centered in 0,0
        Vector2 localPos = _a.transform.InverseTransformPoint(pos);

        // temporary variables to set edges for testing
        float testX = localPos.x;
        float testY = localPos.y;

        // which edge is closest?
        if (localPos.x < -_a.Width/2) testX = - _a.Width/2;      // test left edge
        else if (localPos.x >  + _a.Width/2 ) testX =  + _a.Width/2;   // right edge
        if (localPos.y <- _a.Height/2) testY =  - _a.Height/2;      // top edge
        else if (localPos.y >  +_a.Height/2) testY =  + _a.Height/2;   // bottom edge

        // get distance from closest edges
        float distX = localPos.x - testX;
        float distY = localPos.y - testY;
        float distance = Mathf.Sqrt((distX * distX) + (distY * distY));

        // if the distance is less than the radius, collision!
        if (distance <= _b.Radius)
        {
            return new Collision(
            _a.transform.position + (_b.transform.position - _a.transform.position).normalized /2, //should project on axis 
            (_b.transform.position - _a.transform.position).normalized * (_b.Radius - distance) );
        }
        return null;

    }

    Collision CheckDiscRectCollision(PhysicDisc _b, PhysicRect _a)
    {
        Vector2 pos = _b.transform.position;
        //transform our problem into an axis aligned rectangle centered in 0,0
        Vector2 localPos = _a.transform.InverseTransformPoint(pos);

        // temporary variables to set edges for testing
        float testX = localPos.x;
        float testY = localPos.y;

        // which edge is closest?
        if (localPos.x < -_a.Width/2) testX = -_a.Width/2;      // test left edge
        else if (localPos.x > _a.Width/2) testX = _a.Width/2;   // right edge
        if (localPos.y < -_a.Height/2) testY = -_a.Height/2;      // top edge
        else if (localPos.y > _a.Height/2) testY = _a.Height/2;   // bottom edge

        // get distance from closest edges
        float distX = localPos.x - testX;
        float distY = localPos.y - testY;
        float distance = Mathf.Sqrt((distX * distX) + (distY * distY));

        // if the distance is less than the radius, collision!
        if (distance <= _b.Radius)
        {
            return new Collision(
               _b.transform.position + (_a.transform.position - _b.transform.position).normalized * _b.Radius,
                (_a.transform.position - _b.transform.position).normalized * (_b.Radius - distance));
        }
        return null;

    }

    class Projection
    {
        public Projection(float _min, float _max) { Min = _min; Max = _max; }
        public float getOverlap(Projection other)
        {
            if (Max > other.Min)
            {
                return Max - Min; 
            }
            else if (Min > other.Max)
            {
                return Min - Max;
            }
            return -1;
        }

        readonly float Min;
        readonly float Max;
    }

    Collision CheckRectCollision(PhysicRect _a, PhysicRect _b)
    {
        //SAT quick implementation

        Func<PhysicRect, Vector2, Projection> project = (r, axis) => 
        {
            float min = float.MaxValue ;
            float max = 0;
            for (int i = 1; i < 4; i++)
            {
                // NOTE: the axis must be normalized to get accurate projections
                float p = Vector2.Dot(axis, r[i]);
                min  = Math.Min(p, min);
                max  = Math.Max(p, max);
            }
            return new Projection(min, max);
        };

        float overlap = float.MaxValue;
        Vector2 smallest = Vector2.zero;

        Vector2[] axes1 = new Vector2[4];
        axes1[0] = (_a[0] - _a[1]).normalized;
        axes1[1] = (_a[1] - _a[2]).normalized;
        //no need for others since we have a rect
        axes1[2] = (_b[0] - _b[1]).normalized;
        axes1[3] = (_b[1] - _b[2]).normalized;

        // loop over the axes1
        foreach(Vector2 axis in axes1)
        {
            // project both shapes onto the axis
            Projection p1 = project(_a, axis);
            Projection p2 = project(_b, axis);
            // do the projections overlap?
                // get the overlap
                float o = p1.getOverlap(p2);
            if(o >= 0 ) //overlapping
            { 
                // check for minimum
                if (o < overlap)
                {
                    // then set this one as the smallest
                    overlap = o;
                    smallest = axis;
                }
            }
        }
        return new Collision(
            _a.transform.position + (_a.transform.position - _b.transform.position), //should project on axis 
             smallest* overlap);
    }
}
