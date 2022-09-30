using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player _player;

    private PlayerInventory _inventory;
    private PlayerMovement _movement;
    private PlayerAnimator _animation;

    public PlayerInventory Inventory => _inventory;
    public PlayerMovement Movement => _movement;
    public PlayerAnimator PlayerAnimations => _animation;

    private void Awake()
    {
        _player = this;
        _inventory = GetComponent<PlayerInventory>();
        _movement = GetComponent<PlayerMovement>();
        _animation = GetComponent<PlayerAnimator>();
    }

}
