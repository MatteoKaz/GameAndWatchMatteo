using UnityEngine;

public class OpenMenuAmelio : MonoBehaviour
{
    [SerializeField] GameObject MenuParent;
    [SerializeField] GameObject Black;

    public void OpenMenu()
    {
        MenuParent.SetActive(true);
        Black.SetActive(true);
    }

    public void CloseMenu()
    {
        MenuParent.SetActive(false);
        Black .SetActive(false);
    }
}
