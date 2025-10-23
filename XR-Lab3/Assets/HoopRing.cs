using UnityEngine;

public class HoopRing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Basketball"))
        {
            BasketballManager.Instance.OnBasketballEnteredHoop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Basketball"))
        {
            BasketballManager.Instance.OnBasketballExitedHoop();
        }
    }
}
