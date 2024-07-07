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
    
    private static Dictionary<int, IPickable> m_TriggerPickable = new Dictionary<int, IPickable>();
    public static bool GetTriggerPickable(Collider key, out IPickable obj)
    {
        if (!m_TriggerPickable.ContainsKey(key.gameObject.GetInstanceID()))
        {
            obj = key.gameObject.GetComponent<IPickable>();
            if (obj == null) return false;
            m_TriggerPickable[key.gameObject.GetInstanceID()] = obj;
        }

        obj = m_TriggerPickable[key.gameObject.GetInstanceID()];
        return true;
    }
    private static Dictionary<Collider, Stove> m_ColliderStove = new Dictionary<Collider, Stove>();
    public static bool GetColliderStove(Collider key, out Stove obj)
    {
        if (!m_ColliderStove.ContainsKey(key))
        {
            obj = key.GetComponent<Stove>();
            if (obj == null) return false;
            m_ColliderStove[key] = obj;
        }

        obj = m_ColliderStove[key];
        return true;
    }
    private static Dictionary<Collider, Tillage> m_ColliderTillage = new Dictionary<Collider, Tillage>();
    public static bool GetColliderTillage(Collider key, out Tillage obj)
    {
        if (!m_ColliderTillage.ContainsKey(key))
        {
            obj = key.GetComponent<Tillage>();
            if (obj == null) return false;
            m_ColliderTillage[key] = obj;
        }

        obj = m_ColliderTillage[key];
        return true;
    }
    
    private static Dictionary<Collider, TeleDoor> m_ColliderTeleDoor = new Dictionary<Collider, TeleDoor>();
    public static bool GetColliderTeleDoor(Collider key, out TeleDoor obj)
    {
        if (!m_ColliderTeleDoor.ContainsKey(key))
        {
            obj = key.GetComponent<TeleDoor>();
            if (obj == null) return false;
            m_ColliderTeleDoor[key] = obj;
        }

        obj = m_ColliderTeleDoor[key];
        return true;
    }
    private static Dictionary<Collider, Shop> m_ColliderShop = new Dictionary<Collider, Shop>();
    public static bool GetColliderShop(Collider key, out Shop obj)
    {
        if (!m_ColliderShop.ContainsKey(key))
        {
            obj = key.GetComponent<Shop>();
            if (obj == null) return false;
            m_ColliderShop[key] = obj;
        }

        obj = m_ColliderShop[key];
        return true;
    }
    private static Dictionary<Collider,OrderContainer> m_ColliderOrderContainer = new Dictionary<Collider, OrderContainer>();
    public static bool GetColliderOrderContainer(Collider key, out OrderContainer obj)
    {
        if (!m_ColliderOrderContainer.ContainsKey(key))
        {
            obj = key.GetComponent<OrderContainer>();
            if (obj == null) return false;
            m_ColliderOrderContainer[key] = obj;
        }

        obj = m_ColliderOrderContainer[key];
        return true;
    }
    private static Dictionary<Collider, KnifeTable> m_ColliderKnifeTable = new Dictionary<Collider, KnifeTable>();
    
    public static bool GetColliderKnifeTable(Collider key, out KnifeTable obj)
    {
        if (!m_ColliderKnifeTable.ContainsKey(key))
        {
            obj = key.GetComponent<KnifeTable>();
            if (obj == null) return false;
            m_ColliderKnifeTable[key] = obj;
        }

        obj = m_ColliderKnifeTable[key];
        return true;
    }
    private static Dictionary<Collider, TrashBin> m_ColliderTrashBin = new Dictionary<Collider, TrashBin>();
    public static bool GetColliderTrashBin(Collider key, out TrashBin obj)
    {
        if (!m_ColliderTrashBin.ContainsKey(key))
        {
            obj = key.GetComponent<TrashBin>();
            if (obj == null) return false;
            m_ColliderTrashBin[key] = obj;
        }

        obj = m_ColliderTrashBin[key];
        return true;
    }
    private static Dictionary<Collider, FishingSite> m_ColliderFishingSite = new Dictionary<Collider, FishingSite>();
    public static bool GetColliderFishingSite(Collider key, out FishingSite obj)
    {
        if (!m_ColliderFishingSite.ContainsKey(key))
        {
            obj = key.GetComponent<FishingSite>();
            if (obj == null) return false;
            m_ColliderFishingSite[key] = obj;
        }

        obj = m_ColliderFishingSite[key];
        return true;
    }
}