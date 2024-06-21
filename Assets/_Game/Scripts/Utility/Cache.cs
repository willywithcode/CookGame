using UnityEngine;
using System.Collections.Generic;


public class Cache
{
    //------------------------------------------------------------------------------------------------------------
    private static Dictionary<float, WaitForSeconds> m_WFS = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWFS(float key)
    {
        if (!m_WFS.ContainsKey(key))
        {
            m_WFS[key] = new WaitForSeconds(key);
        }

        return m_WFS[key];
    }
    //------------------------------------------------------------------------------------------------------------
    
    
    private static Dictionary<int, IPickable> m_CollisionPickable = new Dictionary<int, IPickable>();
    public static bool GetCollisionPickable(Collision key, out IPickable obj)
    {
        if (!m_CollisionPickable.ContainsKey(key.gameObject.GetInstanceID()))
        {
            obj = key.gameObject.GetComponent<IPickable>();
            if (obj == null) return false;
            m_CollisionPickable[key.gameObject.GetInstanceID()] = obj;
        }

        obj = m_CollisionPickable[key.gameObject.GetInstanceID()];
        return true;
    }
}