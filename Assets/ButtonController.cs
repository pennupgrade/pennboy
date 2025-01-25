using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject[] gameButtons;

    public void disableGameButtons(string callerID) {
        foreach (GameObject button in gameButtons ) {
            OpenGame cmp = button.GetComponent<OpenGame>();

            if (cmp.gameID == callerID) {
                continue;
            }

            button.SetActive(false);
        }
    }
}
