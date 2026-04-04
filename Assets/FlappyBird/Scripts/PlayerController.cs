using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Floating")] [SerializeField] private float floatAmplitude = .1f;
    [SerializeField] private float floatSpeed = 4f;

    [Header("Jumping")] [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 20f;

    private Rigidbody2D _rigidbody2D;
    private Vector3 _startPosition;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _startPosition = Vector3.zero;
        _rigidbody2D.simulated = true;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.StartScreen ||
            GameManager.Instance.GameState == GameState.GameReady)
            FloatIdle();
    }

    private void FloatIdle()
    {
        var newY = _startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
    }

    private void RotateBird()
    {
        var angle = Mathf.Clamp(_rigidbody2D.linearVelocityY * 5f, -90f, 30f);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            Time.deltaTime * rotationSpeed
        );
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.GameState == GameState.StartScreen ||
            GameManager.Instance.GameState == GameState.GameOver) return;
        if (GameManager.Instance.GameState == GameState.GameReady)
        {
            GameManager.Instance.GamePlay();
            _rigidbody2D.simulated = true;
        }
        _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, 0f);
        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(GameManager.Instance.GameState == GameState.GameOver) return;
        if (!collision.collider.CompareTag("Obstacle")) return;
        GameManager.Instance.GameOver();
    }
    
    public void ResetPlayer()
    {
        _rigidbody2D.simulated = false;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _startPosition = new Vector3(-.5f, 0f, 0f);
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
    }
}