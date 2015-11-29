/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Button Recording class to support VRPNEdit recordings playing
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNButtonRecording.cs
 *
 * usage: 
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class VRPNButtonRecording
{
    //Public properties
    public string name;
    public float reportTime;
    public Dictionary<int, int> buttons = new Dictionary<int, int>();
    public bool isPlaying = false;
    public float lastTime;

    //Private properties
    private VRPNButton.ButtonReports data;
    private bool firstReport = true;
    private float firstTime;
    private List<VRPNButton.ButtonReportNew>.Enumerator e;
    private VRPNButton.ButtonReportNew actualReport;

    //VRPNButtonRecording Constructor
    public VRPNButtonRecording(string nName, float nTime, VRPNButton.ButtonReports nData)
    {
        name = nName;
        reportTime = nTime;
        data = nData;

        e = data.list.GetEnumerator();

        while (e.MoveNext())
        {
            VRPNButton.ButtonReportNew report = e.Current;
            int test;
            if (!buttons.TryGetValue(report.button, out test))
            {
                buttons.Add(report.button, report.button);
            }
            lastTime = report.msg_time.tv_sec + (report.msg_time.tv_usec / 1000000f);
        }

        e = data.list.GetEnumerator();
    }

    //Public method that allows to start playing
    public void StartPlaying()
    {
        isPlaying = true;
    }

    //Public method that allows to update the playing state
    public void Update()
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
                actualReportTime = actualReport.msg_time.tv_sec + (actualReport.msg_time.tv_usec / 1000000f) + reportTime;
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

//Auxiliar class to store a list of recordings
[Serializable]
public class VRPNButtonRecordings
{
    public List<VRPNButtonRecording> recordings = new List<VRPNButtonRecording>();
}
