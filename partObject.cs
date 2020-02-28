using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PartsObject : ScriptableObject
{
    public string partName;
    public List<float> position = new List<float>();
    public List<string> physics = new List<string>();
    public List<string> children = new List<string>();
    public string newCenterDirection;
    public string newCenterAxis;

    public string parent;
    public string texture;
}
