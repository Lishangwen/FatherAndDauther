using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GmCtrl : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
		
	}
	void OnMouseDown()
    {
        SceneMgr.Switch2NextCurtain();
    }
	// Update is called once per frame
	void Update () 
    {
		
	}
}
