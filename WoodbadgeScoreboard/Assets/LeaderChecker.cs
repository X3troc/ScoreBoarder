using UnityEngine;

public class LeaderChecker : MonoBehaviour
{
    public GameObject[] playerGameObjects; // Array to track player game objects

    // Update is called once per frame
    void Update()
    {
        GameObject leader = null;
        float highestY = float.MinValue;
        bool isTie = false;

        foreach (GameObject player in playerGameObjects)
        {
            if (player.transform.position.y > highestY)
            {
                highestY = player.transform.position.y;
                leader = player;
                isTie = false;
            }
            else if (player.transform.position.y == highestY)
            {
                isTie = true;
            }
        }

        if (isTie)
        {
            leader = null; // No leader if there's a tie
        }
        
        // Handle the leader (or lack thereof) as needed
        foreach (GameObject player in playerGameObjects)
        {
            PlayerMarker playerMarker = player.GetComponent<PlayerMarker>();
            if (playerMarker != null)
            {
                playerMarker.SetIsWinning(player == leader);
            }
        }

        // Handle the leader (or lack thereof) as needed
        if (leader != null)
        {
            Debug.Log("Leader: " + leader.name);
        }
        else
        {
            Debug.Log("No leader due to a tie.");
        }
    }
}