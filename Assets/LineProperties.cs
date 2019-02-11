using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LineProperties
{
    public float width = 0.03f;
    public Color color = Color.white;
    public int cornerVertices = 2;
    public int endCapVertices = 2;
    public bool useWorldSpace = true;
    public Material material;
}