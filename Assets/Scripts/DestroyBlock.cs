using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("PickAxe"))
      {
         Destroy(this.gameObject,0.2f);
      }
      else
      {
         
      }
   }
}
