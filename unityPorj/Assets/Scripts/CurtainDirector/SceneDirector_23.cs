using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector_23 : MonoBehaviour 
{
    public GameObject dog;
    public GameObject bg1_3;
    public GameObject bg4;
    public GameObject bg5;
    public GameObject bg3;
	// Use this for initialization
	void Start () 
    {
		if(SceneMgr.curtainIndex!=2)
        {
            dog.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (SceneMgr.curtainIndex != 2)
        {
            dog.SetActive(false);
        }
        if (SceneMgr.curtainIndex == 4)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(true);
            bg5.SetActive(false);
            bg3.SetActive(false);
        }
        else if (SceneMgr.curtainIndex == 5)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(false);
            bg5.SetActive(true);
            bg3.SetActive(false);
        }
        else if (SceneMgr.curtainIndex == 3)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(false);
            bg5.SetActive(false);
            bg3.SetActive(true);
        }
        else
        {
            bg1_3.SetActive(true);
            bg4.SetActive(false);
            bg5.SetActive(false);
            bg3.SetActive(false);
        }
	}
}
