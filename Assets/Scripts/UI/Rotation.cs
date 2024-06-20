using UnityEngine;

namespace UI
{
    public class Rotation : MonoBehaviour
    {
        public float rotationSpeed;

        private void LateUpdate()
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}