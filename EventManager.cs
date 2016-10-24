using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EVENT_TYPE { BLOCK_SELECTED, BLOCK_HOVER, BLOCK_NOHOVER, REVEAL_BLOCK };

public interface IListener
{
    void OnEvent(EVENT_TYPE event_type, Component sender, Object param = null);
}

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    private Dictionary<EVENT_TYPE, List<IListener>> listeners = new Dictionary<EVENT_TYPE, List<IListener>>();

    public static EventManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public void AddListener(EVENT_TYPE event_type, IListener listener)
    {
        List<IListener> listeningList = null;

        if (listeners.TryGetValue(event_type, out listeningList))
        {
            listeningList.Add(listener);
            return;
        }


        listeningList = new List<IListener>();
        listeningList.Add(listener);
        listeners.Add(event_type, listeningList);
    }

    public void RemoveEvent(EVENT_TYPE event_type)
    {
        listeners.Remove(event_type);
    }

    public void Notify(EVENT_TYPE event_type, Component sender, Object param = null)
    {
        List<IListener> listeningList = null;
        
        if(listeners.TryGetValue(event_type, out listeningList))
        {

            for (int i=0; i < listeningList.Count; i++)
            {
                if( !listeningList[ i ].Equals( null ) ) {
                    listeningList[ i ].OnEvent( event_type, sender, param );
                }
            }

        }
    }

    public void RemoveDuplicates()
    {
        Dictionary<EVENT_TYPE, List<IListener>> tmp = new Dictionary<EVENT_TYPE, List<IListener>>();

        foreach(KeyValuePair<EVENT_TYPE, List<IListener>> entry in listeners)
        {

            for (int i = 0; i < entry.Value.Count; i++)
            {
                if (entry.Value[i].Equals(null))
                {
                    Debug.Log( entry.Value.ToString() );
                    entry.Value.RemoveAt(i);              
                }
            }

            if (entry.Value.Count > 0)
            {
                tmp.Add(entry.Key, entry.Value);
            }

        }

        listeners = tmp;
    }

    void Start()
    {
        RemoveDuplicates();
    }
}