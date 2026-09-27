using UnityEngine;

[CreateAssetMenu(menuName = "WheelSpin/End Screen")]
public class EndScreen : ScriptableObject
{
    public string Title;
    [TextArea] public string Description;
    public string ButtonLabel;
    public Sprite ButtonSprite;
    public bool ShowsRewards;   // true shows the reward list, false shows the bomb card
}
