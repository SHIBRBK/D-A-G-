using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    protected static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null) { m_instance = (T)FindObjectOfType(typeof(T)); }
            if (m_instance == null) { Debug.LogError($"{typeof(T)} does not exist."); }
            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (this.CheckInstance()) { DontDestroyOnLoad(this.gameObject); }
    }

    private bool CheckInstance()
    {
        if (m_instance == null) { m_instance = (T)this; return true; }
        else if (Instance == this) { return true; }

        Destroy(this.gameObject);

        return false;
    }
}