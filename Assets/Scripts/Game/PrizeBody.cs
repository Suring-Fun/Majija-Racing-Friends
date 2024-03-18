using System;
using System.Collections;
using UnityEngine;

public interface IPrize
{
    enum UpdateResult
    {
        None = 0,
        PrizeRetired = 1
    }

    enum PrizeApplyMode
    {
        JustApply = 0,
        TakeAim = 1
    }

    Sprite Icon { get; }

    public bool IsApplyable { get; }

    public bool IsReplaceable { get; }

    public float Amount { get; }

    public PrizeApplyMode ApplyMode { get; }

    void Apply(Vector2 direction);

    UpdateResult Update(float delata);

    float PreviewDistance(Vector2 direction);

    event Action<IPrize> PrizeChanged;
}

public abstract class PrizeCreator : ScriptableObject
{
    [field: SerializeField]
    private float[] ProbabilityFromLastToFirst;

    public float CalculateProbabilityForCurrentPlace(int curr, int total)
    {
        int currInd = curr * (ProbabilityFromLastToFirst.Length - 1) / (total - 1);
        return ProbabilityFromLastToFirst[ProbabilityFromLastToFirst.Length - 1 - currInd];
    }

    public abstract IPrize NewPrize(Transform car);
}

public class PrizeBody : MonoBehaviour
{
    [field: SerializeField]
    PrizeCreator[] Prizes = new PrizeCreator[0];

    public float TimeToRecover = 10f;

    public GameObject Graphics;

    private bool m_used;

    private IEnumerator DoUse()
    {
        m_used = true;
        Graphics.SetActive(false);

        yield return new WaitForSeconds(TimeToRecover);
        m_used = false;
        Graphics.SetActive(true);

    }

    public IPrize TryUse(Transform car)
    {
        if (m_used)
            return null;

        StartCoroutine(DoUse());
        var rpt = car.GetComponent<RoadPositionTracker>();
        (int my, int total) = rpt.CalculatePlace();

        float rnd = UnityEngine.Random.value;
        float left = 0f;

        for (int x = 0; x < Prizes.Length; ++x)
        {
            left += rnd * Prizes[x].CalculateProbabilityForCurrentPlace(my, total);
        }

        for (int x = 0; x < Prizes.Length; ++x)
        {
            var prob = Prizes[x].CalculateProbabilityForCurrentPlace(my, total);
            if (left <= prob)
                return Prizes[x].NewPrize(car);

            left -= prob;
        }

        return Prizes[Prizes.Length - 1].NewPrize(car);
    }
}
