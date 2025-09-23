using UnityEngine;

public class Lightnings : MonoBehaviour
{
    float timeSpent;
    [SerializeField] GameObject particles;
    void Start()
    {
        
    }

    
    void Update()
    {
        timeSpent += Time.deltaTime;
        if(timeSpent >= 1 && !particles.activeInHierarchy)
        {
            particles.SetActive(true);
            timeSpent = 0;
        }
        if(timeSpent >= 1 && particles.activeInHierarchy)
        {
            particles.SetActive(false);
            timeSpent = 0;
        }
    }
}
