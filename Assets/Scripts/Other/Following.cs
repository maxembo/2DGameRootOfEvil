using UnityEngine;

namespace Scripts.Other
{
    public class Following : MonoBehaviour
    {
        [SerializeField] private Transform _trackingObject;
        [SerializeField] private float _offset;
        [SerializeField] private float _speed;

        private bool _isMove;

        private void FixedUpdate()
        {
            Checking();
            Moving();
        }

        private void Checking()
        {
            if (transform.position.x - _trackingObject.position.x > _offset ||
                _trackingObject.position.x - transform.position.x > _offset ||
                transform.position.y - _trackingObject.position.y > _offset ||
                _trackingObject.position.y - transform.position.y > _offset)
                _isMove = true;
        }

        private void Moving()
        {
            if (transform.position != _trackingObject.position && _isMove)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                        _trackingObject.position, _speed * Time.deltaTime);

                return;
            }

            _isMove = false;
        }
    }
}