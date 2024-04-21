using UnityEngine;
using UnityEngine.EventSystems;

public class StickFetch : MonoBehaviour, IPointerDownHandler
{
    public bool IsInUse { get; private set; }
    public Vector2 Position { get; private set; }

    public Vector2 Mult { get; set; }

    public Rect Clamp { get; set; } = new Rect(-1f, -1f, 2f, 2f);

    public float Radius { get; set; } = 50f;

    private int pointerId;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsInUse)
            return;

        IsInUse = true;
        pointerId = eventData.pointerId;
        UpdateStickInfo(eventData.pointerId, eventData.position);
    }

    public void Update()
    {
        if (IsInUse)
        {
            bool touchesFound = false;
            for (int x = 0; x < Input.touchCount; ++x)
            {
                var touch = Input.GetTouch(x);
                touchesFound |= UpdateStickInfo(touch.fingerId, touch.position);
            }

            if (!touchesFound)
            {
                IsInUse = false;
                Position = default;
            }
        }
    }

    bool UpdateStickInfo(int pId, Vector2 position)
    {
        if (pId != pointerId)
            return false;

        bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            position,
            null,
            out var value
        );

        if (!success)
            return false;

        Vector2 vector2 = value * Mult;
        var pos = vector2.normalized * Mathf.Min(vector2.magnitude, Radius) / Radius;
        pos.x = Mathf.Clamp(pos.x, Clamp.xMin, Clamp.xMax);
        pos.y = Mathf.Clamp(pos.y, Clamp.yMin, Clamp.yMax);

        Position = pos;
        return true;
    }
}
