using UnityEngine;

public class ManageShop : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private bool _isOpen;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    public void CLickShopButton()
    {
        _isOpen = !_isOpen;

        switch (_isOpen)
        {
            case true:
                animator.SetBool(IsOpen, true);
                break;
            case false:
                animator.SetBool(IsOpen, false);
                break;
        }
    }
}
