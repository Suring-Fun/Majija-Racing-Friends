using UnityEngine;

[CreateAssetMenu(menuName = "Car/Prizes/AutoShootPrizeCreator", fileName = "AutoShootPrizeCreator")]
public class AutoShootPrizeCreator : PrizeCreator
{
    [field: SerializeField]
    public Sprite Icon { get; private set; }
    
    [field: SerializeField]
    public AutoBullet Prefab { get; private set; }

    public override IPrize NewPrize(Transform car)
    {
        return new AutoShootPrize(
            car,
            Icon,
            Prefab
        );
    }
}
