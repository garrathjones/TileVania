using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float kickMinX = -25f;
    [SerializeField] float kickMaxX = 25f;
    [SerializeField] float kickY = 25f;
    [SerializeField] AudioClip dieSFX;
    [SerializeField] AudioClip jumpSFX;

    bool isAlive = true;
    float gravityScaleAtStart;

    Rigidbody2D myRigidBody;   
    Animator myAnimator; 
    CapsuleCollider2D myBodyCollieder2D;
    BoxCollider2D myFeet;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();   
        myAnimator = GetComponent<Animator>();
        myBodyCollieder2D = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;        
    }


    void Update()
    {
        if (!isAlive) { return; }         
        Run();
        ClimbLadder();
        Jump();
        FlipSprite();
        Die();
    }

    private void Run()
    {          
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is betwen -1 to 1 
        Vector2 playerVeloctiy = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVeloctiy;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);         
    }


    private void ClimbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

        myRigidBody.gravityScale = 0f;
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);      
    }


    private void Jump()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }

    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;   // Mathf.Epsilon is better than using zero
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1);
        }
    }

    private void Die()
    {
        if (myBodyCollieder2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Die");
            AudioSource.PlayClipAtPoint(dieSFX, Camera.main.transform.position);
            GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(kickMinX, kickMaxX), kickY);

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }      
}


