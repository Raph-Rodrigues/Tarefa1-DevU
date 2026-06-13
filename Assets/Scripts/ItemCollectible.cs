using System;
using UnityEngine;

public class ItemCollectible : MonoBehaviour
{
  [Header("Efeitos Visuais")]
  [SerializeField] private float _rotationSpeed = 100f;
  [SerializeField] private float _floatAmplitude = 0.5f;
  [SerializeField] private float _floatFrequency = 2f;
  [SerializeField] private GameObject _particlePrefab;

  private Vector3 _startPos;

  private void Start()
  {
    _startPos = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    // efeito de rotação
    transform.Rotate(Vector3.up * _rotationSpeed * Time.fixedDeltaTime);

    // efeito de flutuação
    float newY = _startPos.y + Mathf.Sin(Time.time * _floatFrequency) * _floatAmplitude;
    transform.position = new Vector3(transform.position.x, newY, transform.position.z);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      GameManager.Instance.AddCollectible();
      if (_particlePrefab != null)
      {
        Instantiate(_particlePrefab, transform.position, Quaternion.identity);
      }
      Destroy(gameObject);
    }
  }
}
