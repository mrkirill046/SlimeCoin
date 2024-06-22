using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Rotation : MonoBehaviour
    {
        public float rotationSpeed;

        private void LateUpdate()
        {
            if (GetComponent<Image>())
            {
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
        }
    }
}