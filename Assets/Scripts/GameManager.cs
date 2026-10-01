using UnityEngine;
using UnityEngine.Serialization;

// Composition root: holds the scene references and wires them into GameFlow.
public class GameManager : MonoBehaviour
{
    [SerializeField] private RewardsManager _rewardsManager;
    [FormerlySerializedAs("_im")]
    [SerializeField] private InputManager _input;
    [SerializeField] private ViewManager _vm;

    private GameFlow _gameFlow;

    void Awake()
    {
        _gameFlow = new GameFlow(_rewardsManager, _input, _vm);
    }

    void Start()
    {
        _gameFlow.Begin();
    }

    void OnDestroy()
    {
        _gameFlow?.Dispose();
    }
}
