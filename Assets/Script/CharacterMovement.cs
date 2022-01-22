using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterState {
    GROUNDED,
    JUMPING,
    FALLING,
    SLIDING
}
public enum AnimationState {
    IDLE,
    JUMP,
    FALL,
    LAND,
    SLIDE,
    ATTACK,
    RUN
}
public class CharacterMovement : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    public float speed = 3;
    public float rotationSpeed = 4;
    public float jumpHeight = 200;
    public float gravity = 3.81f;
    public float terminalVelocity = 10f;
    public Vector3 currentMovement = Vector3.zero;
    public Vector3 currentRotation = Vector3.zero;
    public CharacterState characterState;
    public AnimationState animationState;
    public float standingHeight;
    public Vector3 standingCenter;
    public float slidingHeight;
    public Vector3 slidingCenter;
    public float slidingDuration = 3;
    public float slidingTimer;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        standingHeight = controller.height;
        standingCenter = controller.center;
        slidingTimer = slidingDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if(controller){
            handleMovement();
            handleAnimation();
            if(controller.isGrounded && characterState != CharacterState.SLIDING)
                characterState = CharacterState.GROUNDED;
        }
    }

    void handleMovement() {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        // float gravityDelta = gravity * Time.deltaTime;
        // if(state == CharacterState.GROUNDED) {
        //     currentMovement = new Vector3(horizontal, 0 , vertical) * speed;
        //     if(Input.GetAxis("Jump") != 0)
        //     {
        //         currentMovement = new Vector3(currentMovement.x,jumpHeight,currentMovement.z);
        //         state = CharacterState.JUMPING;
        //     }
        // }
        // else {
        //     if(currentMovement.y > -terminalVelocity)
        //         currentMovement = new Vector3 (currentMovement.x,currentMovement.y-gravityDelta,currentMovement.z);
        //     if(currentMovement.y <= -terminalVelocity)
        //         currentMovement = new Vector3 (currentMovement.x,-gravityDelta,currentMovement.z);
        // }
        // controller.Move(currentMovement * (Time.deltaTime*1.5f));
        if(characterState == CharacterState.GROUNDED) {
            if(Input.GetAxis("Slide") != 0) {
                characterState = CharacterState.SLIDING;
                slidingTimer = slidingDuration;
                controller.height = slidingHeight;
                controller.center = slidingCenter;
            }
            currentMovement = new Vector3(hInput, 0 , vInput) * speed;
            currentRotation = transform.up * rotationSpeed * hInput;
            if(Input.GetAxis("Jump") != 0)
            {
                currentMovement.y = jumpHeight;
                characterState = CharacterState.JUMPING;
            }
        }
        if (characterState == CharacterState.SLIDING) {
            slidingTimer -= Time.deltaTime;
            if (slidingTimer <= 0) {
                characterState = CharacterState.FALLING;
                controller.height = standingHeight;
                controller.center = standingCenter;
            }
        }
        currentMovement.y -= gravity * Time.deltaTime;
        if (currentMovement.y <= -terminalVelocity)
            currentMovement.y = -terminalVelocity;
        controller.Move(currentMovement * Time.deltaTime);
        currentRotation = new Vector3(currentMovement.x, 0 , currentMovement.z);
        // transform.Rotate(currentRotation * Time.deltaTime);
        if (currentMovement.y <= 0 && characterState != CharacterState.SLIDING) {
            characterState = CharacterState.FALLING;
        }
    }
    void handleAnimation() {
        if (characterState == CharacterState.SLIDING) {
            animationState = AnimationState.SLIDE;
        }
        else if(currentMovement.y > 0) {
            animationState = AnimationState.JUMP;
        }
        else if(currentMovement.y < -0.75f && characterState != CharacterState.GROUNDED) {
            animationState = AnimationState.FALL;
        }
        else if(animationState == AnimationState.FALL) {
            animationState = AnimationState.LAND;
        }
        else if(currentMovement.x != 0 || currentMovement.z !=0) {
            animationState = AnimationState.RUN;
        }
        else {
            animationState = AnimationState.IDLE;
        }
        //reset animation states
            animator.SetBool("jump", false);
            animator.SetBool("fall", false);
            animator.SetBool("run", false);
            animator.SetBool("idle", false);
            animator.SetBool("land", false);
            animator.SetBool("slide", false);
        switch (animationState) {
            case AnimationState.IDLE:
            animator.SetBool("idle", true);
            break;
            case AnimationState.LAND:
            animator.SetBool("land", true);
            break;
            case AnimationState.JUMP:
            animator.SetBool("jump", true);
            break;
            case AnimationState.FALL:
            animator.SetBool("fall", true);
            break;
            case AnimationState.RUN:
            animator.SetBool("run", true);
            break;
            case AnimationState.SLIDE:
            animator.SetBool("slide", true);
            break;
        }
    }
}