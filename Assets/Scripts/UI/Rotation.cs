using UnityEngine;

namespace UI
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;

        private void LateUpdate()
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}