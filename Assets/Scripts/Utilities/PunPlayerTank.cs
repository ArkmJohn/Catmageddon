using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PunPlayerTank : MonoBehaviour
{
    public const string PlayerTankProp = "tank";
}

public static class TankExtensions
{
    public static void SetTank(this PhotonPlayer player, int tankID)
    {
        Hashtable tank = new Hashtable();
        tank[PunPlayerTank.PlayerTankProp] = tankID;

        player.SetCustomProperties(tank);
    }

    public static int GetTank(this PhotonPlayer player)
    {
        object tank;
        if(player.CustomProperties.TryGetValue(PunPlayerTank.PlayerTankProp, out tank))
        {
            return (int)tank;
        }

        return 0;
    }
}