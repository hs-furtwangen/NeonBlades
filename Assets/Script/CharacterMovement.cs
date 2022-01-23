using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
public enum CharacterState {
    GROUNDED,
    JUMPING,
    FALLING,
    SLIDING,
    WALLRUN
}
public enum AttackState {
    RED,
    BLUE,
    NONE
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
    public Animator animator;
    public float speed = 3;
    public float rotationSpeed = 4;
    public float jumpHeight = 200;
    public float gravity = 3.81f;
    public float terminalVelocity = 10f;
    public Vector3 currentMovement = Vector3.zero;
    public Vector3 currentRotation = Vector3.zero;
    public CharacterState characterState;
    public AnimationState animationState;
    public AttackState attackState;
    public float standingHeight;
    public Vector3 standingCenter;
    public float slidingHeight;
    public Vector3 slidingCenter;
    public float slidingDuration = 0.01f;
    public float slidingTimer;
    public float fallingThreshold = 0.6f;
    public bool canSlide = true;
    public bool canWallrun = true;
    public Sword sword1;
    public Sword sword2;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        if(!animator)
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
            handleAttack();
        }
    }

    void handleMovement() {
        //Apply Gravity to check it the character is actually grounded or not
        // currentMovement = new Vector3(0, gravity * Time.deltaTime , 0);
        // controller.Move(currentMovement * Time.deltaTime);
        if(characterState == CharacterState.SLIDING || characterState == CharacterState.WALLRUN)
        {
            Debug.Log("Either sliding or Wallrunning right now");
        }
        if (controller.isGrounded) {
            characterState = CharacterState.GROUNDED;
        }
        else {
            characterState = CharacterState.FALLING;
        }
        //Read Input
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        if(characterState == CharacterState.GROUNDED) {
            // if(Input.GetAxis("Slide") != 0) {
            //     characterState = CharacterState.SLIDING;
            //     slidingTimer = slidingDuration;
            //     controller.height = slidingHeight;
            //     controller.center = slidingCenter;
            // }
            currentMovement = new Vector3(hInput, 0 , vInput) * speed;
            if(Input.GetAxis("Jump") != 0)
            {
                currentMovement.y = jumpHeight;
                characterState = CharacterState.JUMPING;
            }
        }
        // if (characterState == CharacterState.SLIDING) {
        //     slidingTimer -= Time.deltaTime;
        //     currentMovement.y -= (gravity/2) * Time.deltaTime;
        //     if (slidingTimer <= 0) {
        //         characterState = CharacterState.FALLING;
        //         controller.height = standingHeight;
        //         controller.center = standingCenter;
        //     }
        // }
        currentMovement.y -= gravity * Time.deltaTime;
        if (currentMovement.y <= -terminalVelocity)
            currentMovement.y = -terminalVelocity;
        controller.Move(currentMovement * Time.deltaTime);
    }
    void handleAnimation() {
        if (characterState == CharacterState.SLIDING) {
            animationState = AnimationState.SLIDE;
        }
        else if(currentMovement.y > 0) {
            animationState = AnimationState.JUMP;
        }
        else if(currentMovement.y < -fallingThreshold && characterState != CharacterState.GROUNDED) {
            animationState = AnimationState.FALL;
        }
        // else if(animationState == AnimationState.IDLE && characterState != CharacterState.GROUNDED) {
        //     animationState = AnimationState.FALL;
        // }
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
    void handleAttack() {
        if(animator.GetBool("sword_idle") == true) {
            attackState = AttackState.NONE;
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", false);
            sword1.SetActiveState(false);
            sword2.SetActiveState(false);
        }
        if (attackState == AttackState.NONE) {
            if(Input.GetAxis("Fire1") != 0) {
                animator.SetBool("attack1", true);
                animator.SetBool("sword_idle", false);
                attackState = AttackState.RED;
                sword1.SetActiveState(true);
            }
            else if(Input.GetAxis("Fire2") != 0) {
                animator.SetBool("attack2", true);
                animator.SetBool("sword_idle", false);
                attackState = AttackState.BLUE;
                sword2.SetActiveState(true);
            }
        }
    }
}