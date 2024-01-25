using UnityEngine;

[CreateAssetMenu(menuName = "Car/Prizes/SpeedUpPrizeCreator", fileName = "SpeedUpPrizeCreator")]
public class SpeedUpPrizeCreator : PrizeCreator
{
    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    [Tooltip("Duration in the seconds")]
    public float Duration { get; private set; } = 5f;

    [field: SerializeField]
    public float MoveMultFactor { get; private set; } = 2f;

    [field: SerializeField]
    public float RotateMultFactor { get; private set; } = 1.25f;

    public override IPrize NewPrize(Transform car)
    {
        return new SpeedUpPrize(
            car,
            Icon,
             1f / Duration,
            MoveMultFactor,
            RotateMultFactor
        );
    }
}