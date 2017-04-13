using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PunPlayerCat : MonoBehaviour
{
    public const string PlayerCatProp = "cat";
}

public static class CatExtensions
{
    public static void SetCat(this PhotonPlayer player, int catID)
    {
        Hashtable cat = new Hashtable();
        cat[PunPlayerCat.PlayerCatProp] = catID;

        player.SetCustomProperties(cat);
    }

    public static int GetCat(this PhotonPlayer player)
    {
        object cat;
        if (player.CustomProperties.TryGetValue(PunPlayerCat.PlayerCatProp, out cat))
        {
            return (int)cat;
        }

        return 0;
    }
}