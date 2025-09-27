using UnityEngine;

public class GripEndChecker : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject playerObject;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FinishGrip"))
        {
            player.isGrip = false;
            playerObject.transform.position = new Vector3(other.transform.position.x,other.transform.position.y+10,other.transform.position.z);
            player.rb.useGravity = true;
        }
    }
}
