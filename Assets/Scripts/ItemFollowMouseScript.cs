using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class ItemFollowMouseScript : MonoBehaviour
{
    private GameObject gameObjectToFollow;
    private GameObject pickAxeObject;
    private float yMouseMovement;
    private float xMouseMovement;
    private Vector3 originalLocalScale;


    private void Start()
    {
        gameObjectToFollow = GameObject.FindWithTag("ArmJoint");
        pickAxeObject = GameObject.FindWithTag("PickAxe");
        StartCoroutine(FollowTheMouseFunction());
        originalLocalScale = gameObjectToFollow.transform.localScale;
    }

    IEnumerator FollowTheMouseFunction()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                mouseWorldPosition.z = gameObjectToFollow.transform.position.z;
                Vector3 direction = mouseWorldPosition - gameObjectToFollow.transform.position;
                
                float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                
                gameObjectToFollow.transform.rotation = Quaternion.Euler(0, 0, angle);
 
                Debug.Log("Mouse X Position: " + xMouseMovement);
                Debug.Log("Mouse Y Position: " + yMouseMovement);
            
                if (gameObjectToFollow.transform.rotation.z < 0)
                {
                    pickAxeObject.GetComponent<SpriteRenderer>().flipY = false;
                }
                else
                {
                    pickAxeObject.GetComponent<SpriteRenderer>().flipY = true;
                }

            }
            
            yield return null;
        }
    }
}
