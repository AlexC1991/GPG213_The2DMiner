using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace AlexzanderCowell
{
    public class TwoDCharacterMovement : MonoBehaviour
    {
        [Range(0, 10)]
        [SerializeField] private float moveSpeed;
        [Range(0, 20)]
        [SerializeField] private float runSpeed;
        [Range(0, 50)]
        [SerializeField] private float jumpHeight;
        [Range(-50, 50)]
        [SerializeField] private float gravity;
        private float horizontalMovement;
        private float verticalMovement;
        public static bool runningNow;
        public static bool jumpingNow;
        private float moveSpeedOriginal;
        private Rigidbody2D rb2D;
        private float jumpCount;
        [SerializeField] private LayerMask groundLayer;
        public static bool walkingNow;
        public static bool isGrounded;
        public SpriteRenderer[] childSpriteRenderers;
        public GameObject[] childrenObjects;
        [SerializeField] private float groundDetectionRadius = 5f;
        [SerializeField] private GameObject crosshairsZDirection;
        private Animator miningMovement;
        [SerializeField] private GameObject rotatingBackground;
        private Vector2 startPosition;
        
        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            childSpriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            childrenObjects = GetAllChildren(gameObject.transform);
            miningMovement = childrenObjects[0].GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            startPosition = rotatingBackground.transform.position;
            moveSpeedOriginal = moveSpeed;
            walkingNow = true;
        }
        
        private void Update()
        {
            rb2D.gravityScale = gravity;
            horizontalMovement = Input.GetAxis("Horizontal");
            verticalMovement = Input.GetAxis("Vertical");
            PlayerMovement();
            PlayerJump();
            PlayerMiningAction();
        }

        private void PlayerMovement()
        {
            if (horizontalMovement > 0)
            {
                /*float newPosition = Mathf.Repeat(Time.time * moveSpeed - 1, 50);
                rotatingBackground.transform.position = startPosition + Vector2.right * newPosition;*/
                
                foreach (var g in childSpriteRenderers)
                {
                    g.flipX = true;
                    childSpriteRenderers[3].flipX = false;
                }
                
                transform.Translate(Vector3.right * (horizontalMovement * moveSpeed * Time.deltaTime));
            }
            else if (horizontalMovement < 0)
            {
                /*float newPosition = Mathf.Repeat(Time.time * moveSpeed, 50);
                rotatingBackground.transform.position = startPosition + Vector2.left * newPosition;*/
                
                foreach (var g in childSpriteRenderers)
                {
                    g.flipX = false;
                    childSpriteRenderers[3].flipX = true;
                }
                
                transform.Translate(Vector3.left * (-horizontalMovement * moveSpeed * Time.deltaTime));
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                runningNow = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                runningNow = false;
            }

            if (runningNow)
            {
                walkingNow = false;
                moveSpeed = runSpeed;
            }
            else if (!runningNow)
            {
                walkingNow = true;
                moveSpeed = moveSpeedOriginal;
            }
        }
        
        private void PlayerJump()
        {

            isGrounded = Physics2D.OverlapCircle(transform.position, groundDetectionRadius, groundLayer);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpCount < 1)
            {
                jumpingNow = true;
                jumpCount += 1;
            }
            else if (jumpingNow || jumpCount > 1)
            {
                rb2D.velocity = new Vector2(-rb2D.velocity.x, jumpHeight);
                jumpCount = 0;
                jumpingNow = false;
            }
        }

        private void PlayerMiningAction()
        {
            if (miningMovement != null)
            {
                Debug.Log("Detected Animator");
                
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Debug.Log("Pressing Button");
                    
                    miningMovement.SetBool("MiningStart", true);
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    Debug.Log("Stopping Animation");
                    miningMovement.SetBool("MiningStart", false);
                }
            }
            else
            {
                Debug.Log("Didn't Find Animator");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, groundDetectionRadius);
        }
        
        private GameObject[] GetAllChildren(Transform parent)
        {
            int childCount = parent.childCount;
            GameObject[] children = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.GetChild(i);
                children[i] = child.gameObject;
            }

            return children;
        }

    }
}
