using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMgr : MonoBehaviour {

	// Use this for initialization
    static public float GameTime = 0;  // 全局时间，单位为秒
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        GameTime += Time.deltaTime;
	}
}
