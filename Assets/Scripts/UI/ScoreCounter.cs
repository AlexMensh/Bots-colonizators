using TMPro;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private Base _homeBase;

    private void Awake()
    {
        _homeBase = GetComponent<Base>();
    }

    private void OnEnable()
    {
        _homeBase.ScoreChanged += SetValue;
    }

    private void OnDisable()
    {
        _homeBase.ScoreChanged -= SetValue;
    }

    public void SetValue(int value)
    {
        _scoreText.text = value.ToString();
    }
}
