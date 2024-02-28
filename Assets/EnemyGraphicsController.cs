using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraphicsController : MonoBehaviour
{
    public GameObject RunningVer, FalledVer;
    private RescueableCar m_rescuable;
    private ShockableCar m_shockable;

    // Start is called before the first frame update
    void Start()
    {
        m_rescuable = GetComponentInParent<RescueableCar>();
        m_shockable = GetComponentInParent<ShockableCar>();
    }

    // Update is called once per frame
    void Update()
    {
        bool falled = m_rescuable.IsResquing || m_shockable.IsShocked;
        RunningVer.SetActive(!falled);
        FalledVer.SetActive(falled);

    }
}
