using UnityEngine;

interface IPreDestroying
{
    public void NotifyObjectAboutDeath();

    public static void NotifyObjectAboutDeath(GameObject root)
    {
        foreach (var o in root.GetComponentsInChildren<IPreDestroying>())
            o.NotifyObjectAboutDeath();
    }
}
