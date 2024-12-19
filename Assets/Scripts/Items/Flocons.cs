using System.Collections;
using UnityEngine;

/// <summary>
/// Gère le comportement du flocon 
/// lorsqu'il est ramassé par le joueur
/// #Élyzabelle Rollin
/// </summary>
public class Flocons : MonoBehaviour
{
    [Header("Données du joueur")]
    [SerializeField] SOPerso _donneesPerso; // Données du joueur

    [Header("Paramètres du flocon")]
    [SerializeField] AudioClip _sonCollecte; // Son de collecte
    Animator _animFlocon; // Animator du flocon
    int collisionCount = 0; // Compteur de collisions
    [SerializeField, Range(0, 50)] int _delaisSuppression = 10; // Probabilité de collecte

    void Awake()
    {
        _animFlocon = GetComponent<Animator>(); // Rechercher l'animator
    }

    void Start()
    {
        StartCoroutine(DisparitionFlocon()); // Lancer la coroutine
    }

    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0); //Animation de rotation lorsque le flocon est en idle
    }

    void OnTriggerEnter(Collider other)
    {
        //Vérifier si le joueur entre en collision et qu'il a pas atteint la limite de flocons:
        if (other.tag == "perso" && _donneesPerso.nbFlocons < _donneesPerso.FLOCONS_MAX)
        {
            collisionCount++; // Incrémenter le compteur
            if (collisionCount == 2) // Vérifier si c'est la deuxième collision
            {
                _donneesPerso.AjouterFlocon(); // Ajouter un flocon au joueur
                _animFlocon.SetTrigger("amasser"); // Jouer l'animation de collecte
            }
        }
    }

    /// <summary>
    /// Fonction déclenché à la fin de l'animation de collecte
    /// Sert à jouer le son de collecte et à supprimer le flocon
    /// !!! NE PAS SUPPRIMER !!!
    /// </summary>
    void Ramasse()
    {
        if (_sonCollecte != null && GestAudio.instance != null) GestAudio.instance.JouerEffetSonore(_sonCollecte, 0.1f); // Jouer le son
        Destroy(gameObject); // Supprimer le flocon
    }

    /// <summary>
    /// Coroutine de suppression
    /// du flocon après un délais
    /// </summary>
    IEnumerator DisparitionFlocon()
    {
        // Attendre le 2/3 du temps avant de lancer l'animation de flash:
        yield return new WaitForSeconds(_delaisSuppression / 3 * 2);
        //Animation de flash quand il ne reste plus beaucoup de temps:
        _animFlocon.SetTrigger("flash");
        // Attendre le 1/3 du temps avant de lancer l'animation de mort:
        yield return new WaitForSeconds(_delaisSuppression / 3);
        _animFlocon.SetTrigger("mort"); // Jouer l'animation de mort
    }

    /// <summary>
    /// Destruction du flocon
    /// </summary>
    void Destroy()
    {
        Destroy(gameObject); // Supprimer le flocon
    }
}
