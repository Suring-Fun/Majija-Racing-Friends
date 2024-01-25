using UnityEngine;

[CreateAssetMenu(menuName = "Car/Prizes/ShootPrizeCreator", fileName = "ShootPrizeCreator")]
public class ShootPrizeCreator : PrizeCreator
{
    [field: SerializeField]
    private Sprite[] m_icons;

    [field: SerializeField]
    public float Speed { get; private set; } = 20f;

    [field: SerializeField]
    public int Count { get; private set; } = 3;

    [field: SerializeField]
    public Bullet BulletPerfab { get; private set; }

    public override IPrize NewPrize(Transform car)
    {
        return new ShootPrize(
        car,
        m_icons,
        Speed,
        Count,
        BulletPerfab
        );
    }
}
