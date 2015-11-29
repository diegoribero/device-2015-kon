/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Based on:
 * http://docs.unity3d.com/Manual/GUIScriptingGuide.html
 * http://unity3d.com/es/learn/tutorials/modules/beginner/live-training-archive/editor-basics/editor-scripting-intro?playlist=17117
 * http://angryant.com/2009/09/18/gui-drag-drop/
 * https://gist.github.com/bzgeb/3800350
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNEditWindow.cs
 *
 * usage: Must be located in the Editor folder
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

public class VRPNEditorWindow : EditorWindow
{
    private static VRPNEditorWindow window;

    //General layout properties
    private float headerHeight = 40;
    private float headerLabelsWidth = 250;
    private float scrollSize = 15;
    private float zoomButtonSize = 30;
    private float footerHeight = 60;
    private float footerButtonsWidth = 125;

    //Time line properties
    private Vector2 timeLineScrollViewVector = Vector2.zero;
    private int timeLineTime = 60;
    private int timeLineZoom = 1;
    private int timeLineProportion = 10;
    private int timeLineLinesSeparation = 40;
    private int timeLineLabelsWidth = 40;
    private int timeLineDeviceHeight = 40;
    private int timeLineDevicesNumber = 5;
    private Rect timeLineDropArea;

    //General styles
    private GUIStyle styleBoxHeader;
    private GUIStyle styleBoxLimit;
    private GUIStyle styleLabelTimeLineZoom;
    private GUIStyle styleLabelTimeLineDevice;

    //Messages
    private string error = "";
    private bool ready;

    //Editor State
    private VRPNDataObject inFront;
    private List<VRPNDataObject> devicesData;

    //Update State
    private bool doRepaintDrag = false;
    private bool doRepaintPlaying = false;

    [MenuItem("VRPN-Tool/VRPN Recordings Editor")]
    public static void ShowVRPNRecordingsEditorEditorWindow()
    {
        window = GetWindow<VRPNEditorWindow>();
        window.titleContent.text = "VRPN Editor";
        window.minSize = new Vector2(375,200);
    }

    //Method used each time the window is opened, including each time Unity Editor enter and exit play mode
    public void OnEnable()
    {
        VRPNEditEditor.Instance.ChangeTimeLineData(timeLineTime, timeLineZoom, timeLineProportion);
        VRPNEditEditor.Instance.LoadState(Application.dataPath + "/VRPNFiles/temp.vrpnEditFile");
        timeLineTime = VRPNEditEditor.Instance.getTimeLineTime();
        timeLineZoom = VRPNEditEditor.Instance.getTimeLineZoom();
        timeLineProportion = VRPNEditEditor.Instance.getTimeLineProportion();
        DataFilling();
    }

    //Method used each time the window is closed, including each time Unity Editor enter and exit play mode
    public void OnDisable()
    {
        VRPNEditEditor.Instance.ChangeTimeLineData(timeLineTime, timeLineZoom, timeLineProportion);
        VRPNEditEditor.Instance.SaveState(Application.dataPath + "/VRPNFiles/temp.vrpnEditFile");
        if (Application.isPlaying && VRPNEdit.instance != null && VRPNEdit.instance.isPlaying)
        {
            VRPNEdit.instance.StopPlaying();
        }
    }

    //Method used to modify some original Unity GUI styles
    public void createStyles()
    {
        //Style for the box in the header
        styleBoxHeader = new GUIStyle();
        styleBoxHeader.alignment = TextAnchor.LowerCenter;
        styleBoxHeader.fontStyle = FontStyle.Bold;
        styleBoxHeader.fontSize = 12;
        styleBoxHeader.normal.textColor = Color.white;

        //Style for the box used as limits
        styleBoxLimit = new GUIStyle(GUI.skin.box);
        styleBoxLimit.border = new RectOffset();

        //Style for the label used in time line Zoom
        styleLabelTimeLineZoom = new GUIStyle(GUI.skin.label);
        styleLabelTimeLineZoom.alignment = TextAnchor.MiddleCenter;

        //Style for the label used to show the time line devices
        styleLabelTimeLineDevice = new GUIStyle(GUI.skin.label);
        styleLabelTimeLineDevice.alignment = TextAnchor.MiddleCenter;
        styleLabelTimeLineDevice.fontStyle = FontStyle.BoldAndItalic;
        styleLabelTimeLineDevice.fontSize = 10;
    }

    public void OnGUI()
    {
        //GUI must be painted only if there is a VRPNEdit instance and a VRPNEventManager instance
        if (VRPNEdit.instance != null && VRPNEventManager.instance != null)
        {
            createStyles();
            if (window == null)
            {
                window = GetWindow<VRPNEditorWindow>();
            }

            GUI.BeginGroup(new Rect(0, 0, window.position.width, window.position.height));
            paintHeader();
            paintBody();
            paintFooter();
            GUI.EndGroup();
        }
        else
        {
            EditorGUILayout.HelpBox("There needs to be one active VRPNEdit script and one active VRPNEventManager script on a GameObject in your scene.\nJust add a VRPNEventManager PREFAB to the scene and you will be ready to go!", MessageType.Error);
        }           
    }

    public void Update()
    {
        if (doRepaintDrag)
        {
            Repaint();
        }
        if (doRepaintPlaying)
        {
            Repaint();
            if (Application.isPlaying && VRPNEdit.instance != null && !VRPNEdit.instance.isPlaying)
            {
                doRepaintPlaying = false;
            }
        }
    }

    //Method to paint the upper part of the GUI: The time field, the devices label, and the time line
    private void paintHeader()
    {
        //Header group
        GUI.BeginGroup(new Rect(0, 0, window.position.width, headerHeight + 1));
        //Time field
        timeLineTime = EditorGUI.IntField(new Rect(0, 0, headerLabelsWidth, headerHeight/2), "Time:", timeLineTime);
        if (timeLineTime < 1)
        {
            timeLineTime = 1;
        }
        //Device label
        GUI.Box(new Rect(0, headerHeight / 2, headerLabelsWidth, headerHeight / 2), "Devices", styleBoxHeader);
        //Time line
        if (window.position.height < headerHeight + footerHeight + 2 + timeLineDeviceHeight * timeLineDevicesNumber)
        {
            GUI.BeginScrollView(new Rect(headerLabelsWidth, 0, window.position.width - headerLabelsWidth - scrollSize, headerHeight), new Vector2(timeLineScrollViewVector.x, 0), new Rect(0, 0, timeLineTime * timeLineProportion * timeLineZoom + 1, headerHeight), GUIStyle.none, GUIStyle.none);
        }
        else
        {
            GUI.BeginScrollView(new Rect(headerLabelsWidth, 0, window.position.width - headerLabelsWidth, headerHeight), new Vector2(timeLineScrollViewVector.x, 0), new Rect(0, 0, timeLineTime * timeLineProportion * timeLineZoom + 1, headerHeight), GUIStyle.none, GUIStyle.none);
        }
        GUI.Label(new Rect(0, 0, timeLineLabelsWidth, headerHeight), "0");
        for (int i = 1; i < (timeLineTime * timeLineProportion * timeLineZoom) / timeLineLinesSeparation; i++)
        {
            float timeValue = (timeLineTime / 1f) / ((timeLineTime * timeLineProportion * timeLineZoom) / timeLineLinesSeparation) * i;
            GUI.Box(new Rect(timeLineLinesSeparation * i, 0, 1, headerHeight), "", styleBoxLimit);
            GUI.Label(new Rect(timeLineLinesSeparation * i, 0, timeLineLabelsWidth, headerHeight), Math.Round(timeValue, 2).ToString());
        }
        //Border Lines
        GUI.Box(new Rect(timeLineTime * timeLineProportion * timeLineZoom, 0, 1, headerHeight), "", styleBoxLimit);
        GUI.EndScrollView();
        //Border Lines
        GUI.Box(new Rect(headerLabelsWidth, 0, 1, headerHeight), "", styleBoxLimit);
        GUI.Box(new Rect(0, headerHeight, window.position.width, 1), "", styleBoxLimit);
        GUI.EndGroup();
    }

    //Method to paint the body of the GUI: The devices list, the time line recordings and the time line zoom buttons
    private void paintBody()
    {
        float bodyHeight = window.position.height - headerHeight - footerHeight - 2;
        //Body group
        GUI.BeginGroup(new Rect(0, headerHeight + 1, window.position.width, window.position.height - footerHeight - 1));
        //Auxiliar methods to detect events
        timeLineDropArea = new Rect(headerLabelsWidth, 0, window.position.width - headerLabelsWidth, bodyHeight);
        detectFileDragAndDrop();
        detectRecordingDelete();
        detectTrackerSensorEdit();
        //Devices section
        GUI.BeginScrollView(new Rect(0, 0, headerLabelsWidth, bodyHeight - scrollSize), new Vector2(0, timeLineScrollViewVector.y), new Rect(0, 0, headerLabelsWidth, timeLineDevicesNumber * timeLineDeviceHeight), GUIStyle.none, GUIStyle.none);
        DevicePainting();
        //Border Lines
        for (int i = 1; i <= timeLineDevicesNumber; i++)
        {
            GUI.Box(new Rect(0, timeLineLinesSeparation * i, headerLabelsWidth, 1), "", styleBoxLimit);
        }
        GUI.EndScrollView();
        //Time line zoom buttons
        if (GUI.Button(new Rect(0, bodyHeight - scrollSize, zoomButtonSize, scrollSize), "-"))
        {
            timeLineZoom--;
            if (timeLineZoom < 1)
            {
                timeLineZoom = 1;
            }
        }
        GUI.Label(new Rect(zoomButtonSize, bodyHeight - scrollSize, headerLabelsWidth - zoomButtonSize * 2, scrollSize), "Change time zoom", styleLabelTimeLineZoom);
        if (GUI.Button(new Rect(headerLabelsWidth- zoomButtonSize, bodyHeight - scrollSize, zoomButtonSize, scrollSize), "+"))
        {
            timeLineZoom++;
        }
        //Time line recordings
        timeLineScrollViewVector = GUI.BeginScrollView(timeLineDropArea, timeLineScrollViewVector, new Rect(0, 0, timeLineTime * timeLineProportion * timeLineZoom + 1, timeLineDevicesNumber * timeLineDeviceHeight));
        DataPainting();
        //Border Lines
        for (int i = 1; i < (timeLineTime * timeLineProportion * timeLineZoom) / timeLineLinesSeparation; i++)
        {
            GUI.Box(new Rect(timeLineLinesSeparation * i, 0, 1, timeLineDevicesNumber * timeLineDeviceHeight), "", styleBoxLimit);
        }
        GUI.Box(new Rect(timeLineTime * timeLineProportion * timeLineZoom, 0, 1, bodyHeight), "", styleBoxLimit);
        for (int i = 1; i <= timeLineDevicesNumber; i++)
        {
            GUI.Box(new Rect(0, timeLineDeviceHeight * i, timeLineTime * timeLineProportion * timeLineZoom + 1, 1), "", styleBoxLimit);
        }
        //Playback position indicator
        if (Application.isPlaying && VRPNEdit.instance != null && VRPNEdit.instance.isPlaying)
        {
            Color color = GUI.color;
            GUI.color = Color.blue;
            GUI.Box(new Rect((Time.time - VRPNEdit.instance.initTime) * timeLineZoom * timeLineProportion, 0, 1, timeLineDevicesNumber * timeLineDeviceHeight), "");
            GUI.color = color;
            doRepaintPlaying = true;
        }
        GUI.EndScrollView();
        //Border Lines
        GUI.Box(new Rect(headerLabelsWidth, 0, 1, bodyHeight), "", styleBoxLimit);
        GUI.EndGroup();
    }

    //Method to paint the lower part of the GUI: The playback, clear, save and open buttons, the error area and the selection area
    private void paintFooter()
    {
        ready = true;
        error = "";
        //Validation
        if (VRPNEdit.instance == null)
        {
            error = "There needs to be one active VRPNEdit script on a GameObject in your scene.";
            ready = false;
        }
        if (!Application.isPlaying)
        {
            error = "The editor must be running to play";
            ready = false;
        }
        //Footer group
        GUI.BeginGroup(new Rect(0, window.position.height - footerHeight - 1, window.position.width, footerHeight + 1));
        if (!ready)
        {
            GUI.enabled = false;
        }
        //Play and stop buttons
        if (VRPNEdit.instance.isPlaying)
        {
            if (GUI.Button(new Rect(0, 1, footerButtonsWidth, footerHeight / 3), "Stop"))
            {
                VRPNEdit.instance.StopPlaying();
                doRepaintPlaying = false;
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 1, footerButtonsWidth, footerHeight / 3), "Play"))
            {
                VRPNEdit.instance.StartPlaying();
            }
        }
        if (VRPNEdit.instance.isPlaying)
        {
            GUI.enabled = false;
        }
        else
        {
            GUI.enabled = true;
        }
        //Save, Open and Clear Buttons
        if (GUI.Button(new Rect(footerButtonsWidth, 1, footerButtonsWidth, footerHeight / 3), "Save"))
        {
            string vrpnEditSavePath = EditorUtility.SaveFilePanel("Save VRPN Edit File", "/Assets/VRPNFiles", "Editor", "vrpnEditFile");
            VRPNEditEditor.Instance.ChangeTimeLineData(timeLineTime, timeLineZoom, timeLineProportion);
            VRPNEditEditor.Instance.SaveState(vrpnEditSavePath);
        }
        if (GUI.Button(new Rect(footerButtonsWidth, 1 + footerHeight / 3, footerButtonsWidth, footerHeight / 3), "Open"))
        {
            string vrpnEditSavePath = EditorUtility.OpenFilePanel("Save VRPN Edit File", "/Assets/VRPNFiles", "vrpnEditFile");
            VRPNEditEditor.Instance.LoadState(vrpnEditSavePath);
            timeLineTime = VRPNEditEditor.Instance.getTimeLineTime();
            timeLineZoom = VRPNEditEditor.Instance.getTimeLineZoom();
            timeLineProportion = VRPNEditEditor.Instance.getTimeLineProportion();
            DataFilling();
        }
        if (GUI.Button(new Rect(0, 1 + footerHeight / 3, footerButtonsWidth, footerHeight / 3), "Clear"))
        {
            VRPNEditEditor.Instance.Clear();
            DataFilling();
        }
        GUI.enabled = true;
        //Error area
        EditorGUI.HelpBox(new Rect(0, 1 + 2 * footerHeight / 3, footerButtonsWidth * 2, footerHeight / 3), error, MessageType.Error);
        //Selection area
        EditorGUI.HelpBox(new Rect(footerButtonsWidth * 2, 1, window.position.width - footerButtonsWidth * 2, footerHeight), inFront == null ? "" : inFront.dataName, MessageType.Info);
        //Border Lines
        GUI.Box(new Rect(0, 0, window.position.width, 1), "", styleBoxLimit);
        GUI.EndGroup();
    }

    //Method to detect and process a file drag and drop event
    private void detectFileDragAndDrop()
    {
        if (!VRPNEdit.instance.isPlaying)
        {
            Event evt = Event.current;
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!timeLineDropArea.Contains(evt.mousePosition))
                        return;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (UnityEngine.Object dragged_object in DragAndDrop.objectReferences)
                        {
                            string fileExtension = AssetDatabase.GetAssetPath(dragged_object).Substring(AssetDatabase.GetAssetPath(dragged_object).LastIndexOf(".") + 1);
                            switch (fileExtension)
                            {
                                case "vrpnTrackerFile":
                                    VRPNEditEditor.Instance.AddTracker(dragged_object.name, (evt.mousePosition.x - headerLabelsWidth + timeLineScrollViewVector.x) / (timeLineZoom * timeLineProportion), Application.dataPath + AssetDatabase.GetAssetPath(dragged_object).Substring(6));
                                    break;
                                case "vrpnAnalogFile":
                                    VRPNEditEditor.Instance.AddAnalog(dragged_object.name, (evt.mousePosition.x - headerLabelsWidth + timeLineScrollViewVector.x) / (timeLineZoom * timeLineProportion), Application.dataPath + AssetDatabase.GetAssetPath(dragged_object).Substring(6));
                                    break;
                                case "vrpnButtonFile":
                                    VRPNEditEditor.Instance.AddButton(dragged_object.name, (evt.mousePosition.x - headerLabelsWidth + timeLineScrollViewVector.x) / (timeLineZoom * timeLineProportion), Application.dataPath + AssetDatabase.GetAssetPath(dragged_object).Substring(6));
                                    break;
                                default:
                                    Debug.LogError("Wrong file format");
                                    break;
                            }
                        }
                        DataFilling();
                    }
                    break;
            }
        }
    }

    //Method to detect and process a delete event (Delete key)
    private void detectRecordingDelete()
    {
        if (!VRPNEdit.instance.isPlaying)
        {
            Event evt = Event.current;
            switch (evt.type)
            {
                case EventType.KeyUp:
                    if (evt.keyCode == KeyCode.Delete && EditorWindow.focusedWindow.Equals(window) && inFront != null)
                    {
                        inFront.deleteDataObject();
                        DataFilling();
                    }
                    break;
            }
        }
    }

    //Method to detect and process a tracker sensor edit (S key)
    private void detectTrackerSensorEdit()
    {
        if (!VRPNEdit.instance.isPlaying)
        {
            Event evt = Event.current;
            switch (evt.type)
            {
                case EventType.KeyUp:
                    if (evt.keyCode == KeyCode.S && EditorWindow.focusedWindow.Equals(window) && inFront != null && inFront.dataType == VRPNManager.DeviceType.Tracker)
                    {
                        VRPNSensorSelectionWindow.ShowSensorSelectionWindow(new Vector2(headerLabelsWidth, 200), new Rect(window.position.position.x, window.position.position.y, headerLabelsWidth, 0), inFront);
                    }
                    break;
            }
        }
            
    }

    //Method to paint the device list in the time line
    private void DevicePainting()
    {
        Dictionary<string, VRPNTrackerRecordings>.Enumerator eTrackers = VRPNEditEditor.Instance.GetTrackersEnumerator();
        Dictionary<string, VRPNAnalogRecordings>.Enumerator eAnalogs = VRPNEditEditor.Instance.GetAnalogsEnumerator();
        Dictionary<string, VRPNButtonRecordings>.Enumerator eButtons = VRPNEditEditor.Instance.GetButtonsEnumerator();
        timeLineDevicesNumber = 0;
        while (eTrackers.MoveNext())
        {
            GUI.Label(new Rect(0, timeLineLinesSeparation * timeLineDevicesNumber, headerLabelsWidth, timeLineLinesSeparation), "Tracker: \n" + eTrackers.Current.Key, styleLabelTimeLineDevice);
            timeLineDevicesNumber++;
        }
        while (eAnalogs.MoveNext())
        {
            GUI.Label(new Rect(0, timeLineLinesSeparation * timeLineDevicesNumber, headerLabelsWidth, timeLineLinesSeparation), "Analog: \n" + eAnalogs.Current.Key, styleLabelTimeLineDevice);
            timeLineDevicesNumber++;
        }
        while (eButtons.MoveNext())
        {
            GUI.Label(new Rect(0, timeLineLinesSeparation * timeLineDevicesNumber, headerLabelsWidth, timeLineLinesSeparation), "Button: \n" + eButtons.Current.Key, styleLabelTimeLineDevice);
            timeLineDevicesNumber++;
        }
    }

    //Method to paint the recordings in the time line
    private void DataPainting()
    {
        VRPNDataObject toFront;
        bool previousState, flipRepaint;

        toFront = null;
        doRepaintDrag = false;
        flipRepaint = false;

        foreach (VRPNDataObject data in devicesData)
        {
            previousState = data.dragging;

            data.OnGUI(headerHeight, timeLineTime, timeLineZoom, timeLineProportion, data.Equals(inFront));

            if (data.dragging)
            {
                doRepaintDrag = true;

                if (devicesData.IndexOf(data) != devicesData.Count - 1)
                {
                    toFront = data;
                }
            }
            else if (previousState)
            {
                flipRepaint = true;
                inFront = data;
            }
        }

        // Move an object to front if needed
        if (toFront != null)        
        {
            devicesData.Remove(toFront);
            devicesData.Add(toFront);
        }

        // If some object just stopped being dragged, we should repaing for the state change
        if (flipRepaint)
        {
            Repaint();
        }
    }

    //Method to extract the editor updated state from the VRPNEditEditor
    private void DataFilling()
    {
        Dictionary<string, VRPNTrackerRecordings>.Enumerator eTrackers = VRPNEditEditor.Instance.GetTrackersEnumerator();
        Dictionary<string, VRPNAnalogRecordings>.Enumerator eAnalogs = VRPNEditEditor.Instance.GetAnalogsEnumerator();
        Dictionary<string, VRPNButtonRecordings>.Enumerator eButtons = VRPNEditEditor.Instance.GetButtonsEnumerator();
        devicesData = new List<VRPNDataObject>();
        timeLineDevicesNumber = 0;
        while (eTrackers.MoveNext())
        {
            List<VRPNTrackerRecording>.Enumerator eTracker = VRPNEditEditor.Instance.GetTrackerRecordingsEnumerator(eTrackers.Current.Key);
            while (eTracker.MoveNext())
            {
                devicesData.Add(new VRPNDataObject(VRPNManager.DeviceType.Tracker, eTrackers.Current.Key, eTracker.Current.name, eTracker.Current.reportTime, eTracker.Current.lastTime, timeLineLinesSeparation * timeLineDevicesNumber));
            }
            timeLineDevicesNumber++;
        }
        while (eAnalogs.MoveNext())
        {
            List<VRPNAnalogRecording>.Enumerator eAnalog = VRPNEditEditor.Instance.GetAnalogRecordingsEnumerator(eAnalogs.Current.Key);
            while (eAnalog.MoveNext())
            {
                devicesData.Add(new VRPNDataObject(VRPNManager.DeviceType.Analog, eAnalogs.Current.Key, eAnalog.Current.name, eAnalog.Current.reportTime, eAnalog.Current.lastTime, timeLineLinesSeparation * timeLineDevicesNumber));
            }
            timeLineDevicesNumber++;
        }
        while (eButtons.MoveNext())
        {
            List<VRPNButtonRecording>.Enumerator eButton = VRPNEditEditor.Instance.GetButtonRecordingsEnumerator(eButtons.Current.Key);
            while (eButton.MoveNext())
            {
                devicesData.Add(new VRPNDataObject(VRPNManager.DeviceType.Button, eButtons.Current.Key, eButton.Current.name, eButton.Current.reportTime, eButton.Current.lastTime, timeLineLinesSeparation * timeLineDevicesNumber));
            }
            timeLineDevicesNumber++;
        }
        inFront = null;
        Repaint();
    }
}