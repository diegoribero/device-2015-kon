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
 * VRPNTrackerSaveEditor.cs
 *
 * usage: Must be added in the Editor folder
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VRPNTrackerSave))]
public class VRPNTrackerSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Properties
        VRPNTrackerSave vrpnTrackerSave = (VRPNTrackerSave) target;
        bool ready = true;
        string errorText = "";

        //VRPNTracker interaction
        if (vrpnTrackerSave.gameObject.GetComponent<VRPNTracker>() != null)
        {
            vrpnTrackerSave.TrackerType = vrpnTrackerSave.gameObject.GetComponent<VRPNTracker>().TrackerType;
            vrpnTrackerSave.TrackerName = vrpnTrackerSave.gameObject.GetComponent<VRPNTracker>().TrackerName;
        }

        //Validation
        if (vrpnTrackerSave.path == null || vrpnTrackerSave.path == "")
        {
            errorText = "A save path must be chosen";
            ready = false;
        }
        if (!Application.isPlaying)
        {
            errorText = "The editor must be running";
            ready = false;
        }
        if (vrpnTrackerSave.gameObject.GetComponent<VRPNTracker>() == null)
        {
            errorText = "This GameObject must contain a VRPNTracker script to record";
            ready = false;
        }
        if (vrpnTrackerSave.gameObject.GetComponents<VRPNTracker>().Length > 1)
        {
            errorText = "This GameObject must contain ONLY ONE VRPNTracker script to record";
            ready = false;
        }

        //Controls
        EditorGUILayout.LabelField("Tracker Type", vrpnTrackerSave.TrackerType.ToString(), EditorStyles.textField);
        EditorGUILayout.LabelField("Tracker Name", vrpnTrackerSave.TrackerName.ToString(), EditorStyles.textField);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Record", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(vrpnTrackerSave.path, EditorStyles.textArea);
        if (GUILayout.Button("Record Path"))
        {
            vrpnTrackerSave.path = EditorUtility.SaveFilePanel("Save VRPN Tracker File", "/Assets/VRPNFiles", vrpnTrackerSave.TrackerType.ToString() + "-" + vrpnTrackerSave.TrackerName.ToString(), "vrpnTrackerFile");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        if (!ready || vrpnTrackerSave.isRecording)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Start"))
        {
            vrpnTrackerSave.StartRecording();
        }
        GUI.enabled = true;
        if (!ready || !vrpnTrackerSave.isRecording)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Stop"))
        {
            vrpnTrackerSave.StopRecording();
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        //User Feedback
        if (!ready)
        {
            EditorGUILayout.HelpBox(errorText, MessageType.Error);
        }

        if (vrpnTrackerSave.isRecording)
        {
            EditorGUILayout.HelpBox("Recording", MessageType.Info);
        }
    }
} 