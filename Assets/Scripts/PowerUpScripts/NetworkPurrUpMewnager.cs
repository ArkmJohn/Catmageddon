using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPurrUpMewnager : Photon.MonoBehaviour, IPunObservable
{
    public NetworkPurrUpMewnager Instance;
    public GameObject RatGroupAI;
    public List<GameObject> SpawnLocations;
    public List<GameObject> PowerUpsPrefab;
    public List<GameObject> RedAISpawnLocation;
    public List<GameObject> BlueAISpawnLocation;

    public bool Spawning = false;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
        
        if (this.Spawning == false)
        {
            StartCoroutine(SpawnUp());
        }
		
	}

    public IEnumerator SpawnUp()
    {
        this.Spawning = true;
        Debug.Log("Starting Spawn");
        yield return new WaitForSeconds(5);
        Debug.Log("Spawning now " + SpawnLocations.Count);

        for (int i = 0; i < SpawnLocations.Count;i++)
        {
            if (SpawnLocations[i].GetComponent<PowerUpSpawnLocation>().MySpawnedPowerUp == null)
            {
                Debug.Log("Spawning");
                int random = Random.Range(0, PowerUpsPrefab.Count);
                photonView.RPC("SpawnPowerUp", PhotonTargets.All, i, random);
            }
        }
        this.Spawning = false;
    }

    // Spawns a powerup Based on a spawn Index
    [PunRPC]
    public void SpawnPowerUp(int spawnIndex, int powerUpIndex)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        GameObject powerUp = PhotonNetwork.Instantiate(PowerUpsPrefab[powerUpIndex].name, SpawnLocations[spawnIndex].transform.position, Quaternion.identity, 0);
        SpawnLocations[spawnIndex].GetComponent<PowerUpSpawnLocation>().SpawnAPowerUp(powerUp); // Updates the location
    }

    [PunRPC]
    public void SpawnAGroup(string Group)
    {
        switch (Group)
        {
            case "Red":
                for (int i = 0; i <= RedAISpawnLocation.Count; i++)
                {
                    GameObject RedRat = PhotonNetwork.Instantiate(RatGroupAI.name, RedAISpawnLocation[i].transform.position, RedAISpawnLocation[i].transform.rotation, 0);
                    RedRat.GetComponent<Steering>().MyTeam = PunTeams.Team.red;
                }
                break;

            case "Blue":
                for (int i = 0; i <= BlueAISpawnLocation.Count; i++)
                {
                    GameObject BlueRat = PhotonNetwork.Instantiate(RatGroupAI.name, BlueAISpawnLocation[i].transform.position, BlueAISpawnLocation[i].transform.rotation, 0);
                    BlueRat.GetComponent<Steering>().MyTeam = PunTeams.Team.blue;
                }
                break;

            default:

                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
        }
        else
        {

        }
    }
}
