using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour {

	// Use this for initialization
    static bool isLoaded = false;
    static List<string> sceneList = new List<string>() { "StartScene","Scene2","Scene3","Scene4"};
    static int curSceneIndex;
	void Start () {
       curSceneIndex = 0;
        if(!isLoaded)
        {
            DontDestroyOnLoad(this);
            isLoaded = true;
        }
        
	}
	public static void Switch2NextScene()
    {
        if (curSceneIndex < 3)
        {
            curSceneIndex++;
            SceneManager.LoadScene(sceneList[curSceneIndex]);
        }
    }
    public static void Switch2PreScene()
    {
        if(curSceneIndex>0)
        {
            curSceneIndex--;
            SceneManager.LoadScene(sceneList[curSceneIndex]);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
