/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Based on http://unity3d.com/es/learn/tutorials/modules/beginner/live-training-archive/persistence-data-saving-loading
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNButtonPlay.cs
 *
 * usage: Must be added once for each button that is desired to play.
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class VRPNButtonPlay : MonoBehaviour
{
    //Public properties
    public string path;
    public bool isPlaying = false;

    //Private properties
    private VRPNButton.ButtonReports data;
    private bool firstReport = true;
    private float firstTime;
    private List<VRPNButton.ButtonReportNew>.Enumerator e;
    private VRPNButton.ButtonReportNew actualReport;

    //Public method that allows to start playing
    //It reads the data from the indicated path
    public void StartPlaying()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            data = (VRPNButton.ButtonReports)bf.Deserialize(file);

            file.Close();

            isPlaying = true;

            e = data.list.GetEnumerator();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            float actualTime;
            float actualReportTime = 0f;
            bool moreReports = true;
            bool alreadyAdvanced = false;
            Dictionary<int, VRPNButton.ButtonReportNew> lastReports = new Dictionary<int, VRPNButton.ButtonReportNew>();

            if (firstReport)
            {
                firstReport = false;
                firstTime = Time.time;
                if (e.MoveNext())
                {
                    actualReport = e.Current;
                }
                else
                {
                    isPlaying = false;
                    moreReports = false;
                    firstReport = true;
                }
            }

            actualTime = Time.time - firstTime;

            //It seeks the last appropiate report for the actual time
            while (moreReports)
            {
                actualReportTime = actualReport.msg_time.tv_sec + (actualReport.msg_time.tv_usec / 1000000f);
                if (actualReportTime <= actualTime)
                {
                    VRPNButton.ButtonReportNew test;
                    if (lastReports.TryGetValue(actualReport.button, out test))
                    {
                        lastReports[actualReport.button] = actualReport;
                    }
                    else
                    {
                        lastReports.Add(actualReport.button, actualReport);
                    }
                    if (e.MoveNext())
                    {
                        actualReport = e.Current;
                        alreadyAdvanced = true;
                    }
                    else
                    {
                        sendingReports(lastReports);

                        moreReports = false;
                        isPlaying = false;
                        firstReport = true;
                    }
                }
                else if (alreadyAdvanced)
                {
                    sendingReports(lastReports);

                    moreReports = false;
                }
                else
                {
                    moreReports = false;
                }
            }
        }
    }

    //Auxiliar method that sends last frame report for each button
    private void sendingReports(Dictionary<int, VRPNButton.ButtonReportNew> lastReports)
    {
        foreach (KeyValuePair<int, VRPNButton.ButtonReportNew> pair in lastReports)
        {
            VRPNButton.ButtonReport newReport = new VRPNButton.ButtonReport();
            VRPNManager.TimeVal newMsgTime = new VRPNManager.TimeVal();
            newMsgTime.tv_sec = (UInt32)pair.Value.msg_time.tv_sec;
            newMsgTime.tv_usec = (UInt32)pair.Value.msg_time.tv_usec;
            newReport.msg_time = newMsgTime;
            newReport.button = pair.Value.button;
            newReport.state = pair.Value.state;
            VRPNEventManager.TriggerEventButton(data.deviceType, data.deviceName, newReport);
        }
    }

    //Public method that allows to stop playing
    public void StopPlaying()
    {
        isPlaying = false;
        firstReport = true;
        e = data.list.GetEnumerator();
    }
}
