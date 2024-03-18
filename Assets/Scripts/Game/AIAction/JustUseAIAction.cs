using System;
using System.Collections;
using UnityEngine;

public class JustUseAIAction : AIAction
{
    public override bool CanHandle()
    {
        if (Host.MainPrize is null)
            return false;
            
        return Host.MainPrize.ApplyMode == IPrize.PrizeApplyMode.JustApply;
    }

    public override Func<float, bool> CreateHandler()
    {
        float time = 1f;
        return deltaTime =>
        {
            Host.DisableApplyView();

            if (Host.MainPrize is null)
                return false;

            if (!Host.MainPrizeAccessed)
                return true;

            time -= deltaTime;
            if (time < 0f)
            {
                Host.ApplyPrize(Vector2.up);
                return false;
            }

            Host.EnableApplyPreview(Vector2.up);
            return true;
        };
    }
}
