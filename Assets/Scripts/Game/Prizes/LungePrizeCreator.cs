using UnityEngine;

[CreateAssetMenu(menuName = "Car/Prizes/LungePrizeCreator", fileName = "LungePrizeCreator")]
public class LungePrizeCreator : PrizeCreator
{
    [field: SerializeField]
    private Sprite[] m_icons;

    [field: SerializeField]
    public float Radius { get; private set; }

    [field: SerializeField]
    public float Duration { get; private set; }

    [field: SerializeField]
    public int Count { get; private set; } = 3;

    [field: SerializeField]
    public GameObject AudioInstance { get; private set; }

    [field: SerializeField]
    public float AudioLifeTime { get; private set; } = 1f;

    public override IPrize NewPrize(Transform car)
    {
        return new LungePrize(car,
        m_icons,
        Radius,
        Duration,
        Count,
        AudioInstance,
        AudioLifeTime
        );
    }
}
