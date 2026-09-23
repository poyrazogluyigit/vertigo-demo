using UnityEngine;
using DG.Tweening;

public class LevelPanelView : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    private Vector3 _initialPosition;
    void Awake()
    {
        _initialPosition = rt.anchoredPosition;
    }

    void OnEnable()
    {
        GameManager.LevelChanged += MoveLevelIndicator;
    }
    void OnDisable()
    {
        GameManager.LevelChanged -= MoveLevelIndicator;
    }

    void MoveLevelIndicator(int level)
    {
        float xDelta = 135;
        Vector3 target = _initialPosition + new Vector3(-(level - 1) * xDelta, 0, 0);
        Debug.Log(target);
        rt.DOAnchorPos(target, 1f);
    }
}