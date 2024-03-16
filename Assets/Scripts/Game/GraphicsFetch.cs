
using UnityEngine;

public class GraphicsFetch : MonoBehaviour
{
    public string KindOfGraphics;

    public void Awake() {
        Instantiate(
            GraphicsProvider.GetProviderFromScene(KindOfGraphics).SelectRandomGraphics(),
            transform, false
            );
    }
}
