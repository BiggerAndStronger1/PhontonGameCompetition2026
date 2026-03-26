using UnityEditor.SpeedTree.Importer;
using UnityEngine;

public class MainMenuText : MonoBehaviour
{
    private float initialRotationZ;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {

       if((transform.rotation.eulerAngles.magnitude - 0) > 0.1f) return;
       print("main menu current button clicked");
    }
}
