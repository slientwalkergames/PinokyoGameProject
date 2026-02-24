using UnityEngine;
using Core.Input;

namespace Mechanics.Movement
{
    // Bu scriptin çalışması için CharacterController şarttır.
    // Unity'nin bunu otomatik eklemesini veya uyarmasını sağlıyoruz.
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Ayarlar")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _gravity = -9.81f;

        [Header("Referanslar")]
        // InputManager'ı Editörden Sürükleyip Bırakmak İçin
        [SerializeField] private InputManager _inputManager;
        private CharacterController _controller;
        private Vector3 _velocity; // Yerçekimi Hızı İçin
        private bool _isGround; // Yerdemiyiz

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }
        void Update()
        {
            HandleGravity(); // Yerçekimi Kontrölü
            Move(); // Hareket İşlemi
        }
        public void Move()
        {
            // InputManager'dan veriyi al (x ve y)
            Vector2 input = _inputManager.MoveInput;
            // 2D girdiyi 3D dünyaya uyarla (Y ekseni yukarıdır, Z ekseni ileridir)
            // Şimdilik kamera açısını hesaba katmıyoruz, dünya koordinatlarına göre hareket edecek.
            Vector3 moveDirection = new Vector3(input.x , 0f , input.y);
            // Karakteri Hareket Ettir
            // Time.deltatime : Bilgisayar Hızından Bağımsız Akıcı Hareket Sağlar
            _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);
        }
        private void HandleGravity()
        {
            // Karakt Yerde Mi Kontrölü (CharacterController'ın kendi özelliği)
            _isGround = _controller.isGrounded;
            if(_isGround && _velocity.y < 0)
            {
                // Yerdeysek düşme hızını sıfırla (hafif eksi veriyoruz ki yere tam bassın)
                _velocity.y = -2f;
            }
            // Yerçekimi uygula
            _velocity.y += _gravity * Time.deltaTime;
            // Yerçekimi Hareketini Uygula
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}