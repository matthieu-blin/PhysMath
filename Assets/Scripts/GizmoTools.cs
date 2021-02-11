
using UnityEngine;
namespace Bk
{
    public static class Gizmos
    {
        public static void DrawArrow( Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.AngleAxis(arrowHeadAngle, Vector3.forward) * -direction ;
            Vector3 left = Quaternion.AngleAxis(-arrowHeadAngle, Vector3.forward) * -direction ;
            UnityEngine.Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            UnityEngine.Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}