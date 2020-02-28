using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Valve.VR.InteractionSystem;



public class ObjectFromStream : MonoBehaviour
{

    [SerializeField]
    private PartsArray partsArray;

    private GameObject partOnProgress;

    private int active = 0;

    // [SerializeField]
    // private PartsObject partsObject;

    // [SerializeField]
    // private Test test;



    void Update() {
        if(active < 3)
        {
            Debug.Log(active); 
            loadAssets();
            active++;
        }   
    }

    void loadAssets()
    {   
        Debug.Log("Loading Assets");

        // for(int i = 0; i < partsArray.parts.Count ; i++){
        //     organizeHierarchy(i);
        // }

        for(int i = 0; i < partsArray.parts.Count ; i++)
        {
            partOnProgress = GameObject.Find(partsArray.parts[i].partName);

           
            //Instantiate() TODO will download the assets from a link and instantiate them runtime
            organizeHierarchy(i);
            setPosition(i);
            recenter(i);
            addComponents(i);// TOComplete
            //organizeAxis(); TODO
            
        }
        //checkForRecenter();
        //createAbstractChildren();
    }


    //Should be substitute by an URL where the assets will be stored 
    private void organizeHierarchy(int i)
    {
        GameObject parent = GameObject.Find(partsArray.parts[i].parent);
        partOnProgress.transform.SetParent(parent.transform);
    }
    
    //---------------------------------------------------------------------------

    //Set the position provided by coordinate on file
    private void setPosition(int i)
    {
        float x = partsArray.parts[i].position[0];
        float y = partsArray.parts[i].position[1];
        float z = partsArray.parts[i].position[2];

        Vector3 coor = new Vector3(x,y,z);
        partOnProgress.transform.position = coor;
    }

    //---------------------------------------------------------------------------

    //Check for recenter need and make that change
    private void recenter(int i)
    {
        // string side = partsArray.parts[i].newCenterDirection;
        // string axis = partsArray.parts[i].newCenterAxis;

        // string parentName = partsArray.parts[i].partName+"Parent";
        // GameObject parent = new GameObject(parentName);


        try{
            string side = partsArray.parts[i].newCenterDirection;
            string axis = partsArray.parts[i].newCenterAxis;

            string childrenName = partsArray.parts[i].children[0];
            GameObject children = GameObject.Find(childrenName);
            
            Debug.Log("Side object on recenter : "+partsArray.parts[i].newCenterDirection);
            Debug.Log("Side variable on recenter: "+side);

            if(side == "left" || side  == "right")
            {
                moveAxis(children, side, axis);
            }

        }
        catch{}

       
    }

    private void setNewParent(GameObject parent, GameObject tempParent)
    { 
        Vector3 childCoor = partOnProgress.transform.position;
        parent.transform.position = childCoor;
        parent.transform.SetParent(tempParent.transform);
        partOnProgress.transform.SetParent(parent.transform);
    }

    private void moveAxis(GameObject children, string side, string axis)
    {
        float factorForParent = 0;
        float factorForChildren = 0;

        Debug.Log("Side object on moveAxis : "+side);

        if(side == "left")
        {
            factorForParent = 1;
            factorForChildren = -1;
        }
        else if(side == "right")
        {
            factorForParent = -1;
            factorForChildren = 1;
        }

        //TODO : fix the recenter for y,z axis 

        //recenter the partOnProgress 
        float sizeOfChildren = children.GetComponent<Renderer>().bounds.size.x;
        float distanceToDes = factorForParent * (sizeOfChildren/2);
        
        Vector3 axisToIncrement = axisSetter(distanceToDes, axis);

        Vector3 testeV = new Vector3(distanceToDes,0,0);
        partOnProgress.transform.position += testeV;

        //recenter the Children (GUARDAR PARA EVENTUAIS CONSULTAS)
        // distanceToDes = distanceToDes * factorForChildren;
        // axisToIncrement = new Vector3(distanceToDes,0,0);
        // partOnProgress.transform.position += axisToIncrement;

    }

    private Vector3 axisSetter(float distanceToDes, string axis)
    {
        Vector3 axisToIncrement = new Vector3();

        if(axis == "x")
        {
            axisToIncrement = new Vector3(distanceToDes,0,0);
        }
        else if(axis == "y") 
        {
            axisToIncrement = new Vector3(0,distanceToDes,0);
        }
        else if(axis == "z")
        {
            axisToIncrement = new Vector3(0,0,distanceToDes);
        }
        return axisToIncrement;
    }

    //-----------------------------------------------------------------------------------

    private void addComponents(int i)
    {

        //check about unity behaviour componets
        //check physics componets 
        //check another particular script

        Debug.Log("Adding Components");

        if(partsArray.parts[i].physics.Count != 0)
        {
            foreach(string j in partsArray.parts[i].physics)
            {
                if(String.Equals(j,"Rigidbody")){
                    Debug.Log("Adding rigidbody.....");
                    Rigidbody game = partOnProgress.AddComponent<Rigidbody>();
                }
                else if (String.Equals(j,"MeshRenderer"))
                {
                    Debug.Log("Adding mesh renderer.....");
                }
                else if (String.Equals(j,"MeshFilter"))
                {
                    Debug.Log("Adding mesh filter.....");
                }
                else if (String.Equals(j,"CircularDrive"))
                {
                    //Debug.Log("Adding mesh Circular Drive and Interactable.....");
                    Interactable inter = partOnProgress.AddComponent<Interactable>();
                    CircularDrive cirDr = partOnProgress.AddComponent<CircularDrive>();

                    //TODO a function to choose the axis and fix it
                    cirDr.axisOfRotation = CircularDrive.Axis_t.YAxis;

                    cirDr.limited = true;

                    if(partsArray.parts[i].newCenterDirection == "right")
                    {
                        cirDr.minAngle = -150;
                        cirDr.maxAngle = 0;
                    }
                    else if (partsArray.parts[i].newCenterDirection == "left")
                    {
                        cirDr.minAngle = 0;
                        cirDr.maxAngle = 150;
                    }
                   
                    //for right : min -negative, max 0
                    //for left : min 0, max +positive

                   
                }
                else if (String.Equals(j,"SphereCollider"))
                {
                    SphereCollider sc = partOnProgress.AddComponent<SphereCollider>();
                }
                else if (String.Equals(j,"BoxCollider"))
                {
                    BoxCollider bc = partOnProgress.AddComponent<BoxCollider>();
                }
            }
        }
    }
}
