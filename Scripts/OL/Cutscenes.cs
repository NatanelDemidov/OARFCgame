using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    [SerializeField] GameObject plr;
    [SerializeField] GameObject plrCar;
    [SerializeField] GameObject cutScene1Objects;
    int numOfCutscene = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishCutScene"))
        {
            gameObject.SetActive(false);
            plr.SetActive(true);
            plrCar.SetActive(true);
            cutScene1Objects.SetActive(false);
            numOfCutscene += 1;
        }
    }
}
