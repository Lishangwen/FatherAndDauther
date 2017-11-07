using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmCtrl : MonoBehaviour {
    static bool isLoaded=false;
	// Use this for initialization
	void Start () {
        if(!isLoaded)
        {
            DontDestroyOnLoad(this);
            isLoaded = true;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
