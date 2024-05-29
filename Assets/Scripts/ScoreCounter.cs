using TMPro;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private Base _homeBase;
    private int _score;

    private void Awake()
    {
        _homeBase = GetComponent<Base>();
    }

    private void OnEnable()
    {
        _homeBase.ScoreChanged += AddPoint;
    }

    private void OnDisable()
    {
        _homeBase.ScoreChanged -= AddPoint;
    }

    public void AddPoint()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
}
