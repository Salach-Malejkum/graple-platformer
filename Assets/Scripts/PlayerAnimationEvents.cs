using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private MainCharacter mainCharacter;

    public void Step()
    {
        mainCharacter.Step();
    }
}
