using UnityEngine;

[CreateAssetMenu(menuName = "Car/Prizes/PuddlePrizeCreator", fileName = "PuddlePrizeCreator")]
public class PuddlePrizeCreator : PrizeCreator
{
    [field: SerializeField]
    private Sprite[] m_icons;

    [field: SerializeField]
    public OilBullet OilBulletPrefab { get; private set; }

    [field: SerializeField]
    public int Count { get; private set; } = 3;

    [field: SerializeField]
    public float MinDistance { get; private set; } = 5f;

    [field: SerializeField]
    public float MaxDistance { get; private set; } = 5f;

    [field: SerializeField]
    public float FlyDuration { get; private set; } = 0.3f;



    public override IPrize NewPrize(Transform car)
    {
        return new PuddlePrize(car, m_icons, OilBulletPrefab, Count, MinDistance, MaxDistance, FlyDuration);
    }
}
