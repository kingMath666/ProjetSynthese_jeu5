using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Classe permettant de gérer le comportement du joueur
/// #Patrick Watt-Charron
/// </summary>
public class Perso : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // Référence aux données du personnage (informations liées aux balles de neige, tutoriel, etc.)
    [SerializeField] SONiveau _donneesNiveau; // Référence aux données du niveau (comme le nombre d'ennemis à tuer)
    [SerializeField] GameObject _prefabBonhomeDeNeige; // Référence au prefab du bonhomme de neige à instancier
    [SerializeField] GameObject _prefabBalleDeNeige; // Référence au prefab de la balle de neige utilisée par les bonshommes de neige
    [SerializeField] Transform _conteneurAmi; // Référence au conteneur où les bonshommes de neige (amis) seront instanciés
    [SerializeField] Transform _conteneurBalleDeNeige; // Conteneur pour les balles de neige générées
    [SerializeField] AudioClip _sonInstanciationAmi; // Son joué lors de l'instanciation d'un bonhomme de neige
    [SerializeField] SONavigation _navigation; // Référence au gestionnaire de navigation pour gérer les interactions du jeu
    [SerializeField] GameObject _texteMiniJeu; // Référence au texte affiché pendant le mini-jeu (indiquant que le joueur doit avoir des balles de neige pour continuer)

    void Start()
    {
        if (_donneesPerso.tutorielActif1) _texteMiniJeu.SetActive(false);
        StartCoroutine(VerificationDistanceLac()); // Démarre une coroutine pour vérifier la distance du personnage au lac en boucle
    }

    // Méthode pour invoquer un bonhomme de neige lorsque la grosse boule de neige est prête
    public void InvokeBonhomeDeNeige(Transform grosseBoulePos)
    {
        // Joue le son de création du bonhomme de neige
        if (GestAudio.instance) GestAudio.instance.JouerEffetSonore(_sonInstanciationAmi);

        // Achète un bonhomme de neige (diminution de la quantité de balles de neige)
        _donneesPerso.AcheterBonhomeDeNeige();

        // Crée le bonhomme de neige à la position de la grosse boule de neige avec un décalage vertical
        GameObject unAmi = Instantiate(_prefabBonhomeDeNeige, grosseBoulePos.position + Vector3.up * 3, Quaternion.identity, _conteneurAmi);

        // Ajoute des informations sur la balle de neige et le conteneur de balles de neige dans l'objet Ami
        unAmi.GetComponent<AmisEtatsManager>().infos.Add("prefabBalleDeNeige", _prefabBalleDeNeige);
        unAmi.GetComponent<AmisEtatsManager>().infos.Add("conteneurBalleDeNeige", _conteneurBalleDeNeige);
    }

    // Coroutine pour vérifier la distance entre le joueur et le lac
    private IEnumerator VerificationDistanceLac()
    {
        int nbEnnemis = _donneesNiveau.nbEnnemis - _donneesNiveau.nbTuer; // Calcul du nombre d'ennemis restants (total - tués)

        // Cette boucle s'exécute tant que le tutoriel 1 est actif
        while (_donneesPerso.tutorielActif1 == false)
        {
            // Cherche un objet avec le tag "lac" dans la scène
            GameObject lac = GameObject.FindWithTag("lac");

            // Si aucun objet avec le tag "lac" n'est trouvé, attendre 1 seconde et réessayer
            while (lac == null)
            {
                yield return new WaitForSeconds(1f); // Attendre une seconde avant de réessayer
                lac = GameObject.FindWithTag("lac"); // Rechercher à nouveau le lac
            }

            // Calcule la distance entre le personnage et le lac
            float distance = Vector3.Distance(transform.position, lac.transform.position);
            if (_texteMiniJeu != null) _texteMiniJeu.SetActive(false); // Masque le texte du mini-jeu au début de la vérification

            // Si le personnage est suffisamment proche du lac (moins de 22 unités) et qu'il a encore des balles de neige et qu'il reste des ennemis
            if (distance < 22f)
            {
                if (_donneesPerso.nbBalleDeNeige > 0 && nbEnnemis > 0)
                {
                    _navigation.MiniJeu(); // Lance le mini-jeu
                }
                // Si le personnage n'a pas de balle de neige mais qu'il reste des ennemis, affiche le texte pour prévenir le joueur
                else if (_donneesPerso.nbBalleDeNeige <= 0 && nbEnnemis > 0 && _texteMiniJeu != null)
                {
                    if (GestAudio.instance != null && _donneesPerso.sonManqueBalles != null) GestAudio.instance.JouerVoiceOver(_donneesPerso.sonManqueBalles[Random.Range(0, _donneesPerso.sonManqueBalles.Length)]);
                    _texteMiniJeu.SetActive(true); // Affiche le texte pour dire au joueur de collecter des balles de neige
                    _donneesPerso.bougerPerso = true;
                }
            }

            // Attendre encore 1 seconde avant de refaire la vérification
            yield return new WaitForSeconds(1f);
        }
    }
}