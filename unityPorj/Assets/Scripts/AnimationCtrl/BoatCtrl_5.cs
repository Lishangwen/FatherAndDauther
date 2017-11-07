using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCtrl_5 : MonoBehaviour {
    public GameObject BoatTip;
	// Use this for initialization
	void Start () {
		
	}
	void OnMouseDown()
    {
        EventManager.dispatchEvent(EventType.GrandmaLieDown, null);
        BoatTip.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
