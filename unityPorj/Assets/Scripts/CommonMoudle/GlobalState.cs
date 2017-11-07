using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState 
{
    public static void ResetFlag()
    {
        startHugGirl = false;
        hugGirlComplete = false;
        isBoatShowed = false;
        isEnteredWharf = false;
        isTalked = false;
        startTalk = false;
    }
    // 第一幕中相关状态标志
    public static bool startHugGirl = false;
    public static bool hugGirlComplete = false;
    

    // 第二幕相关状态标志
    public static bool isBoatShowed = false;        // 第二幕码头船是否已经出现过
    public static bool isEnteredWharf = false;      // 是否进入过码头

    public static bool isTalked = false;
    static public bool startTalk = false;


}
