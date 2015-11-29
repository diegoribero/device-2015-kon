/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * 
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNEditEditor.cs
 *
 * usage: Class that represents the state of the VRPN Editor Window
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class VRPNEditEditor
{
    //Private properties
    //Device Dictionaries
    private Dictionary<string, VRPNTrackerRecordings> VRPNTrackerDevice = new Dictionary<string, VRPNTrackerRecordings>();
    private Dictionary<string, VRPNAnalogRecordings> VRPNAnalogDevice = new Dictionary<string, VRPNAnalogRecordings>();
    private Dictionary<string, VRPNButtonRecordings> VRPNButtonDevice = new Dictionary<string, VRPNButtonRecordings>();

    //VRPN Editor Window properties
    //Time line length
    private int timeLineTime;
    private int timeLineZoom;
    private int timeLineProportion;

    //VRPNEditEditor instance
    private static VRPNEditEditor instance;

    private VRPNEditEditor()
    {

    }

    //VRPNEditEditor getter
    public static VRPNEditEditor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new VRPNEditEditor();
            }
            return instance;
        }
    }

    /* ========================================================================
     * Methods to add recordings
     * ========================================================================*/

    //Public method that allows to add a tracker recording
    public void AddTracker(string name, float timeTracker, string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            VRPNTracker.TrackerReports data = (VRPNTracker.TrackerReports)bf.Deserialize(file);

            file.Close();

            VRPNTrackerRecording recording = new VRPNTrackerRecording(name, timeTracker, data);

            VRPNTrackerRecordings test;
            if (VRPNTrackerDevice.TryGetValue(data.deviceType + " " + data.deviceName, out test))
            {
                test.recordings.Add(recording);
            }
            else
            {
                test = new VRPNTrackerRecordings();
                test.recordings.Add(recording);
                VRPNTrackerDevice.Add(data.deviceType + " " + data.deviceName, test);
            }
        }
    }

    //Public method that allows to add an analog recording
    public void AddAnalog(string name, float timeAnalog, string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            VRPNAnalog.AnalogReports data = (VRPNAnalog.AnalogReports)bf.Deserialize(file);

            file.Close();

            VRPNAnalogRecording recording = new VRPNAnalogRecording(name, timeAnalog, data);

            VRPNAnalogRecordings test;
            if (VRPNAnalogDevice.TryGetValue(data.deviceType + " " + data.deviceName, out test))
            {
                test.recordings.Add(recording);
            }
            else
            {
                test = new VRPNAnalogRecordings();
                test.recordings.Add(recording);
                VRPNAnalogDevice.Add(data.deviceType + " " + data.deviceName, test);
            }
        }
    }

    //Public method that allows to add a button recording
    public void AddButton(string name, float timeButton, string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            VRPNButton.ButtonReports data = (VRPNButton.ButtonReports)bf.Deserialize(file);

            file.Close();

            VRPNButtonRecording recording = new VRPNButtonRecording(name, timeButton, data);

            VRPNButtonRecordings test;
            if (VRPNButtonDevice.TryGetValue(data.deviceType + " " + data.deviceName, out test))
            {
                test.recordings.Add(recording);
            }
            else
            {
                test = new VRPNButtonRecordings();
                test.recordings.Add(recording);
                VRPNButtonDevice.Add(data.deviceType + " " + data.deviceName, test);
            }
        }
    }

    /* ========================================================================
     * Methods to remove recordings
     * ========================================================================*/

    //Public method that allows to remove a tracker recording
    public void RemoveTracker(string name, float timeTracker, string tracker)
    {
        VRPNTrackerRecordings test;
        if (VRPNTrackerDevice.TryGetValue(tracker, out test))
        {
            List<VRPNTrackerRecording>.Enumerator e = test.recordings.GetEnumerator();
            VRPNTrackerRecording recording = null;
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeTracker && e.Current.name == name)
                {
                    recording = e.Current;
                    break;
                }
            }
            if (recording != null)
            {
                test.recordings.Remove(recording);
            }
            if (test.recordings.Count == 0)
            {
                VRPNTrackerDevice.Remove(tracker);
            }
        }
    }

    //Public method that allows to remove an analog recording
    public void RemoveAnalog(string name, float timeAnalog, string analog)
    {
        VRPNAnalogRecordings test;
        if (VRPNAnalogDevice.TryGetValue(analog, out test))
        {
            List<VRPNAnalogRecording>.Enumerator e = test.recordings.GetEnumerator();
            VRPNAnalogRecording recording = null;
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeAnalog && e.Current.name == name)
                {
                    recording = e.Current;
                    break;
                }
            }
            if (recording != null)
            {
                test.recordings.Remove(recording);
            }
            if (test.recordings.Count == 0)
            {
                VRPNAnalogDevice.Remove(analog);
            }
        }
    }

    //Public method that allows to remove a button recording
    public void RemoveButton(string name, float timeButton, string button)
    {
        VRPNButtonRecordings test;
        if (VRPNButtonDevice.TryGetValue(button, out test))
        {
            List<VRPNButtonRecording>.Enumerator e = test.recordings.GetEnumerator();
            VRPNButtonRecording recording = null;
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeButton && e.Current.name == name)
                {
                    recording = e.Current;
                    break;
                }
            }
            if (recording != null)
            {
                test.recordings.Remove(recording);
            }
            if (test.recordings.Count == 0)
            {
                VRPNButtonDevice.Remove(button);
            }
        }
    }

    /* ========================================================================
     * Methods to change the time for recordings
     * ========================================================================*/

    //Public method that allows to change the time for a tracker recording
    public void ChangeTimeTracker(string name, float timeTracker, float newTimeTracker, string tracker)
    {
        VRPNTrackerRecordings test;
        if (VRPNTrackerDevice.TryGetValue(tracker, out test))
        {
            List<VRPNTrackerRecording>.Enumerator e = test.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeTracker && e.Current.name == name)
                {
                    e.Current.reportTime = newTimeTracker;
                    break;
                }
            }
        }
    }

    //Public method that allows to change the time for an analog recording
    public void ChangeTimeAnalog(string name, float timeAnalog, float newTimeAnalog, string analog)
    {
        VRPNAnalogRecordings test;
        if (VRPNAnalogDevice.TryGetValue(analog, out test))
        {
            List<VRPNAnalogRecording>.Enumerator e = test.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeAnalog && e.Current.name == name)
                {
                    e.Current.reportTime = newTimeAnalog;
                    break;
                }
            }
        }
    }

    //Public method that allows to change the time for a button recording
    public void ChangeTimeButton(string name, float timeButton, float newTimeButton, string button)
    {
        VRPNButtonRecordings test;
        if (VRPNButtonDevice.TryGetValue(button, out test))
        {
            List<VRPNButtonRecording>.Enumerator e = test.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeButton && e.Current.name == name)
                {
                    e.Current.reportTime = newTimeButton;
                    break;
                }
            }
        }
    }

    /* ========================================================================
     * Methods to enable and disable sensors for a tracker recording
     * ========================================================================*/

    //Public method that allows to enable a sensor for a tracker recording   
    public void EnableTrackerSensor(string name, float timeTracker, string tracker, int sensor)
    {
        VRPNTrackerRecordings test;
        if (VRPNTrackerDevice.TryGetValue(tracker, out test))
        {
            List<VRPNTrackerRecording>.Enumerator e = test.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeTracker && e.Current.name == name)
                {
                    int testInt;
                    if (e.Current.sensorsDisabled.TryGetValue(sensor, out testInt))
                    {
                        e.Current.sensorsDisabled.Remove(sensor);
                    }
                    break;
                }
            }
        }
    }

    //Public method that allows to disable a sensor for a tracker recording   
    public void DisableTrackerSensor(string name, float timeTracker, string tracker, int sensor)
    {
        VRPNTrackerRecordings test;
        if (VRPNTrackerDevice.TryGetValue(tracker, out test))
        {
            List<VRPNTrackerRecording>.Enumerator e = test.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeTracker && e.Current.name == name)
                {
                    int testInt;
                    if (!e.Current.sensorsDisabled.TryGetValue(sensor, out testInt))
                    {
                        e.Current.sensorsDisabled.Add(sensor, sensor);
                    }
                    break;
                }
            }
        }
    }

    /* ========================================================================
     * Getters
     * ========================================================================*/

    //Method that returns a sensors' list of a given recording
    public Dictionary<int, int> GetSensors(string name, float timeTracker, string tracker)
    {
        VRPNTrackerRecordings test;
        if (VRPNTrackerDevice.TryGetValue(tracker, out test))
        {
            List<VRPNTrackerRecording>.Enumerator e = test.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeTracker && e.Current.name == name)
                {
                    return e.Current.sensors;
                }
            }
        }
        return null;
    }

    //Method that returns a sensors' disabled list of a given recording
    public Dictionary<int, int> GetDisabledSensors(string name, float timeTracker, string tracker)
    {
        VRPNTrackerRecordings test;
        if (VRPNTrackerDevice.TryGetValue(tracker, out test))
        {
            List<VRPNTrackerRecording>.Enumerator e = test.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.reportTime == timeTracker && e.Current.name == name)
                {
                    return e.Current.sensorsDisabled;
                }
            }
        }
        return null;
    }

    //Method that returns an enumerator of tracker devices
    public Dictionary<string, VRPNTrackerRecordings>.Enumerator GetTrackersEnumerator()
    {
        return VRPNTrackerDevice.GetEnumerator();
    }

    //Method that returns an enumerator of analog devices
    public Dictionary<string, VRPNAnalogRecordings>.Enumerator GetAnalogsEnumerator()
    {
        return VRPNAnalogDevice.GetEnumerator();
    }

    //Method that returns an enumerator of button devices
    public Dictionary<string, VRPNButtonRecordings>.Enumerator GetButtonsEnumerator()
    {
        return VRPNButtonDevice.GetEnumerator();
    }

    //Method that returns a recordings' enumerator of a given tracker device
    public List<VRPNTrackerRecording>.Enumerator GetTrackerRecordingsEnumerator(string name)
    {
        VRPNTrackerRecordings test = new VRPNTrackerRecordings();
        if (VRPNTrackerDevice.TryGetValue(name, out test))
        {
            return test.recordings.GetEnumerator();
        }
        return test.recordings.GetEnumerator();
    }

    //Method that returns a recordings' enumerator of a given analog device
    public List<VRPNAnalogRecording>.Enumerator GetAnalogRecordingsEnumerator(string name)
    {
        VRPNAnalogRecordings test = new VRPNAnalogRecordings();
        if (VRPNAnalogDevice.TryGetValue(name, out test))
        {
            return test.recordings.GetEnumerator();
        }
        return test.recordings.GetEnumerator();
    }

    //Method that returns a recordings' enumerator of a given button device
    public List<VRPNButtonRecording>.Enumerator GetButtonRecordingsEnumerator(string name)
    {
        VRPNButtonRecordings test = new VRPNButtonRecordings();
        if (VRPNButtonDevice.TryGetValue(name, out test))
        {
            return test.recordings.GetEnumerator();
        }
        return test.recordings.GetEnumerator();
    }

    //Method that returns the time property of the editor
    public int getTimeLineTime()
    {
        return timeLineTime;
    }

    //Method that returns the zoom property of the editor
    public int getTimeLineZoom()
    {
        return timeLineZoom;
    }

    //Method that returns the proportion property of the editor
    public int getTimeLineProportion()
    {
        return timeLineProportion;
    }

    /* ========================================================================
     * Methods to give functionality to the editor buttons
     * ========================================================================*/

    //Method that allows to start the playback of the editor recordings
    public void StartPlaying()
    {
        foreach (KeyValuePair<string, VRPNTrackerRecordings> pair in VRPNEditEditor.Instance.VRPNTrackerDevice)
        {
            List<VRPNTrackerRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.StartPlaying();
            }
        }
        foreach (KeyValuePair<string, VRPNAnalogRecordings> pair in VRPNEditEditor.Instance.VRPNAnalogDevice)
        {
            List<VRPNAnalogRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.StartPlaying();
            }
        }
        foreach (KeyValuePair<string, VRPNButtonRecordings> pair in VRPNEditEditor.Instance.VRPNButtonDevice)
        {
            List<VRPNButtonRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.StartPlaying();
            }
        }
    }

    //Method that allows to stop the playback of the editor recordings
    public void StopPlaying()
    {
        foreach (KeyValuePair<string, VRPNTrackerRecordings> pair in VRPNEditEditor.Instance.VRPNTrackerDevice)
        {
            List<VRPNTrackerRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.StopPlaying();
            }
        }
        foreach (KeyValuePair<string, VRPNAnalogRecordings> pair in VRPNEditEditor.Instance.VRPNAnalogDevice)
        {
            List<VRPNAnalogRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.StopPlaying();
            }
        }
        foreach (KeyValuePair<string, VRPNButtonRecordings> pair in VRPNEditEditor.Instance.VRPNButtonDevice)
        {
            List<VRPNButtonRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.StopPlaying();
            }
        }
    }

    //Method that allows to save on a file the actual editor's state
    public void SaveState(string path)
    {
        EditorState state = new EditorState();
        state.VRPNTrackerDevice = VRPNTrackerDevice;
        state.VRPNAnalogDevice = VRPNAnalogDevice;
        state.VRPNButtonDevice = VRPNButtonDevice;
        state.timeLineTime = timeLineTime;
        state.timeLineZoom = timeLineZoom;
        state.timeLineProportion = timeLineProportion;

        BinaryFormatter bf = new BinaryFormatter();
        bool canEdit = false;
        FileStream file = null;
        while (!canEdit)
        {
            try
            {
                file = File.Create(path);
                canEdit = true;
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore 0168
            {
                canEdit = false;
            }
        }

        bf.Serialize(file, state);
        file.Close();
    }

    //Method that allows to load from a file a saved editor's state
    public void LoadState(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bool canEdit = false;
            FileStream file = null;
            while (!canEdit)
            {
                try
                {
                    file = File.Open(path, FileMode.Open);
                    canEdit = true;
                }
#pragma warning disable 0168
                catch (Exception e)
#pragma warning restore 0168
                {
                    canEdit = false;
                }
            }

            EditorState state = (EditorState)bf.Deserialize(file);

            file.Close();

            VRPNTrackerDevice = state.VRPNTrackerDevice;
            VRPNAnalogDevice = state.VRPNAnalogDevice;
            VRPNButtonDevice = state.VRPNButtonDevice;
            timeLineTime = state.timeLineTime;
            timeLineZoom = state.timeLineZoom;
            timeLineProportion = state.timeLineProportion;
        }
    }

    //Method that allows to reset the recordings' list
    public void Clear()
    {
        VRPNTrackerDevice = new Dictionary<string, VRPNTrackerRecordings>();
        VRPNAnalogDevice = new Dictionary<string, VRPNAnalogRecordings>();
        VRPNButtonDevice = new Dictionary<string, VRPNButtonRecordings>();
    }

    //Method that allows to change the editor properties (just before saving or loading)
    public void ChangeTimeLineData(int nTimeLineTime, int nTimeLineZoom, int nTimeLineProportion)
    {
        timeLineTime = nTimeLineTime;
        timeLineZoom = nTimeLineZoom;
        timeLineProportion = nTimeLineProportion;
    }

    /* ========================================================================
     * Method to update the recordings playback state
     * ========================================================================*/

    public bool Update()
    {
        bool keepPlaying = false;
        foreach (KeyValuePair<string, VRPNTrackerRecordings> pair in VRPNEditEditor.Instance.VRPNTrackerDevice)
        {
            List<VRPNTrackerRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Update();
                keepPlaying = keepPlaying || e.Current.isPlaying;
            }
        }
        foreach (KeyValuePair<string, VRPNAnalogRecordings> pair in VRPNEditEditor.Instance.VRPNAnalogDevice)
        {
            List<VRPNAnalogRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Update();
                keepPlaying = keepPlaying || e.Current.isPlaying;
            }
        }
        foreach (KeyValuePair<string, VRPNButtonRecordings> pair in VRPNEditEditor.Instance.VRPNButtonDevice)
        {
            List<VRPNButtonRecording>.Enumerator e = pair.Value.recordings.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Update();
                keepPlaying = keepPlaying || e.Current.isPlaying;
            }
        }
        if (!keepPlaying)
        {
            StopPlaying();
        }
        return keepPlaying;
    }
}

//Auxiliar class to store the editor state
[Serializable]
public class EditorState
{
    public Dictionary<string, VRPNTrackerRecordings> VRPNTrackerDevice = new Dictionary<string, VRPNTrackerRecordings>();
    public Dictionary<string, VRPNAnalogRecordings> VRPNAnalogDevice = new Dictionary<string, VRPNAnalogRecordings>();
    public Dictionary<string, VRPNButtonRecordings> VRPNButtonDevice = new Dictionary<string, VRPNButtonRecordings>();
    public int timeLineTime;
    public int timeLineZoom;
    public int timeLineProportion;
}