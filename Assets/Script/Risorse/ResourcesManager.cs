using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcesManager : MonoBehaviour {


    public List<BaseResourceData> resourcesPrefabs = new List<BaseResourceData>();
    public List<BaseResourceData> resources = new List<BaseResourceData>();

    public void Init() {
        foreach (BaseResourceData res in resourcesPrefabs) {
            resources.Add(Instantiate(res));
        }
    }


	public BaseResourceData GetResourceDataFromString(string _resourceName) {
        foreach (BaseResourceData resourceData in resources) {
            if (resourceData.ID == _resourceName)
                return resourceData;
        }
        return null;
    }
    private void Start() {
        Init();
        Debug.Log(GetResourceDataFromString("Food").ID);

    }
}
