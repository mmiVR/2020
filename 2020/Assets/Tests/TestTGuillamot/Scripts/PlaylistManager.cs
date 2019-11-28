using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlaylistManager : MonoBehaviour //Initialisation du manager
{
    public GameObject thumbnailPrefab;       //On passe le prefab des boutons

    public RenderTexture renderTexture;      //On passe la texture de rendu
    public Material renderMaterial;          //On passe le materiel de rendu pour la skybox

    public bool playing = false;             ///On s'en servira plus tard (Permettra de faire disparaitre le canvas lors de la lecture)

    public List<Sprite> thumbnails;          //Liste contenant les images des thumbnails
    public List<VideoClip> videos;           //Liste contenant les Videos

    private Dictionary<Sprite, VideoClip> playlist;    //Playlist contenant les thumbnails et les videos

    [HideInInspector] public VideoPlayer videoPlayer;  //Objet lecteur de video
    [HideInInspector] public AudioSource audioSource;  //Objet source audio

    private GameObject projector;            //Objet dans le quel le lecteur audio et videos seront crées, permet de garder un hierarchie plus propre

    private void Awake()
    {
        playlist = new Dictionary<Sprite, VideoClip>();
        if (thumbnails.Count == videos.Count)            //Test pour s'assurer qu'on declare autant de video que de thumbnails
        {                                                //Si c'est le cas, on crée le lecteur video / son et on crée la liste des boutons a afficher
            for (int i = 0; i < thumbnails.Count; i++)   //Pour chaque element de la liste, on...
            {
                playlist.Add(thumbnails[i], videos[i]);  //...Attribue a playlist[i] la thumbnail et la video que l'on veux afficher, il y a autant de cases dans playlist que de boutons
            }
            Debug.Log("<b>Disctionnaire de vidéos crée : <color=magenta>" + thumbnails.Count + " éléments chargés</color></b>");

            //Code pour init le projo (ptdr aled)
            projector = new GameObject("Projecteur Video 360");                       //Création de l'objet projector, comme expliqué plus haut, il permet de garder un hierarchie plus propre
            videoPlayer = projector.AddComponent<VideoPlayer>();                      //On ajoute le lecteur video a cet objet
            audioSource = projector.AddComponent<AudioSource>();                      //On ajoute le lecteur audio a cet objet

            
            videoPlayer.playOnAwake = false; audioSource.playOnAwake = false;         //On empeche le lancement automatique des medias
            videoPlayer.source = VideoSource.VideoClip;                               //On veux lire la video depuis un fichier et pas une url
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;           //On set la source audio en mode output
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture; //On set le mode de rendu en mode rendu texture (Pour pouvoir streamer la video sur une texture)
            videoPlayer.EnableAudioTrack(0, true);                                    //On assigne le son de la video a notre source audio
            videoPlayer.SetTargetAudioSource(0, audioSource);

            Debug.Log("<b>Le player a été initialisé. (got : " + videoPlayer + ").</b>");
        }
        else
        {
            Debug.Log("<b><color=red>Erreur lors de la création du dictionnaire : le nombre de videos et de thumbnails ne correspondent pas</color></b>");
        }
    }

    public void Start()
    {
        foreach (KeyValuePair<Sprite, VideoClip> playlistItem in playlist)                  //Pour chacuns de nos boutons...
        {
            GameObject newThumbnail = Instantiate(thumbnailPrefab, transform);              //On cree un nouveau objet bouton
            newThumbnail.GetComponent<Image>().sprite = playlistItem.Key;                   //On set sa thumbnail
            newThumbnail.GetComponent<ThumbnailButton>().videos = playlistItem.Value;       //On set la video qu'il permettra de lire
            newThumbnail.GetComponent<ThumbnailButton>().renderTexture = renderTexture;     //On set la texture sur la quelle il va rendre la video
            newThumbnail.GetComponent<ThumbnailButton>().renderMaterial = renderMaterial;   //On set le materiel qui servira a update la skybox
            Debug.Log("<b>Element " + playlistItem.Value + " crée.</b>");                   //Puis on logue sa création dans la console :)
        }
    }
}
