using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGraphicsController : MonoBehaviour
{
    private ShockableCar m_shockable;

    public Animator BasisAnimator;

    public string IsInWaterAnimatorBool = "IsInWater";

    public GameObject Dust;
    private RescueableCar m_resquable;

    void Awake()
    {
        m_resquable = GetComponentInParent<RescueableCar>();
        //m_shockable = GetComponentInParent<ShockableCar>();
    }

    void LateUpdate()
    {

        Dust.SetActive(!m_resquable.IsSwimming && m_resquable.IsResquing);
        BasisAnimator.SetBool(IsInWaterAnimatorBool, m_resquable.IsSwimming);
    }
}
