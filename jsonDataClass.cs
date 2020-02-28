using System;
using System.Collections.Generic;

[Serializable]
public class jsonDataClass
{
    public Item [] itens;
}

[Serializable]
public class Item
{
    public string partName;
    public float [] position;
    public string newCenterCoordinate ;
    public string [] physics;
    public string [] actions;
    public string [] children;
    public string newCenterDirection;
    public string newCenterAxis;
    
    public string parent;
    public string texture;
}



