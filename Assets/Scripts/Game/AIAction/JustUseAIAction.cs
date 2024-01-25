using System;
using System.Collections;
using UnityEngine;

public class JustUseAIAction : AIAction
{
    public override bool CanHandle()
    {
        if (Host.Prize is null)
            return false;
            
        return Host.Prize.ApplyMode == IPrize.PrizeApplyMode.JustApply;
    }

    public override Func<float, bool> CreateHandler()
    {
        float time = 1f;
        return deltaTime =>
        {
            Host.DisableApplyView();

            if (Host.Prize is null)
                return false;

            if (!Host.PrizeAccessed)
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
