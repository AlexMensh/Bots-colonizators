using TMPro;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private BaseGatherer _baseGatherer;

    private void Awake()
    {
        _baseGatherer = GetComponent<BaseGatherer>();
    }

    private void OnEnable()
    {
        _baseGatherer.ScoreChanged += SetValue;
    }

    private void OnDisable()
    {
        _baseGatherer.ScoreChanged -= SetValue;
    }

    public void SetValue(int value)
    {
        _scoreText.text = value.ToString();
    }
}
