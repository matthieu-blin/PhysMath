using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class PhysicRect : PhysicShape
{
    [SerializeField]
    float m_Width = 1;
    [SerializeField]
    float m_Height = 1;
    public override float GetInertia()
    {
        return m_physicBody.Masse * (m_Width * m_Width + m_Height * m_Height) / 12;
    }
    public override Vector2 GetCentroid()
    {
        return Vector2.zero;
    }


    private LineRenderer LineDrawer;

    protected override void Start()
    {
        base.Start();
        LineDrawer = GetComponent<LineRenderer>();

        LineDrawer.startWidth = 0.1f;
        LineDrawer.endWidth = 0.1f;
        LineDrawer.startColor = Color.green;
        LineDrawer.endColor = Color.green;
        Material mat = new Material(Shader.Find("GUI/Text Shader"));
        LineDrawer.material = mat;
    }

    public void OnDestroy()
    {
        DestroyImmediate(LineDrawer.material);
    }
    public void Update()
    {
        float halfH =  m_Height / 2;
        float halfW =  m_Width / 2;
        Vector3[] pos = new Vector3[4];
        pos[0] = transform.TransformPoint( new Vector3( - halfW,  - halfH, 0));
        pos[1] = transform.TransformPoint(new Vector3( + halfW,  - halfH, 0));
        pos[2] = transform.TransformPoint(new Vector3( + halfW,  + halfH, 0));
        pos[3] = transform.TransformPoint(new Vector3( - halfW,  + halfH, 0));
        LineDrawer.SetPositions(pos);
        LineDrawer.positionCount = 4;
    }

}

