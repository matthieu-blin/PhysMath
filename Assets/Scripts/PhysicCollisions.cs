using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bk
{
    public class Collision
    {
        public Collision(Vector2 _p, Vector2 _f)
        {
            Point = _p;
            Force = _f;
        }

        public PhysicBody m_bodyA;
        public PhysicBody m_body;
        public readonly Vector2 Point;
        public readonly Vector2 Force;
    }

    public static class Collisions
    {
        public static Collision CheckCollision(PhysicBody _a, PhysicBody _b)
        {
            PhysicShape a = _a.GetComponent<PhysicShape>();
            PhysicShape b = _b.GetComponent<PhysicShape>();
            if (a == null || b == null)
                return null;
            if (a is PhysicDisc && b is PhysicDisc)
            {
                return CheckDiscCollision(a as PhysicDisc, b as PhysicDisc);
            }

            if (a is PhysicRect && b is PhysicRect)
            {
                return  CustomSAT(a as PhysicRect, b as PhysicRect);
            }

            if (a is PhysicRect && b is PhysicDisc)
            {
                return CheckRectDiscCollision(a as PhysicRect, b as PhysicDisc);
            }

            if (a is PhysicDisc && b is PhysicRect)
            {
                return CheckDiscRectCollision(a as PhysicDisc, b as PhysicRect);
            }

            return null;
        }

        private static Collision CheckDiscCollision(PhysicDisc _a, PhysicDisc _b)
        {
            Vector2 dist = _b.transform.position - _a.transform.position;
            float distanceMax = _a.Radius + _b.Radius;
            float currentdist = dist.magnitude;
            if (currentdist < distanceMax)
            {
                return new Collision(
                    (Vector2) _a.transform.position + dist.normalized * _a.Radius,
                    -dist.normalized * (distanceMax - currentdist));
            }

            return null;
        }

        private static Collision CheckRectDiscCollision(PhysicRect _a, PhysicDisc _b)
        {
            Vector2 pos = _b.transform.position;
            //transform our problem into an axis aligned rectangle centered in 0,0
            Vector2 localPos = _a.transform.InverseTransformPoint(pos);

            // temporary variables to set edges for testing
            float testX = localPos.x;
            float testY = localPos.y;

            // which edge is closest?
            if (localPos.x < -_a.Width / 2) testX = -_a.Width / 2; // test left edge
            else if (localPos.x > +_a.Width / 2) testX = +_a.Width / 2; // right edge
            if (localPos.y < -_a.Height / 2) testY = -_a.Height / 2; // top edge
            else if (localPos.y > +_a.Height / 2) testY = +_a.Height / 2; // bottom edge

            // get distance from closest edges
            float distX = localPos.x - testX;
            float distY = localPos.y - testY;
            float distance = Mathf.Sqrt((distX * distX) + (distY * distY));

            // if the distance is less than the radius, collision!
            if (distance <= _b.Radius)
            {
                return new Collision(
                    _a.transform.position +
                    (_b.transform.position - _a.transform.position).normalized / 2, //should project on axis 
                    (_b.transform.position - _a.transform.position).normalized * (_b.Radius - distance));
            }

            return null;
        }

        private static Collision CheckDiscRectCollision(PhysicDisc _b, PhysicRect _a)
        {
            Vector2 pos = _b.transform.position;
            //transform our problem into an axis aligned rectangle centered in 0,0
            Vector2 localPos = _a.transform.InverseTransformPoint(pos);

            // temporary variables to set edges for testing
            float testX = localPos.x;
            float testY = localPos.y;

            // which edge is closest?
            if (localPos.x < -_a.Width / 2) testX = -_a.Width / 2; // test left edge
            else if (localPos.x > _a.Width / 2) testX = _a.Width / 2; // right edge
            if (localPos.y < -_a.Height / 2) testY = -_a.Height / 2; // top edge
            else if (localPos.y > _a.Height / 2) testY = _a.Height / 2; // bottom edge

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


        private static Collision CheckRectCollision(PhysicRect _a, PhysicRect _b)
        {
            //SAT quick implementation

            Func<PhysicRect, Vector2, Projection> project = (r, axis) =>
            {
                float min = float.MaxValue;
                float max = float.MinValue;
                for (int i = 0; i < 4; i++)
                {
                    // NOTE: the axis must be normalized to get accurate projections
                    float p = Vector2.Dot(axis, r[i]);
                    min = Math.Min(p, min);
                    max = Math.Max(p, max);
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
            foreach (Vector2 axis in axes1)
            {
                // project both shapes onto the axis
                Projection p1 = project(_a, axis);
                Projection p2 = project(_b, axis);
                // do the projections overlap?
                // get the overlap
                float o = p1.getOverlap(p2);
                if (o >= 0) //overlapping
                {
                    // check for minimum
                    if (o < overlap)
                    {
                        // then set this one as the smallest
                        overlap = o;
                        smallest = axis;
                    }
                }
                else
                {
                    //no overlap : no collision
                    return null;
                }
            }

            Vector2 perpendicular = new Vector2(-smallest.y, smallest.x);
            return new Collision(
                _a.transform.position + (_b.transform.position - _a.transform.position).normalized *
                (_a.Height + _a.Width) / 2, //should project on axis 
                (_b.transform.position - _a.transform.position).normalized * overlap);
        }

        private static Collision CustomSAT(PhysicRect _a, PhysicRect _b)
        {
            //SAT quick implementation

            Func<PhysicRect, Vector2, Projection> project = (r, axis) =>
            {
                float min = float.MaxValue;
                float max = float.MinValue;
                for (int i = 0; i < 4; i++)
                {
                    // NOTE: the axis must be normalized to get accurate projections
                    float p = Vector2.Dot(axis, r[i]);
                    min = Math.Min(p, min);
                    max = Math.Max(p, max);
                }

                return new Projection(min, max);
            };

            float overlap = 1000;
            Vector2 smallest = Vector2.zero;

            // loop over the axes1
            for (int i = 0; i < _a.VCount; ++i)
            {
                Vector3 axis = (_a[(i + 1) % _a.VCount])  - (_a[i]);
                Vector3 axis2 = _a.transform.TransformVector(_a[(i + 1) % _a.VCount] - _a[i]);
                // project both shapes onto the axis
                Projection p1 = project(_a, axis.normalized);
                Projection p2 = project(_b, axis.normalized);
                // do the projections overlap?
                // get the overlap
                float ov = p1.getOverlap(p2);
                if (ov >= 0) //overlapping
                {
                    // check for minimum
                    if (ov < overlap)
                    {
                        // then set this one as the smallest
                        overlap = ov;
                        smallest = axis;
                    }
                }
                else
                {
                    //no overlap : no collision
                    return null;
                }
            }

            // loop over the b axes
            for (int i = 0; i < _b.VCount; ++i)
            {
                Vector3 axis = (_b[(i + 1) % _b.VCount]) - (_b[i]);
                // project both shapes onto the axis
                Projection p1 = project(_a, axis.normalized);
                Projection p2 = project(_b, axis.normalized);
                // do the projections overlap?
                // get the overlap
                float ov = p1.getOverlap(p2);
                if (ov >= 0) //overlapping
                {
                    // check for minimum
                    if (ov < overlap)
                    {
                        // then set this one as the smallest
                        overlap = ov;
                        smallest = axis;
                    }
                }
                else
                {
                    //no overlap : no collision
                    return null;
                }
            }

            Vector3 dir = (_b.GetCentroid() - _a.GetCentroid()).normalized ;

            Vector3 normal;
            Vector3 point = FindPointOnEdge(dir, _a, out normal);
            return new Collision(point, normal.normalized * overlap);
        }

        public static Vector3 FindPointOnEdge(Vector3 _dir, PhysicRect _p, out Vector3 normal)
        {
            Vector3 result = _dir;
            normal = Vector3.zero;
            
            for (int i = 0; i < _p.VCount; ++i)
            {
                Vector3 e = _p[(i + 1) % _p.VCount] - _p[i];
                Vector3 o = _p.GetCentroid();
                //intersection between line O->dir and edge 
                float t2 = ((e.x * (o.y - _p[i].y)) + (e.y * (_p[i].x - o.x))) / (e.y * _dir.x - e.x * _dir.y);
                float t = ((_dir.x * (_p[i].y - o.y)) + (_dir.y * (o.x - _p[i].x))) / (e.x * _dir.y - e.y * _dir.x);
                float s = Vector3.Dot(_dir, e);

                if (t >= 0 && t <= 1 && t2 > 0) 
                {
                     result.Set(o.x + t2 * _dir.x, o.y + t2 * _dir.y, 0);
                     normal = new Vector3(e.y, -e.x);
                }
            }

            return result;
        }
    }
}