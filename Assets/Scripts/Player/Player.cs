using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    
    public static Player Instance { get; private set; }
    
    public event EventHandler<OnSelectedCounterChangedEventArgs > OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private Transform kitchenObjectHoldPoint;

    [SerializeField] private float movespeed = 7f;

    [SerializeField] private GameInput gameInput;

    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    //private bool isMoving;
    //private Vector3 lastPosition;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 player instance");
        }

        Instance = this;
    }

   

    private void Start()
    {
        //lastPosition = transform.position;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction; //have to tab this, cant type it out, weird
            
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null) 
        {
            selectedCounter.Interact(this);
        }
    }

    
    
    
    // Update is called once per frame
    private void Update()
    {

        HandleMovement();
        HandleInteractions();

    }

    
    
    
    
    public bool IsWalking()
    {
        return isWalking;
    }

   
    
    
    
    
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        //idk why we copied these. 2:21:33


        if (moveDir != Vector3.zero )
        {
            lastInteractDir = moveDir;
        }



        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter
                ))
                //TrygetComponent automatically does the null check for you. is the value null? idk, let's try it
            {
                //has ClearCounter
                //clearCounter.Interact();

                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);


                   
                }
            }
            else
            {
                SetSelectedCounter(null);


            }
                
        }
        else
        {
            SetSelectedCounter(null);
        }

        //Debug.Log(selectedCounter);

    }





    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = GetCurrentMoveSpeed() * Time.deltaTime;
        float playerRadius = 1f;
        float playerHeight = 4f;

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance,
            countersLayerMask,
            QueryTriggerInteraction.Ignore // Ignore trigger colliders during the cast
        );

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirX,
                moveDistance,
                countersLayerMask,
                QueryTriggerInteraction.Ignore
            );

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirZ,
                    moveDistance,
                    countersLayerMask,
                    QueryTriggerInteraction.Ignore
                );

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        //Check if the player is moving for active the sound effect
        //Vector3 currentPosition = transform.position;
        //isMoving = (currentPosition != lastPosition);
        //if (isMoving)
        //{
        //   Debug.Log("Player is moving!");
        //    AudioManager.Instance.PlaySFX("Walking");
        //}
    }

    // New method to get the current move speed based on sprinting
    private float GetCurrentMoveSpeed()
    {
        // Check if the left shift key is held down
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            return movespeed * 2; // Double the move speed for sprinting
        }
        return movespeed; // Return normal move speed
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }




    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }


    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }


}
