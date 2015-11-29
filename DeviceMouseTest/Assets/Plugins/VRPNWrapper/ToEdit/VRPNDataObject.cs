/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Based on:
 * http://angryant.com/2009/09/18/gui-drag-drop/
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNDataObject.cs
 *
 * usage: Class that represents one VRPN Data Object (Recording) in the editor time line
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;

public class VRPNDataObject
{
    //Public properties (Allows to identify where the data comes from)
    public VRPNManager.DeviceType dataType;
    public string dataDevice;
    public string dataName;
    public float originalDataTime;
    public float dataSize;

    //Private properties
    public bool dragging;
    private float dataTime;
    private float dataPositionX;
    private float dataPositionY;
    private float dragStart;
    

    public VRPNDataObject(VRPNManager.DeviceType nDataType, string nDataDevice, string nDataName, float nDataTime, float nDataSize, float nDataPosition)
    {
        dataType = nDataType;
        dataDevice = nDataDevice;
        dataName = nDataName;
        dataTime = originalDataTime = nDataTime;
        dataSize = nDataSize;
        dataPositionY = nDataPosition;
    }

    public void OnGUI(float headerHeight, float timeLineTime, float timeLineZoom, float timeLineProportion, bool selected)
    {
        dataPositionX = dataTime * timeLineZoom * timeLineProportion;
        Rect drawRect = new Rect(dataPositionX, dataPositionY, dataSize * timeLineZoom * timeLineProportion, headerHeight);
        Color color = GUI.color;
        //Data object if selected
        if (selected)
        {
            GUI.color = Color.red;
            if (dataSize * timeLineZoom * timeLineProportion < 20)
            {
                GUI.color = color;
                GUI.Box(new Rect(dataPositionX + (dataSize * timeLineZoom * timeLineProportion) / 2 - 10, dataPositionY, 20, headerHeight), "");
                GUI.color = Color.red;
            }
            GUI.Box(drawRect, dataName);
        }
        //Data object if is not selected
        else
        {
            GUI.color = Color.cyan;
            if (dataSize * timeLineZoom * timeLineProportion < 20)
            {
                GUI.color = color;
                GUI.Box(new Rect(dataPositionX + (dataSize * timeLineZoom * timeLineProportion) / 2 - 10, dataPositionY, 20, headerHeight), "");
                GUI.color = Color.cyan;
            }
            GUI.Box(drawRect, dataName);
        }
        //Special treatment if it is too small
        if (dataSize * timeLineZoom * timeLineProportion < 20)
        {
            drawRect = new Rect(dataPositionX + (dataSize * timeLineZoom * timeLineProportion) / 2 - 10, dataPositionY, 20, headerHeight);
        }

        //What to do if dragging
        if (Event.current.type == EventType.MouseUp)
        {
            dragging = false;
            switch (dataType)
            {
                case VRPNManager.DeviceType.Tracker:
                    VRPNEditEditor.Instance.ChangeTimeTracker(dataName, originalDataTime, dataTime, dataDevice);
                    break;
                case VRPNManager.DeviceType.Analog:
                    VRPNEditEditor.Instance.ChangeTimeAnalog(dataName, originalDataTime, dataTime, dataDevice);
                    break;
                case VRPNManager.DeviceType.Button:
                    VRPNEditEditor.Instance.ChangeTimeButton(dataName, originalDataTime, dataTime, dataDevice);
                    break;
                default:
                    break;
            }
            originalDataTime = dataTime;

        }
        else if (Event.current.type == EventType.MouseDown && drawRect.Contains(Event.current.mousePosition))
        {
            dragging = true;
            dragStart = Event.current.mousePosition.x - dataPositionX;
            Event.current.Use();
        }

        if (dragging && !VRPNEdit.instance.isPlaying)
        {
            dataTime = (Event.current.mousePosition.x - dragStart) / (timeLineZoom * timeLineProportion);
            if (dataTime < 0)
            {
                dataTime = 0;
            }
            else if (dataTime + dataSize > timeLineTime)
            {
                dataTime = timeLineTime - dataSize;
            }
        }
        GUI.color = color;
    }

    //Method that allows to delete a data object from the editor state
    public void deleteDataObject()
    {
        switch (dataType)
        {
            case VRPNManager.DeviceType.Tracker:
                VRPNEditEditor.Instance.RemoveTracker(dataName, originalDataTime, dataDevice);
                break;
            case VRPNManager.DeviceType.Analog:
                VRPNEditEditor.Instance.RemoveAnalog(dataName, originalDataTime, dataDevice);
                break;
            case VRPNManager.DeviceType.Button:
                VRPNEditEditor.Instance.RemoveButton(dataName, originalDataTime, dataDevice);
                break;
            default:
                break;
        }
    }
}
