using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Referenties naar de knoppen in de UI
    public Button startButton;   // Button om de vliegtuigen te laten bewegen
    public Button parkeerButton; // Button om de vliegtuigen te parkeren
    public Button lightOnButton; // Button om de lichten aan te zetten
    public Button lightOffButton;// Button om de lichten uit te zetten

    // Referenties naar alle vliegtuigen
    public PlaneManager[] planes;  // Array van PlaneManager scripts (zet de PlaneManager van elk vliegtuig hier)

    void Start()
    {
        // Voeg listeners toe voor de knoppen
        startButton.onClick.AddListener(StartMovingPlanes);
        parkeerButton.onClick.AddListener(ParkeerPlanes);
        lightOnButton.onClick.AddListener(TurnLightsOn);
        lightOffButton.onClick.AddListener(TurnLightsOff);
    }

    // Start de beweging van de vliegtuigen
    public void StartMovingPlanes()
    {
        foreach (PlaneManager plane in planes)
        {
            plane.StartMoving();  // Zet elke plane aan om te bewegen
        }
    }

    // Parkeer de vliegtuigen
    public void ParkeerPlanes()
    {
        foreach (PlaneManager plane in planes)
        {
            if (plane.HasReachedStopDistance() && !plane.isParked)  // Ensure the plane is ready to park
            {
                plane.ParkPlane();  // Parkeer de plane als deze de stopafstand heeft bereikt
            }
            else
            {
                Debug.Log("Plane hasn't reached the stop distance yet, or it's already parked.");
            }
        }
    }

    // Zet de lichten aan voor alle vliegtuigen
    public void TurnLightsOn()
    {
        foreach (PlaneManager plane in planes)
        {
            plane.TurnLightOn();  // Zet de lichten aan voor elk vliegtuig
        }
    }

    // Zet de lichten uit voor alle vliegtuigen
    public void TurnLightsOff()
    {
        foreach (PlaneManager plane in planes)
        {
            plane.TurnLightOff();  // Zet de lichten uit voor elk vliegtuig
        }
    }
}
