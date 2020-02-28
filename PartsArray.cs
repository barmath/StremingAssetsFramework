using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PartsArray : ScriptableObject
{
    public List<PartsObject> parts = new List<PartsObject>();
}
