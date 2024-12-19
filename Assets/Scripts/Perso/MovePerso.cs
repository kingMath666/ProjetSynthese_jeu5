using System;
using UnityEngine;

/// <summary>
/// Classe qui gère les déplacements du personnage
/// #Élyzabelle Rollin
/// </summary>
public class MovePerso : MonoBehaviour
{
    [Header("Caméra")]
    [SerializeField] private Camera _cam; // Caméra du joueur

    [Header("Champ de force")]
    [SerializeField] private Material _champDeForceMaterial; // Matériau du champ de force

    [Header("Données du perso")]
    [SerializeField] SOPerso _donneesPerso; // Données du perso
    private Animator _animPerso; // Animator du perso 
    public CharacterController _controller; // CharacterController du perso

    [Header("Paramètres de mouvement")]
    [SerializeField] private float _vitesseMouvement = 20.0f; // Vitesse de mouvement
    [SerializeField] private float _vitesseRotation = 3.0f; // Vitesse de rotation
    [SerializeField] private float _gravite = 400f; // Gravité
    [SerializeField] private float _hauteurSaut = 10f; // Hauteur du saut
    private Vector3 _directionsMouvement = Vector3.zero; // Directions de mouvement
    private float _vitesseVerticale; // Vitesse verticale
    private float _vitesse; // Vitesse du personnage
    private bool _estEnGlissade = false; // Etat de glissade
    private float _glissement = 1.5f; // Coefficient de glissade
    private float _frictionGlissade = 0.5f; // Friction de glissade

    private Vector3 _pointDeRespawn = new Vector3(0, 10, 0); // Point de respawn par défaut

    void Awake()
    {
        _animPerso = GetComponent<Animator>(); // Rechercher l'animator
        _controller = GetComponent<CharacterController>(); // Rechercher le CharacterController
    }

    void Update()
    {
        // Initialisation de la direction de mouvement
        _directionsMouvement = Vector3.zero;

        // Gestion de la gravité et du saut
        if (_controller.isGrounded)
        {
            _vitesseVerticale = 0; // Réinitialiser la vitesse verticale au sol
            if (Input.GetButtonDown("Jump")) // Saut
            {
                _vitesseVerticale = Mathf.Sqrt(2 * _gravite * _hauteurSaut);
            }
        }
        else
        {
            _vitesseVerticale -= _gravite * 150 * Time.deltaTime; // Appliquer la gravité
        }

        // Gestion du mouvement horizontal si le personnage peut bouger
        if (_donneesPerso.bougerPerso)
        {
            // Rotation du personnage
            float rotationInput = Input.GetAxis("Horizontal");
            transform.Rotate(0, rotationInput * _vitesseRotation, 0);

            // Calcul de la vitesse du mouvement
            float inputVertical = Input.GetAxis("Vertical");
            _vitesse = _vitesseMouvement * inputVertical;

            if (_estEnGlissade)
            {
                _vitesse *= _glissement; // Augmentation de la vitesse de glissade
                if (_vitesse > _frictionGlissade)
                {
                    _vitesse -= _frictionGlissade;
                }
                else
                {
                    _vitesse = 0;
                }
            }

            _animPerso.SetBool("course", _vitesse > 0); // Activation de l'animation "enCourse"
            _animPerso.SetBool("recul", inputVertical < 0 && !_animPerso.GetBool("course")); // Animation de recul

            Vector3 directionHorizontale = transform.TransformDirection(new Vector3(0, 0, _vitesse));
            _directionsMouvement += directionHorizontale;
        }
        else
        {
            _vitesse = 0; // Si le personnage ne peut bouger, la vitesse est nulle
            _animPerso.SetBool("course", false); // Désactiver l'animation "enCourse"
        }

        // Appliquer le mouvement, y compris la gravité et la vitesse verticale
        _directionsMouvement.y = _vitesseVerticale;
        _controller.Move(_directionsMouvement * Time.deltaTime);

        // Ajuster le champ de vision de la caméra en fonction de la vitesse
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _estEnGlissade ? 80f : 80f + _vitesseMouvement * Input.GetAxis("Vertical"), Time.deltaTime * 5);

        // Modifier l'opacité en fonction de la vitesse
        float nouvelleOpacite = Mathf.Lerp(_champDeForceMaterial.GetFloat("_Opacity"), (_vitesse > 0 && _controller.isGrounded) ? 1f : 0f, Time.deltaTime * 7);
        _champDeForceMaterial.SetFloat("_Opacity", nouvelleOpacite);

        AntiSuicide(); // Appel de la méthode AntiSuicide pour prévenir les chutes infinies
    }

    /// <summary>
    /// Méthode pour prévenir les chutes infinies
    /// </summary>
    void AntiSuicide()
    {
        // Prévenir les chutes en dehors du terrain
        if (transform.position.y < -10)
        {
            _controller.enabled = false; // Désactivation du CharacterController
            transform.position = _pointDeRespawn; // Remise en position sécurisée
            _vitesseVerticale = 0; // Réinitialisation de la vitesse verticale
            _controller.enabled = true; // Activation du CharacterController
        }
    }

    /// <summary>
    /// Méthode pour activer ou non l'état de glissade
    /// </summary>
    /// <param name="enGlissade"></param>
    public void ActiverGlissade(bool enGlissade)
    {
        _estEnGlissade = enGlissade; // Activer ou non l'etat de glissade
    }
}
