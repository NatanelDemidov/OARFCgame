using UnityEngine;

public class Dust : MonoBehaviour
{
    [SerializeField] Transform playerViewDust;
    [SerializeField] float speed = 0.5f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,playerViewDust.position,speed*Time.deltaTime);
    }
}
