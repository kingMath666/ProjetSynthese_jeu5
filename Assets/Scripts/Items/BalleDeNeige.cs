using System.Collections;
using UnityEngine;

/// <summary>
/// Gère le comportement de la balle de neige 
/// lorsqu'elle est ramassée par le joueur
/// #Élyzabelle Rollin
/// </summary>
public class BalleDeNeige : MonoBehaviour
{
    [Header("Données du joueur")]
    [SerializeField] SOPerso _donneesPerso; // Données du joueur

    [Header("Paramètres du flocon")]
    [SerializeField] AudioClip _sonCollecte; // Son de collecte
    Animator _animBalleDeNeige; // Animator du flocon
    [SerializeField, Range(0, 50)] int _delaisSuppression = 10; // Probabilité de collecte

    void Awake()
    {
        _animBalleDeNeige = GetComponent<Animator>(); // Rechercher l'animator
    }

    void Start()
    {
        StartCoroutine(DisparitionBalleDeNeige()); // Lancer la coroutine
    }

    void OnTriggerEnter(Collider other)
    {
        //Vérifier si le joueur entre en collision:
        if (other.tag == "perso")
        {
            _donneesPerso.AjouterBalleDeNeige();
            _animBalleDeNeige.SetTrigger("amasser"); // Jouer l'animation de collecte
        }
    }

    /// <summary>
    /// Fonction déclenché à la fin de l'animation de collecte
    /// Sert à jouer le son de collecte et à supprimer le flocon
    /// !!! NE PAS SUPPRIMER !!!
    /// </summary>
    void Ramasse()
    {
        if (_sonCollecte != null && GestAudio.instance != null) GestAudio.instance.JouerEffetSonore(_sonCollecte); // Jouer le son
        Destroy(gameObject); // Supprimer la balle de neige
    }

    /// <summary>
    /// Coroutine de suppression de la 
    /// balle de neige après un délais
    /// </summary>
    IEnumerator DisparitionBalleDeNeige()
    {
        // Attendre le 2/3 du temps avant de lancer l'animation de flash:
        yield return new WaitForSeconds(_delaisSuppression / 3 * 2);
        //Animation de flash quand il ne reste plus beaucoup de temps:
        _animBalleDeNeige.SetTrigger("flash");
        // Attendre le 1/3 du temps avant de lancer l'animation de mort:
        yield return new WaitForSeconds(_delaisSuppression / 3);
        _animBalleDeNeige.SetTrigger("mort"); // Jouer l'animation de mort
    }

    /// <summary>
    /// Destruction de la balle de neige
    /// </summary>
    void Destroy()
    {
        Destroy(gameObject); // Supprimer la balle de neige
    }
}
