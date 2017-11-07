using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector_22 : MonoBehaviour 
{
    public GameObject dog;
    public GameObject bg1_3;
    public GameObject bg3;
    public GameObject bg4;
    public GameObject bg5;

    public GameObject tree2;
    public GameObject tree3;
    public GameObject tree4;
    public GameObject tree5;

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
            tree2.SetActive(false);
            tree3.SetActive(false);
            tree4.SetActive(true);
        }
        else if(SceneMgr.curtainIndex == 5)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(false);
            bg5.SetActive(true);
            bg3.SetActive(false);
            tree2.SetActive(false);
            tree3.SetActive(false);
            tree4.SetActive(false);
            tree5.SetActive(true);
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
            bg3.SetActive(false);
            bg4.SetActive(false); 
            bg5.SetActive(false);
        }
        if(SceneMgr.curtainIndex==2)
        {
            tree2.SetActive(true);
            tree3.SetActive(false);
            tree4.SetActive(false);
        }
        if (SceneMgr.curtainIndex == 3)
        {
            tree2.SetActive(false);
            tree3.SetActive(true);
            tree4.SetActive(false);
        }
	}
}
