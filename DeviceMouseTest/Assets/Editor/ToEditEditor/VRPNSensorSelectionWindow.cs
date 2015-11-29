/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Based on:
 * http://docs.unity3d.com/Manual/GUIScriptingGuide.html
 * http://unity3d.com/es/learn/tutorials/modules/beginner/live-training-archive/editor-basics/editor-scripting-intro?playlist=17117
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNSensorSelectionWindow.cs
 *
 * usage: Must be located in the Editor folder
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class VRPNSensorSelectionWindow : EditorWindow
{
    //Private properties
    //Size and position of the window
    private static Vector2 size;
    private static Rect pos;
    //VRPN Recording selected
    private static VRPNDataObject inFront;
    //Sensors list and disabled sensors list
    private static Dictionary<int, int> sensors;
    private static Dictionary<int, int>.Enumerator sensorsE;
    private static Dictionary<int, int> disabledSensors;
    //Sensors state
    private static bool[] states;
    //Window controls properties
    private float labelsHeigth = 20;
    private Vector2 scrollPosition;
    private float scrollSize = 15;

    public static void ShowSensorSelectionWindow(Vector2 nSize, Rect nPosition, VRPNDataObject nInFront)
    {
        size = nSize;
        pos = nPosition;
        inFront = nInFront;

        sensors = VRPNEditEditor.Instance.GetSensors(inFront.dataName, inFront.originalDataTime, inFront.dataDevice);
        disabledSensors = VRPNEditEditor.Instance.GetDisabledSensors(inFront.dataName, inFront.originalDataTime, inFront.dataDevice);
        states = new bool[sensors.Count];
        sensorsE = sensors.GetEnumerator();

        //Initial sensors state
        int numSensor = 0;
        while (sensorsE.MoveNext())
        {
            int test;
            if (disabledSensors.TryGetValue(sensorsE.Current.Key, out test))
            {
                states[numSensor] = false;
            }
            else
            {
                states[numSensor] = true;
            }
            numSensor++;
        }

        VRPNSensorSelectionWindow window = VRPNSensorSelectionWindow.CreateInstance<VRPNSensorSelectionWindow>();
        window.ShowAsDropDown(pos, size);
    }

    public void OnGUI()
    {
        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, this.position.width, this.position.height), scrollPosition, new Rect(0, 0, this.position.width - scrollSize, labelsHeigth * (states.Length + 2)));
        //Sensors Label
        GUIStyle styleLabel = new GUIStyle(GUI.skin.label);
        styleLabel.alignment = TextAnchor.MiddleCenter;
        styleLabel.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(0, 0, this.position.width - scrollSize, labelsHeigth), "Sensors", styleLabel);
        //Sensors list
        sensorsE = sensors.GetEnumerator();
        int numSensor = 0;
        while (sensorsE.MoveNext())
        {
            states[numSensor] = EditorGUI.ToggleLeft(new Rect(0, labelsHeigth * (numSensor + 1), this.position.width - scrollSize, labelsHeigth), "Sensor " + sensorsE.Current.Key + ":", states[numSensor]);
            numSensor++;
        }
        sensorsE = sensors.GetEnumerator();
        //Apply button
        if (GUI.Button(new Rect(0, labelsHeigth * (states.Length + 1), this.position.width - scrollSize, labelsHeigth), "Apply"))
        {
            numSensor = 0;
            while (sensorsE.MoveNext())
            {
                if (states[numSensor])
                {
                    VRPNEditEditor.Instance.EnableTrackerSensor(inFront.dataName, inFront.originalDataTime, inFront.dataDevice, sensorsE.Current.Key);
                }
                else
                {
                    VRPNEditEditor.Instance.DisableTrackerSensor(inFront.dataName, inFront.originalDataTime, inFront.dataDevice, sensorsE.Current.Key);
                }
                numSensor++;
            }
            this.Close();
        }
        GUI.EndScrollView();
    }
}
