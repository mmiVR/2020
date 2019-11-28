using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ThumbnailButton : Button     //Initialisation du bouton
{
    [HideInInspector] public VideoClip videos;              //Video a lire

    [HideInInspector] public RenderTexture renderTexture;   //On recupere la texture de rendu
    [HideInInspector] public Material renderMaterial;       //On recupere le materiel de rendu pour la skybox

    private VideoPlayer videoPlayer;      //Objet lecteur de video
    private AudioSource audioSource;      //Objet source audio


    protected override void Awake()
    {
        videoPlayer = gameObject.GetComponentInParent<PlaylistManager>().videoPlayer;   //On recupere le lecteur video
        audioSource = gameObject.GetComponentInParent<PlaylistManager>().audioSource;   //On recupere le lecteur audio
    }

    protected override void Start()
    {
        onClick.AddListener(PlayVideo);   //Execute la fonction PlayVideo lorsque le bouton est cliqué
    }

    private void PlayVideo()
    {
        RenderSettings.skybox = renderMaterial;      //Set le rendu de la skybox avec le materiel sur le quel est streamé la video, c'est ceci qui permet d'afficher la video a 360 degrés

        videoPlayer.targetTexture = renderTexture;   //Set la texture sur la quelle la video va streamer ses frames
        videoPlayer.clip = videos;                   //Set la video a charger
        videoPlayer.Prepare();                       //Pre charge le son pour eviter des problemes de buffering


        videoPlayer.Play();                          //Lance la video
        audioSource.Play();                          //lance l' audio

        gameObject.GetComponentInParent<PlaylistManager>().playing = true;   //Set la variable "playing" du playlistManager sur true
        Debug.Log("<b>Lancement de la lecture de la video " + videos.name + "</b>");
    }
}
