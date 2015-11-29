/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Based on http://unity3d.com/es/learn/tutorials/modules/intermediate/live-training-archive/events-creating-simple-messaging-system
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNEventManager.cs
 *
 * usage: Must be added once in the scene to receive and send vrpn events. 
 * It comes in VRPNEventManager prefab.
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

//Unity Event for Button Report
public class VRPNButtonEvent : UnityEvent<string, VRPNButton.ButtonReport>
{
}

//Unity Event for Analog Report
public class VRPNAnalogEvent : UnityEvent<string, VRPNAnalog.AnalogReport>
{
}

//Unity Event for Tracker Report
public class VRPNTrackerEvent : UnityEvent<string, VRPNTracker.TrackerReport>
{
}

public class VRPNEventManager : MonoBehaviour
{
    //Events Dictionaries
    private Dictionary<string, VRPNButtonEvent> eventDictionaryButton;
    private Dictionary<string, VRPNAnalogEvent> eventDictionaryAnalog;
    private Dictionary<string, VRPNTrackerEvent> eventDictionaryTracker;

    //VRPNEventManager Singleton
    private static VRPNEventManager eventManager;

    //VRPNEventManager Singleton getter
    public static VRPNEventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(VRPNEventManager)) as VRPNEventManager;
                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    //Initialize Events Dictionaries
    void Init()
    {
        if (eventDictionaryButton == null)
        {
            eventDictionaryButton = new Dictionary<string, VRPNButtonEvent>(); 
        }
        if (eventDictionaryAnalog == null)
        {
            eventDictionaryAnalog = new Dictionary<string, VRPNAnalogEvent>();
        }
        if (eventDictionaryTracker == null)
        {
            eventDictionaryTracker = new Dictionary<string, VRPNTrackerEvent>();
        }
    }

    //Initialize VRPNEventManager in Awake
    void Awake()
    {
        //init VRPNEventManager
        #pragma warning disable 0219
        VRPNEventManager getInstance = VRPNEventManager.instance;
        #pragma warning restore 0219
    }

    //To add a listener for Buttons
    public static void StartListeningButton(VRPNManager.Button_Types deviceType, VRPNDeviceConfig.Device_Names deviceName, UnityAction<string, VRPNButton.ButtonReport> listener)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNButtonEvent thisEvent = null;
        if (instance.eventDictionaryButton.TryGetValue(deviceType.ToString() + " " + deviceName.ToString(), out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new VRPNButtonEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryButton.Add(deviceType.ToString() + " " + deviceName.ToString(), thisEvent);
        }
    }

    //To add a listener for Analog sensors
    public static void StartListeningAnalog(VRPNManager.Analog_Types deviceType, VRPNDeviceConfig.Device_Names deviceName, UnityAction<string, VRPNAnalog.AnalogReport> listener)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNAnalogEvent thisEvent = null;
        if (instance.eventDictionaryAnalog.TryGetValue(deviceType.ToString() + " " + deviceName.ToString(), out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new VRPNAnalogEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryAnalog.Add(deviceType.ToString() + " " + deviceName.ToString(), thisEvent);
        }
    }

    //To add a listener for Trackers
    public static void StartListeningTracker(VRPNManager.Tracker_Types deviceType, VRPNDeviceConfig.Device_Names deviceName, UnityAction<string, VRPNTracker.TrackerReport> listener)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNTrackerEvent thisEvent = null;
        if (instance.eventDictionaryTracker.TryGetValue(deviceType.ToString() + " " + deviceName.ToString(), out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new VRPNTrackerEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryTracker.Add(deviceType.ToString() + " " + deviceName.ToString(), thisEvent);
        }
    }

    //To remove a listener for Buttons
    public static void StopListeningButton(VRPNManager.Button_Types deviceType, VRPNDeviceConfig.Device_Names deviceName, UnityAction<string, VRPNButton.ButtonReport> listener)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNButtonEvent thisEvent = null;
        if (instance.eventDictionaryButton.TryGetValue(deviceType.ToString() + " " + deviceName.ToString(), out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //To remove a listener for Analog sensors
    public static void StopListeningAnalog(VRPNManager.Analog_Types deviceType, VRPNDeviceConfig.Device_Names deviceName, UnityAction<string, VRPNAnalog.AnalogReport> listener)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNAnalogEvent thisEvent = null;
        if (instance.eventDictionaryAnalog.TryGetValue(deviceType.ToString() + " " + deviceName.ToString(), out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //To remove a listener for Trackers
    public static void StopListeningTracker(VRPNManager.Tracker_Types deviceType, VRPNDeviceConfig.Device_Names deviceName, UnityAction<string, VRPNTracker.TrackerReport> listener)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNTrackerEvent thisEvent = null;
        if (instance.eventDictionaryTracker.TryGetValue(deviceType.ToString() + " " + deviceName.ToString(), out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    //To add a trigger for Buttons
    public static void TriggerEventButton(string deviceType, string deviceName, VRPNButton.ButtonReport report)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNButtonEvent thisEvent = null;
        if (instance.eventDictionaryButton.TryGetValue(deviceType + " " + deviceName, out thisEvent))
        {
            thisEvent.Invoke(deviceType + " " + deviceName, report);
        }
    }

    //To add a trigger for Analog sensors
    public static void TriggerEventAnalog(string deviceType, string deviceName, VRPNAnalog.AnalogReport report)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNAnalogEvent thisEvent = null;
        if (instance.eventDictionaryAnalog.TryGetValue(deviceType + " " + deviceName, out thisEvent))
        {
            thisEvent.Invoke(deviceType + " " + deviceName, report);
        }
    }

    //To add a trigger for Trackers
    public static void TriggerEventTracker(string deviceType, string deviceName, VRPNTracker.TrackerReport report)
    {
        if (eventManager == null)
        {
            Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            return;
        }
        VRPNTrackerEvent thisEvent = null;
        if (instance.eventDictionaryTracker.TryGetValue(deviceType + " " + deviceName, out thisEvent))
        {
            thisEvent.Invoke(deviceType + " " + deviceName, report);
        }
    }
}