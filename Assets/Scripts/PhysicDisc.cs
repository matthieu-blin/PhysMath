using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicDisc : PhysicShape
{
    [SerializeField]
    float m_Radius = 1;
    public override float GetInertia()
    {
        return 0.5f *  m_physicBody.Masse * m_Radius * m_Radius;
    }
    public override Vector2 GetCentroid() 
    {
        return Vector2.zero;
    }


    private float ThetaScale = 0.01f;
    private int Size;
    private LineRenderer LineDrawer;
    private float Theta = 0f;

    public float Radius { get => m_Radius;  }

    protected override void Start()
    {
        base.Start();
        LineDrawer = GetComponent<LineRenderer>();
        LineDrawer.startWidth = 0.1f;
        LineDrawer.endWidth = 0.1f;
        LineDrawer.startColor = Color.green;
        LineDrawer.endColor = Color.green;
        Material whiteDiffuseMat = new Material(Shader.Find("GUI/Text Shader"));
        LineDrawer.material = whiteDiffuseMat;

    }
    public void OnDestroy()
    {
        DestroyImmediate(LineDrawer.material);
    }

    public void Update()
    {
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        LineDrawer.positionCount = Size;
        LineDrawer.startWidth = 0.1f;
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = transform.position.x + m_Radius * Mathf.Cos(Theta);
            float y = transform.position.y + m_Radius * Mathf.Sin(Theta);
            LineDrawer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

}
