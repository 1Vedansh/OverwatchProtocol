using UnityEngine;

public class DestinationButton : MonoBehaviour
{
    public GameObject currentMenu;
    public GameObject destinationMenu;

    public void goToDestination()
    {
        currentMenu.SetActive(false);
        destinationMenu.SetActive(true);
    }
}
