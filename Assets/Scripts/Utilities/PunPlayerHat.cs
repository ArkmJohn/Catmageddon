using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PunPlayerHat : MonoBehaviour
{
    public const string PlayerHatProp = "hat";
}

public static class HatExtensions
{
    public static void SetHat(this PhotonPlayer player, int hatID)
    {
        Hashtable hat = new Hashtable();
        hat[PunPlayerHat.PlayerHatProp] = hatID;

        player.SetCustomProperties(hat);
    }

    public static int GetHat(this PhotonPlayer player)
    {
        object hat;
        if (player.CustomProperties.TryGetValue(PunPlayerHat.PlayerHatProp, out hat))
        {
            return (int)hat;
        }

        return 0;
    }
}