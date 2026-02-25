using UnityEngine;

namespace Core.Input
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        public Vector2 MoveInput {get; private set;}
        void Awake()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Player.Enable();
        }
        void Update()
        {
            MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
            if(MoveInput != Vector2.zero)
            {
                Debug.Log($"Girdi (Core.Input) : {MoveInput}");
            }
        }
        void OnDisable()
        {
            _inputActions.Player.Disable();
        }
    }
}
