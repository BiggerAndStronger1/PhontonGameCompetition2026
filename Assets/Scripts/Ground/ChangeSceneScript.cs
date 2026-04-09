using UnityEngine;

public class ChangeSceneScript : MonoBehaviour
{
    public Player player;

    private bool playerInside = false;
    private bool canSwitchScene = false;
    private float timer;
    public float triggerTime = 0.2f;

    private Vector3 cameraDeltaX = new Vector3(18f, 0);
    private Vector3 cameraDeltaY = new Vector3(0, 10f);
    private Vector3 playerDeltaX = new Vector3(2f, 0);
    private Vector3 playerDeltaY = new Vector3(0, 2.5f);

    private void Start()
    {
        //Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    private void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;
            if (timer >= triggerTime) 
                canSwitchScene = true;
            if (canSwitchScene) 
                SwitchScene();
        }
    }

    private void SwitchScene()
    {
        if (!canSwitchScene) return;

        canSwitchScene = false;
        timer = 0f;
        

        if (player.transform.position.y > Camera.main.transform.position.y + 3.8)
        {
            Camera.main.transform.position += cameraDeltaY;
            player.transform.position += playerDeltaY;
        }
        else if (player.transform.position.y < Camera.main.transform.position.y - 3.5)
        {
            Camera.main.transform.position -= cameraDeltaY;
            player.transform.position -= playerDeltaY;
        }    
        else if (player.transform.position.x > Camera.main.transform.position.x + 8)
        {
            Camera.main.transform.position += cameraDeltaX;
            player.transform.position += playerDeltaX;
        }
        else if (player.transform.position.x < Camera.main.transform.position.x - 8)
        {
            Camera.main.transform.position -= cameraDeltaX;
            player.transform.position -= playerDeltaX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
