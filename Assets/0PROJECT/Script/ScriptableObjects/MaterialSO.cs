using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that holds data of materials
/// </summary>

[CreateAssetMenu(fileName = "MaterialsData", menuName = "SO/Material Data", order = 1)]
public class MaterialSO : ScriptableObject
{
    public Material MAT_Purple;
    public Material MAT_Black;
    public Material MAT_Blue;
    public Material MAT_Green;
    public Material MAT_Orange;
    public Material MAT_Pink;
    public Material MAT_Red;
    public Material MAT_Yellow;
}