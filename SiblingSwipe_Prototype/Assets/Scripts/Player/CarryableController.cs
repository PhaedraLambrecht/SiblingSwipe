using UnityEngine;


// TODO: Clean this up and make it work with the new input system.
public class CarryableController : MonoBehaviour
{  
    // Serialized variables
    [Header("pick up paramaters")]
    [SerializeField] private LayerMask _pickupLayer;
    [SerializeField] private float _pickupRange = 5.0f;

    [Header("References")]
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _hand;

    // Private variables
    private Rigidbody _currentObjectRigidbody;
    private Collider _currentObjectColider;




    void Update()
    {
       //  HandleCarrying();




        if (_playerInputManager.IscarryTriggered)
        {
            Ray pickupRay = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

            if (Physics.Raycast(pickupRay, out RaycastHit hitInfo, _pickupRange, _pickupLayer))
            {
                if (_currentObjectRigidbody)
                {
                    // old object
                    _currentObjectRigidbody.isKinematic = false;
                    _currentObjectColider.enabled = true;


                    PickUpObject(hitInfo);
                }
                else
                {
                    PickUpObject(hitInfo);
                }

                return;
            }

            // Just drops it to the ground
            if (_currentObjectRigidbody)
            {
                // old object
                _currentObjectRigidbody.isKinematic = false;
                _currentObjectColider.enabled = true;


                // New object.
                _currentObjectRigidbody = null;
                _currentObjectColider = null;

            }
        }




        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_currentObjectRigidbody)
            {
                // old object
                _currentObjectRigidbody.isKinematic = false;
                _currentObjectColider.enabled = true;

                _currentObjectRigidbody.AddForce(_mainCamera.transform.forward * 5.0f, ForceMode.Impulse);


                // New object.
                _currentObjectRigidbody = null;
                _currentObjectColider = null;

            }
        }


        if(_currentObjectRigidbody)
        {
            _currentObjectRigidbody.position = _hand.position;
            _currentObjectRigidbody.rotation = _hand.rotation;
        }

       
    }




    private void HandleCarrying()
    {
    }



    private void PickUpObject(RaycastHit hitInfo)
    {
        _currentObjectRigidbody = hitInfo.rigidbody;
        _currentObjectColider = hitInfo.collider;

        _currentObjectRigidbody.isKinematic = true;
        _currentObjectColider.enabled = false;
    }



}
