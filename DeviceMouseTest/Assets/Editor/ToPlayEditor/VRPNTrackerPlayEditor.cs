/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Based on: 
 * http://unity3d.com/es/learn/tutorials/modules/intermediate/editor/building-custom-inspector?playlist=17117
 * http://unity3d.com/es/learn/tutorials/modules/intermediate/editor/drawdefaultinspector-function?playlist=17117
 * http://unity3d.com/es/learn/tutorials/modules/intermediate/editor/adding-buttons-to-inspector?playlist=17117
 * http://unity3d.com/es/learn/tutorials/modules/beginner/live-training-archive/editor-basics/editor-scripting-intro?playlist=17117
 * http://unity3d.com/es/learn/tutorials/modules/intermediate/live-training-archive/property-drawers-custom-inspectors?playlist=17117
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNTrackerPlayEditor.cs
 *
 * usage: Must be located in the Editor folder
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VRPNTrackerPlay))]
public class VRPNTrackerPlayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Properties
        VRPNTrackerPlay vrpnTrackerPlay = (VRPNTrackerPlay) target;
        bool ready = true;
        string errorText = "";

        //Validation
        if (vrpnTrackerPlay.path == null || vrpnTrackerPlay.path == "")
        {
            errorText = "A file must be chosen";
            ready = false;
        }
        if (!Application.isPlaying)
        {
            errorText = "The editor must be running";
            ready = false;
        }

        //Controls
        EditorGUILayout.LabelField("Play", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(vrpnTrackerPlay.path, EditorStyles.textArea);
        if (GUILayout.Button("File Path"))
        {
            vrpnTrackerPlay.path = EditorUtility.OpenFilePanel("Open VRPN Tracker File", "/Assets/VRPNFiles", "vrpnTrackerFile");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        if (!ready || vrpnTrackerPlay.isPlaying)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Start"))
        {
            vrpnTrackerPlay.StartPlaying();
        }
        GUI.enabled = true;
        if (!ready || !vrpnTrackerPlay.isPlaying)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Stop"))
        {
            vrpnTrackerPlay.StopPlaying();
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        //User Feedback
        if (!ready)
        {
            EditorGUILayout.HelpBox(errorText, MessageType.Error);
        }

        if (vrpnTrackerPlay.isPlaying)
        {
            EditorGUILayout.HelpBox("Playing", MessageType.Info);
        }
    }
}