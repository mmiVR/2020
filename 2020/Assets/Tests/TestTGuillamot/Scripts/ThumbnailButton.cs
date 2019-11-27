using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailButton : Button //Initialisation du bouton
{
    public string videoURL; //Url de la video a lire


    protected override void Awake()
    {
        //Code pour init le projo (ptdr aled)
    }

    protected override void Start()
    {
        onClick.AddListener(PlayVideo); //Execute la fonction PlayVideo lorsque le bouton est cliqué
    }

    private void PlayVideo()
    {
        //Oui super, ça va etre relou
        print("PLAY : " + videoURL);
        //Mais l'argument passe alors c'est good
    }
}
