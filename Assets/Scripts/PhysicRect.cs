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

    public float Height { get => m_Height; }
    public float Width { get => m_Width; }

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
        Vector3[] pos = new Vector3[4];
        for (int i = 0; i < 4; i++)
            pos[i] = GetPoint(i);
        LineDrawer.SetPositions(pos);
        LineDrawer.positionCount = 4;
    }

    public Vector2 this[int index]
    {
        get => GetPoint(index);
    }

    Vector2 GetPoint(int index)
    {
        float halfH = m_Height / 2;
        float halfW = m_Width / 2;
        switch (index)
        {
            case 0:
                return transform.TransformPoint(new Vector3(-halfW, -halfH, 0));
            case 1:
                return transform.TransformPoint(new Vector3(+halfW, -halfH, 0));
            case 2:
                return transform.TransformPoint(new Vector3(+halfW, +halfH, 0));
            case 3:
                return transform.TransformPoint(new Vector3(-halfW, +halfH, 0));
        }
        return Vector2.zero;
    }

}

