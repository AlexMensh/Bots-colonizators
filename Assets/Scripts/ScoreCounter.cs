using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    private int _score;

    private void Update()
    {
        _scoreText.text = _score.ToString();
    }

    public void AddPoint()
    {
        _score += 1;
    }
}
