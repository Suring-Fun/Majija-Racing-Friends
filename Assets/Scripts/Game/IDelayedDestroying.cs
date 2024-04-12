using UnityEngine;

interface IPreDestroying
{
    public void OnNotifiedObjectAboutDeath();

    public static void NotifyObjectAboutDeath(GameObject root)
    {
        foreach (var o in root.GetComponentsInChildren<IPreDestroying>())
            o.OnNotifiedObjectAboutDeath();
    }
}
