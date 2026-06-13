using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

  [Header("Configurações do Jogo")]
  [SerializeField] private int _totalItemsInLevel;
  private int _collectedCount = 0;

  [Header("Referências")]
  [SerializeField] private GameObject _finishLine;

  [Header("Configurações de UI")]
  [SerializeField] private TextMeshProUGUI _scoreTxt;
  [SerializeField] private GameObject _victoryPanel;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
    }

    if (_finishLine != null) _finishLine.SetActive(false);
    if (_victoryPanel != null) _victoryPanel.SetActive(false);

    UpdateScoreUI();
  }

  public void AddCollectible()
  {
    _collectedCount++;
    Debug.Log($"Coletou {_collectedCount} / {_totalItemsInLevel}");
    UpdateScoreUI();

    if (_collectedCount >= _totalItemsInLevel)
    {
      UnlockFinishLine();
    }
  }

  private void UnlockFinishLine()
  {
    if (_finishLine != null)
    {
      _finishLine.SetActive(true);
      Debug.Log("LINHA DE CHEGADA DESBLOQUEADA");

      if (_scoreTxt != null)
      {
        _scoreTxt.text = "Vá para linha de chegada";
        _scoreTxt.color = Color.green;
      }
    }
  }

  public void LevelComplete()
  {
    Debug.Log("PARABÉNS, VOCÊ COMPLETOUrA FASE");
    if (_victoryPanel != null)
    {
      _victoryPanel.SetActive(true);
      Time.timeScale = 0f;

      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }

  private void UpdateScoreUI()
  {
    if (_scoreTxt != null)
    {
      _scoreTxt.text = $"Pontuação: {_collectedCount} / {_totalItemsInLevel}";
    }
  }

}
