using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : Photon.PunBehaviour
{

    public PunTeams.Team FlagOwner = PunTeams.Team.none;
    public float RedCaptureValue = 0;
    public float BlueCaptureValue = 0;

    bool IsRedCapturing;
    bool IsBlueCapturing;

    Renderer MyRenderer;

    void Start()
    {
        if (MyRenderer == null)
        {
            MyRenderer = GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.RedCaptureValue < 10 && this.BlueCaptureValue < 10)
        {
            this.FlagOwner = PunTeams.Team.none;
        }

        if (FlagOwner == PunTeams.Team.none)
        {
            if (this.IsRedCapturing && !this.IsBlueCapturing)
            {
                float temp = this.RedCaptureValue / 10;
                MyRenderer.material.color = Color.Lerp(MyRenderer.material.color, Color.red, Mathf.PingPong(temp, 1));
            }
            if (!this.IsRedCapturing && this.IsBlueCapturing)
            {
                float temp = this.RedCaptureValue / 10;
                MyRenderer.material.color = Color.Lerp(MyRenderer.material.color, Color.blue, Mathf.PingPong(temp, 1));
            }
        }

        if (this.RedCaptureValue >= 10 && this.BlueCaptureValue < 10)
        {
            this.FlagOwner = PunTeams.Team.red;
        }

        if (this.BlueCaptureValue >= 10 && this.RedCaptureValue < 10)
        {
            this.FlagOwner = PunTeams.Team.blue;
        }


        if (FlagOwner == PunTeams.Team.red)
        {
            //MyRenderer.material.color = Color.red;

        }

        if (FlagOwner == PunTeams.Team.blue)
        {
            //MyRenderer.material.color = Color.blue;
        }


    }

    [PunRPC]
    public void RedTeamCapture()
    {
        this.IsRedCapturing = true;
        if (this.IsBlueCapturing)
        {
            return;
        }
        else
        {
            if (RedCaptureValue <= 10)
                RedCaptureValue += Time.deltaTime;
            else
                RedCaptureValue = 10;

            if (BlueCaptureValue > 0)
                BlueCaptureValue -= Time.deltaTime;
            else
                BlueCaptureValue = 0;
        }
    }
    [PunRPC]
    public void BlueTeamCapture()
    {
        this.IsBlueCapturing = true;
        if (this.IsRedCapturing)
        {
            return;
        }
        else
        {
            if (BlueCaptureValue <= 10)
                BlueCaptureValue += Time.deltaTime;
            else
                BlueCaptureValue = 10;

            if (RedCaptureValue > 0)
                RedCaptureValue -= Time.deltaTime;
            else
                RedCaptureValue = 0;
        }
    }
    [PunRPC]
    public void RunAwayBlueFromCollider()
    {
        this.IsBlueCapturing = false;
    }
    [PunRPC]
    public void RunAwayRedFromCollider()
    {

        this.IsRedCapturing = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.FlagOwner);
            stream.SendNext(this.RedCaptureValue);
            stream.SendNext(this.BlueCaptureValue);
            stream.SendNext(this.IsRedCapturing);
            stream.SendNext(this.IsBlueCapturing);
        }
        else
        {
            this.FlagOwner = (PunTeams.Team)stream.ReceiveNext();
            this.RedCaptureValue = (float)stream.ReceiveNext();
            this.BlueCaptureValue = (float)stream.ReceiveNext();
            this.IsRedCapturing = (bool)stream.ReceiveNext();
            this.IsBlueCapturing = (bool)stream.ReceiveNext();
        }
    }
}
