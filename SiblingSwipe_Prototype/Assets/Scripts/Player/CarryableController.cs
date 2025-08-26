using UnityEngine;


public class CarryableController : MonoBehaviour
{  
    // Serialized variables
    [Header("Pickup paramaters")]
    [SerializeField] private LayerMask _pickupLayer;
    [SerializeField] private float _pickupRange = 5.0f;
    [SerializeField] private float _throwForce = 5.0f;

    [Header("References")]
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _hand;

    // Private variables
    private CarriedObject _carriedObject;
    private struct CarriedObject
    {
        public Rigidbody rigidbody;
        public Collider collider;

        // Checks if the objects rigidbody isn't null
        public bool HasObject => rigidbody != null;
    }




    void Update()
    {
        HandleCarryinput();
        HandleThrowInput();
        UpdateCarriedPosition();
    }


    private void HandleCarryinput()
    {
        if(_playerInputManager.IscarryTriggered)
        {
            if (TryGetPickupTarget(out RaycastHit hitInfo))
            {
                // If carrying something, drop it
                if (_carriedObject.HasObject)
                {
                    ReleaseObject();
                }

                PickUpObject(hitInfo);
            }
            else if (_carriedObject.HasObject)
            {
                ReleaseObject();
            }
        }
    }

    // TODO: Use new input system
    private void HandleThrowInput()
    {
        if (_playerInputManager.IsThrowTriggered && _carriedObject.HasObject)
        {
            ThrowObject();
        }
    }

    private void UpdateCarriedPosition()
    {
        if (_carriedObject.HasObject)
        {
            _carriedObject.rigidbody.position = _hand.position;
            _carriedObject.rigidbody.rotation = _hand.rotation;
        }
    }



    // Helper functions
    private bool TryGetPickupTarget(out RaycastHit hitInfo)
    {
        Ray pickupRay = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        return Physics.Raycast(pickupRay, out hitInfo, _pickupRange, _pickupLayer);
    }


    private void PickUpObject(RaycastHit hitInfo)
    {
        _carriedObject = new CarriedObject
        {
            rigidbody = hitInfo.rigidbody,
            collider = hitInfo.collider
        };

        _carriedObject.rigidbody.isKinematic = true;
        _carriedObject.collider.enabled = false;
    }

    private void ReleaseObject()
    {
        if(!_carriedObject.HasObject)
        {
            return;
        }

        _carriedObject.rigidbody.isKinematic = false;
        _carriedObject.collider.enabled = true;

         _carriedObject = default;
    }

    private void ThrowObject()
    {
        _carriedObject.rigidbody.isKinematic = false;
        _carriedObject.collider.enabled = true;

        _carriedObject.rigidbody.AddForce(_mainCamera.transform.forward * 5.0f, ForceMode.Impulse);

        _carriedObject = default;
    }

}
