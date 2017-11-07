using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioPlayer))]
public class AudioPlayerEditor : UnityEditor.Editor {

	private AudioPlayer Entity
    {
        get { return this.target as AudioPlayer; }
    }

    public override void OnInspectorGUI()
    {
        Entity.PlayerType = (AudioPlayerType)EditorGUILayout.EnumPopup("音效类型", Entity.PlayerType);
        Entity.playOnAwake = EditorGUILayout.Toggle("是否初始化时播放？", Entity.playOnAwake);
        Entity.playMode = (AudioPlayMode)EditorGUILayout.EnumPopup("循环类型", Entity.playMode);
        if (Entity.playMode == AudioPlayMode.Repeat)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            EditorGUILayout.BeginVertical(style);

            Entity.repeatRate = EditorGUILayout.FloatField("多少秒重复一次 ?", Entity.repeatRate);
            Entity.repeatTimes = EditorGUILayout.IntField("重复次数，-1为无数次", Entity.repeatTimes);

            EditorGUILayout.EndVertical();
        }

        Entity.useEvent = EditorGUILayout.Toggle("是否使用事件来触发播放？", Entity.useEvent);
        if (Entity.useEvent)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            EditorGUILayout.BeginVertical(style);
            Entity.startEvent = (EventType)EditorGUILayout.EnumPopup("播放事件(无选择None)", Entity.startEvent);
            Entity.endEvent = (EventType)EditorGUILayout.EnumPopup("停止播放事件(无选择None)", Entity.endEvent);

            Entity.compEventData = EditorGUILayout.Toggle("是否比较事件参数？", Entity.compEventData);
            if (Entity.compEventData)
            {
                Entity.eventData = EditorGUILayout.TextField("比较值", Entity.eventData);
            }
            EditorGUILayout.EndVertical();
        }
        else
        {
            Entity.startEvent = Entity.endEvent = EventType.None;
        }

        Entity.turnup = EditorGUILayout.Toggle("是否淡入？", Entity.turnup);
        if (Entity.turnup)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            EditorGUILayout.BeginVertical(style);
            Entity.upSpeed = EditorGUILayout.FloatField("淡入速度", Entity.upSpeed);
            Entity.upMinVolume = EditorGUILayout.FloatField("最小音量（初值）", Entity.upMinVolume);
            EditorGUILayout.EndVertical();
        }

        Entity.slowdown = EditorGUILayout.Toggle("是否淡出？", Entity.slowdown);
        if (Entity.slowdown)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            EditorGUILayout.BeginVertical(style);
            Entity.slowSpeed = EditorGUILayout.FloatField("淡出速度", Entity.slowSpeed);
            Entity.downMinVolume = EditorGUILayout.FloatField("最小音量（终值）", Entity.downMinVolume);
            EditorGUILayout.EndVertical();
        }

        //serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

    }
}
