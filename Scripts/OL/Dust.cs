using UnityEngine;

public class Dust : MonoBehaviour
{
    [SerializeField] Transform playerViewDust;
    [SerializeField] float speed = 15;
    float dis;
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,playerViewDust.position,speed*Time.deltaTime);
        dis = Vector3.Distance(transform.position,playerViewDust.position);
        if(dis > 150)
        {
            speed = 100;
        }
        else
        {
            speed = 15;
        }
    }
}
