using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
  [Header("Configurações de Movimento")]
  [SerializeField] private float _speed = 3f;
  [SerializeField] private Transform[] _waypoints;

  [Header("Configurações de Dano")]
  [SerializeField] private float _knockBackForce = 10f;

  private int _currentWaypointIndex = 0;
  private float _minDistanceToWaypoint = 0.2f;

  // Update is called once per frame
  void Update()
  {
    if (_waypoints == null || _waypoints.Length == 0) return;

    // move em direção do waypoint atual
    Transform targetWaypoint = _waypoints[_currentWaypointIndex];
    transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, _speed * Time.fixedDeltaTime);

    // Se chegou muito perto do waypoin muda para direção do próximo
    if (Vector3.Distance(transform.position, targetWaypoint.position) < _minDistanceToWaypoint)
    {
      _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      // pega os dados físicos do primeiro contato onde as malhas se encontram
      ContactPoint contact = collision.GetContact(0);

      // a normal.y próxima de 1 significa que a collisão veio de cima (jogador pisou)
      // a normal.y próxima de -1 significa que a collisão veio de baixo
      // para ser uma collisão lateral (paredes do bloco), a normal.y deve estar próxima de 0
      if (Mathf.Abs(contact.normal.y) < 0.5f)
      {
        Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
          Vector3 pushDirection = contact.normal; // empurra o jogador no ponto onde o impacto ocorreu
          pushDirection.y = 1.5f; // joga o player levemente para o alto
          playerRb.AddForce(pushDirection * _knockBackForce, ForceMode.Impulse);
        }
      }
    }
  }
}
