using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fatherCtrl_1 :BaseActor {

    public GameObject fatherTip;
	// Use this for initialization
	void Start () {
        SetOriginDirect(Direction.RIGHT);
        Init();
        TurnLeft();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
    void OnMouseDown()
    {
        fatherTip.SetActive(false);
        EventManager.dispatchEvent(EventType.FatherLastClick, null);
    }
    void FixedUpdate()
    {

    }
}
