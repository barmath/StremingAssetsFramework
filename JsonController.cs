using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonController : MonoBehaviour
{

    [SerializeField]
    private string jsonURL;

    public jsonDataClass jsnData;

    [SerializeField]
    private PartsArray partsArray;

    [SerializeField]
    private PartsObject partsObject;

    void Start()
    {
        StartCoroutine(getData());
        partsArray.parts.Clear();

    }

    IEnumerator getData(){

        Debug.Log("Processig Data, wait");
        WWW _www = new WWW(jsonURL);
        yield return _www;
        if(_www.error == null){
            processJsonData(_www.text);
        }else{
            Debug.Log("Error loading Json");
        }
    }


    private void processJsonData(string _url){

        jsnData = JsonUtility.FromJson<jsonDataClass>(_url);
        fromJsonToObject();
        showData();
   
    }

    //Send the information to the json parser part object array
    private void fromJsonToObject(){

        for(int i = 0; i < jsnData.itens.Length ; i++){

            partsObject = (PartsObject) ScriptableObject.CreateInstance( typeof(PartsObject) );
            var clone = Instantiate(partsObject);

            partsObject.partName = jsnData.itens[i].partName;
            for(int j = 0 ;j < jsnData.itens[i].position.Length ; j++)
            {
                partsObject.position.Add(jsnData.itens[i].position[j]);
            }
            for(int j = 0 ;j < jsnData.itens[i].physics.Length ; j++)
            {
                partsObject.physics.Add(jsnData.itens[i].physics[j]); 
            }
            for(int j = 0 ;j < jsnData.itens[i].children.Length ; j++)
            {
                partsObject.children.Add(jsnData.itens[i].children[j]);   
            }
            partsObject.newCenterDirection = jsnData.itens[i].newCenterDirection;
            partsObject.newCenterAxis = jsnData.itens[i].newCenterAxis;
            partsObject.parent = jsnData.itens[i].parent.ToString();
            partsObject.texture = jsnData.itens[i].texture.ToString();
            partsArray.parts.Add(partsObject);
        }
    }

    //Show the data to be parsed to the part object array
    public void showData(){

        for(int i = 0; i < jsnData.itens.Length ; i++){
            Debug.Log("Part name : "+partsArray.parts[i].partName);
            for(int j = 0 ;j < jsnData.itens[i].position.Length ; j++)
            {
                Debug.Log("Cooordinates : "+partsArray.parts[i].position[j]);
            }
            for(int j = 0 ;j < jsnData.itens[i].physics.Length ; j++)
            {
                Debug.Log("Physics Components : "+partsArray.parts[i].physics[j]);
            }
            for(int j = 0 ;j < jsnData.itens[i].children.Length ; j++)
            {
                Debug.Log("Children : "+partsArray.parts[i].children[j]);
            }
            Debug.Log("New Center Direction : "+partsArray.parts[i].newCenterDirection);
            Debug.Log("New Center Axis : "+partsArray.parts[i].newCenterAxis);
            Debug.Log("Parent : "+partsArray.parts[i].parent);
            Debug.Log("Texture : "+partsArray.parts[i].texture);

        }

    }
}
