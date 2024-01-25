using UnityEngine;

public class StartLineMark : MonoBehaviour
{
    void Start()
    {
        var startLineInfo = FindObjectOfType<PathData>().GetStartLineInfo();
        transform.position = (Vector3)startLineInfo.point + Vector3.forward * transform.position.z;
        transform.eulerAngles = Vector3.forward * (Mathf.Atan2(-startLineInfo.dir.x, startLineInfo.dir.y) * Mathf.Rad2Deg);
    }
}
