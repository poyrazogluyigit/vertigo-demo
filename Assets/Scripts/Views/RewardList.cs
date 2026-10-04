using UnityEngine;

namespace WheelSpin
{
    public class RewardList : View
    {
        [SerializeField] private RewardListItem _itemPrefab;
        [SerializeField] private bool _drawOnRewardsChanged = true;   // off for the cash-out list, which EndgameView draws

        protected override void Subscribe()
        {
            if (_drawOnRewardsChanged) Bus.Subscribe<RewardsChanged>(OnRewardsChanged);
        }

        protected override void Unsubscribe()
        {
            if (_drawOnRewardsChanged) Bus.Unsubscribe<RewardsChanged>(OnRewardsChanged);
        }

        void OnRewardsChanged(RewardsChanged e) => Draw(e.Earned);

        public void Draw(Reward[] rewards)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                RewardListItem item = i < transform.childCount
                    ? transform.GetChild(i).GetComponent<RewardListItem>()
                    : Instantiate(_itemPrefab, transform);
                item.gameObject.SetActive(true);
                item.Display(rewards[i].Definition.Image, rewards[i].Amount);
            }

            // Hidden rather than destroyed: layout groups skip inactive children,
            // and they are reused the next time the list grows.
            for (int i = rewards.Length; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }

    #if UNITY_EDITOR
        void OnValidate()
        {
            if (_itemPrefab == null) Debug.LogError($"{name}: item prefab is not assigned!", this);
        }

        // Layout stress test: every icon aspect (1:1 up to 3:1) with 1- to 5-digit amounts.
        // Play mode only, so the scene never gets the spawned rows saved into it.
        [ContextMenu("Debug/Fill 15 rewards")]
        void DebugFill()
        {
            const int count = 15;
            int[] amounts = { 1, 25, 950, 1_290, 16_000 };

            RewardDefinition[] definitions = System.Array.ConvertAll(
                UnityEditor.AssetDatabase.FindAssets("t:" + nameof(RewardDefinition)),
                guid => UnityEditor.AssetDatabase.LoadAssetAtPath<RewardDefinition>(
                    UnityEditor.AssetDatabase.GUIDToAssetPath(guid)));
            definitions = System.Array.FindAll(definitions, d => !d.IsBomb && d.Image != null);
            System.Array.Sort(definitions, (a, b) => Aspect(a).CompareTo(Aspect(b)));

            // Evenly spaced over the aspect-sorted list, so the squarest and the widest icons are both in
            var rewards = new Reward[count];
            for (int i = 0; i < count; i++)
                rewards[i] = new Reward(definitions[i * (definitions.Length - 1) / (count - 1)], amounts[i % amounts.Length]);

            Draw(rewards);
        }

        [ContextMenu("Debug/Fill 15 rewards", true)]
        bool CanDebugFill() => Application.isPlaying;

        static float Aspect(RewardDefinition d) => d.Image.rect.width / d.Image.rect.height;
    #endif
    }
}
