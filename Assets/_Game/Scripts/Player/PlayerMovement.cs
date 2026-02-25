using UnityEngine;
using Core.Input;

namespace Mechanics.Movement
{
    // Bu scriptin çalışması için CharacterController şarttır.
    // Unity'nin bunu otomatik eklemesini veya uyarmasını sağlıyoruz.
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Hareket Ayarları")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSmoothTime = 0.1f; // Dönüş Yumuşatma Süresi
        [SerializeField] private float _gravity = -9.81f;

        [Header("Referanslar")]
        [SerializeField] private InputManager _inputManager;
        private CharacterController _controller;
        private Transform _mainCameraTransform; // Kameranın Yönünü Almak İçin
        private Vector3 _velocity;
        private float _targetRotation = 0f;
        private float _rotationVelocity;
        [Header("Animasyon")]
        [SerializeField] private Animator _animator; // Modelin Üzerindeki Animator

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            // Ana Kameranın Transform Referansını Alıyoruz
            if(Camera.main != null)
            {
                _mainCameraTransform = Camera.main.transform;
            }
            if(_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
            if(_animator == null)
            {
                Debug.LogError($"{gameObject.name} üzerinde bir Animator bulunamadı! Lütfen modelin üzerinde Animator olduğundan ve script'e atandığından emin ol.");
            }
        }
        void Update()
        {
            HandleGravity();
            Move();
            UpdateAnimation();
        }
        private void Move()
        {
            Vector2 input = _inputManager.MoveInput;
            // Eğer Girdi Yoksa İşlem Yapma (Dönüşü Korumak İçin)
            if(input == Vector2.zero) return;
            // 1.Kameranın Açısına Göre Hareket Yönünü Hesapla
            // Input Açısını Al Ve Kameranın O Anki Y Rotasyonuyla Topla
            float targetAngle = Mathf.Atan2(input.x , input.y) * Mathf.Rad2Deg + _mainCameraTransform.eulerAngles.y;
            // 2.Karakterin O Yöne Yumuşakça Dönmesini Sağla
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y , targetAngle , ref _rotationVelocity , _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f , angle , 0f);
            // 3 Hesaplanan Açıya Göre Hareket Yönünü Vectör3'e Çevir
            Vector3 moveDirection = Quaternion.Euler(0f , targetAngle , 0f) * Vector3.forward;
            // 4.Karakteri Hareket Ettir
            _controller.Move(moveDirection.normalized * _moveSpeed * Time.deltaTime);
        }
        private void HandleGravity()
        {
            // Karakter yerde mi?
            if (_controller.isGrounded && _velocity.y < 0)
            {
                // Yere değdiğinde küçük bir kuvvetle aşağı itmeye devam et ki havada asılı kalmasın
                _velocity.y = -2f; 
            }
            // Yerçekimi her karede eklenir
            _velocity.y += _gravity * Time.deltaTime;
            // Karakteri aşağı doğru hareket ettir
            _controller.Move(_velocity * Time.deltaTime);
        }
        private void UpdateAnimation()
        {
            if (_animator == null) return;
            float currentSpeed = _inputManager.MoveInput.magnitude;
            // 0.1f değeri geçiş yumuşaklığıdır
            _animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);
        }
    }
}