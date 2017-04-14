using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPurrUpMewnager : Photon.MonoBehaviour, IPunObservable
{
    public NetworkPurrUpMewnager Instance;
    public bool isSpawningAI = false;
    public GameObject ratGroupRedAI, ratGroupBlueAI, cheesePrefab;
    public List<GameObject> spawnLocations;
    public List<GameObject> cheeseSpawnLocations;
    public List<GameObject> powerUpsPrefab;
    public List<GameObject> redAISpawnLocation;
    public List<GameObject> blueAISpawnLocation;

    public bool Spawning = false;
    public bool SpawningCheese = false;
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
        if (this.SpawningCheese == false && isSpawningAI)
        {
            StartCoroutine(SpawnCheese());
        }
    }

    public IEnumerator SpawnUp()
    {
        this.Spawning = true;
        Debug.Log("Starting Spawn");
        yield return new WaitForSeconds(5);
        Debug.Log("Spawning now " + spawnLocations.Count);

        for (int i = 0; i < spawnLocations.Count;i++)
        {
            if (spawnLocations[i].GetComponent<PowerUpSpawnLocation>().MySpawnedPowerUp == null)
            {
                Debug.Log("Spawning");
                int random = Random.Range(0, powerUpsPrefab.Count);
                photonView.RPC("SpawnPowerUp", PhotonTargets.All, i, random);
            }
        }
        this.Spawning = false;
    }
    public IEnumerator SpawnCheese()
    {
        this.SpawningCheese = true;
        Debug.Log("Starting Spawn");
        yield return new WaitForSeconds(10);
        Debug.Log("Spawning now " + spawnLocations.Count);

        for (int i = 0; i < cheeseSpawnLocations.Count; i++)
        {
            if (cheeseSpawnLocations[i].GetComponent<PowerUpSpawnLocation>().MySpawnedPowerUp == null)
            {
                Debug.Log("Spawning");
                int random = Random.Range(0, powerUpsPrefab.Count);

                photonView.RPC("SpawnMyCheese", PhotonTargets.All, i);
            }
        }
        this.SpawningCheese = false;
    }
    // Spawns a powerup Based on a spawn Index
    [PunRPC]
    public void SpawnPowerUp(int spawnIndex, int powerUpIndex)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        GameObject powerUp = PhotonNetwork.Instantiate(powerUpsPrefab[powerUpIndex].name, spawnLocations[spawnIndex].transform.position, Quaternion.identity, 0);
        spawnLocations[spawnIndex].GetComponent<PowerUpSpawnLocation>().SpawnAPowerUp(powerUp); // Updates the location
    }

    [PunRPC]
    public void SpawnMyCheese(int spawnIndex)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        GameObject cheeseUp = PhotonNetwork.Instantiate(cheesePrefab.name, cheeseSpawnLocations[spawnIndex].transform.position, Quaternion.identity, 0);
        cheeseSpawnLocations[spawnIndex].GetComponent<PowerUpSpawnLocation>().SpawnAPowerUp(cheeseUp);
    }

    public void SpawnAI(string Group)
    {
        photonView.RPC("SpawnAGroup", PhotonTargets.All, Group);
    }

    [PunRPC]
    public void SpawnAGroup(string Group)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        switch (Group)
        {
            case "Red":
                for (int i = 0; i < redAISpawnLocation.Count; i++)
                {
                    GameObject RedRat = PhotonNetwork.Instantiate(ratGroupRedAI.name, redAISpawnLocation[i].transform.position, redAISpawnLocation[i].transform.rotation, 0);
                    RedRat.GetComponent<CharInfo>().MyTeam = PunTeams.Team.red;
                }
                break;

            case "Blue":
                for (int i = 0; i < blueAISpawnLocation.Count; i++)
                {
                    GameObject BlueRat = PhotonNetwork.Instantiate(ratGroupBlueAI.name, blueAISpawnLocation[i].transform.position, blueAISpawnLocation[i].transform.rotation, 0);
                    BlueRat.GetComponent<CharInfo>().MyTeam = PunTeams.Team.blue;
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
