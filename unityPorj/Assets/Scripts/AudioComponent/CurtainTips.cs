using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CurtainTips : MonoBehaviour
{
    public Text prefab;
    
    private Text textTips;
    public static CurtainTips Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }


    public void ShowTips(string text)
    {
        textTips = Instantiate(prefab);
        textTips.transform.SetParent(transform, false);
        textTips.rectTransform.anchoredPosition3D = Vector3.zero;
        textTips.text = text;
    }
}
