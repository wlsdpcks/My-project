using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider; 
    private InputAction _thumbstick;
    private bool _isActive;
    
    void Start()
    {
        rayInteractor.enabled= false; // In the start method, we're actually just going to start out by disabling the array iteractor.
        // because the idea is the only show the line when you're pushing the stick forward,
        // because when you release, it's going to automatically teleport you, and then we push the stick forward,
        // it starts the teleportation and then you should be able to cancel it by pressing the grip button.


        // Grab the activate action
        var activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;
        
        // cancel action in the thumbstick move action
        var cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
        cancel.Enable();
        cancel.performed += OnTeleportCancel; 
        // any time this action is performed, the this method is going to run and it's much more proficient than doing this method inside of the update call 
        // because it's not running every sigle frame, it's just only when this action is performed.

        // move direction
        _thumbstick = actionAsset.FindActionMap("XRI LeftHand").FindAction("Move");
        _thumbstick.Enable();
    }

    
    void Update() // In the update method, the thumbstick has been pushed forward, the teleportation has been activated, and then it's also been released, the trigger is back to zero. So thumbstick dot triggered is going to be false.
        // So that means that the user now wants to teleport, and so we're going to have to get a raycast and try to get a hit off of it.
    {
        if (!_isActive) 
            return;// If the actual Teleport is not active, we're not going to continue.
                               // this is running every frame. so if is activist fasle, we want the rest of the code is just not even run.
                               // So I'm going to lift is active and then just type return underneath it. So now if it is not active, if the teleportation array is not active, then we're just going to excape and not do anything. if it is active, we can continue


        // If the thumb stick is still pushed forward, then that means that the player does't want to teleport yet. So we're just going to leave still.
        if (_thumbstick.triggered) 
            return;

        if(!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            rayInteractor.enabled = false;
            _isActive = false;
        }

        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point,
            //destinationRotation=  , ?
        };
        provider.QueueTeleportRequest(request);
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        // when on teleport activate is called we want to enable the Raycast.
        rayInteractor.enabled= true;
        // Tricky part of this is finding out where the position of the Raycast is when we release it.
        // The best way I've come up to do this is to have a toggle that says, okay, the teleportation array is active. and then at the ery last moment,
        // when it becomes not active, that's when we want to grab the position. And that's when we want to teleport, and we're going to perform that inside of the update method.
       _isActive= true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive= false;
    }
}
