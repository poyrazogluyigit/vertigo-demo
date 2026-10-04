using DG.Tweening;
using UnityEngine;

namespace WheelSpin
{
    public class RewardListItem : UIItem
    {
        protected override Tween Effect()
        {
            DOTween.Kill(this, true);
            return DOTween.Sequence().SetTarget(this).SetLink(gameObject)
                .Join(transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 6))
                .Join(DOTween.To(() => PreviousAmount, ShowAmount, Amount, 0.4f));
        }
    }
}
