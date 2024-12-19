using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.AI.Navigation;

/// <summary>
///  Classe permettant de gérer le mini jeu
/// #Patrick Watt-Charron
/// </summary>
public class MiniJeu : MonoBehaviour
{
    [Header("Ile")]
    [SerializeField] Transform _conteneurIle;// Le conteneur de l'ile
    [SerializeField] GameObject[] _posIle;// Les positions de l'ile
    [SerializeField] GameObject _prefabeCube;// Le prefab de l'ile
    [SerializeField] GameObject _decor;// Le prefab de l'ile

    [Header("Instructions")]
    [SerializeField] GameObject _prefabInstructions;// Le prefab de l'ile

    [Header("Paramètres de la balle de neige")]
    public RectTransform cursorImage; // Référence à l'image du curseur
    public RectTransform boundary; // Zone définissant les limites (Canvas ou autre RectTransform)
    [SerializeField] SOPerso _donneesPerso;// Données du joueur
    [SerializeField] SONavigation _navigation;// Données de navigation
    [SerializeField] AudioClip _sonLancer;// Son de lancer de balle de neige
    [SerializeField] Transform _conteneurBalleDeNeige;// Le conteneur de la balle de neige
    [SerializeField] GameObject _prefabBalleDeNeigeAttaque;// Le prefab de la balle de neige
    [SerializeField] float forceDeLancement = 500f; // Force du lancer
    Camera mainCamera;// La caméra principale

    [Header("Ennemi")]
    [SerializeField] List<Vector3> _pos;// Les positions des ennemis
    [SerializeField] SONiveau _donneesNiveau;// Données du niveau
    List<GameObject> _ennemisListe = new List<GameObject>();// La liste des ennemis
    [SerializeField] Transform _conteneurEnnemis;// Le conteneur des ennemis


    void Awake()
    {
        _decor.SetActive(false);// Désactiver le décor
        Invoke("CloseInstructions", 5f);// appeller la fonction CloseInstructions apres 5 secondes
        GenererIle();// Générer l'ile
    }

    void GenererIle()
    {
        // Parcourir chaque élément dans le tableau _posIle
        foreach (GameObject posIle in _posIle)
        {
            if (posIle != null) // Vérifier que l'objet n'est pas nul
            {
                // Récupérer la position de l'objet
                Vector3 position = posIle.transform.position;

                // Instancier un cube à cette position
                GameObject cube = Instantiate(_prefabeCube, position, Quaternion.identity, _conteneurIle);
            }
        }
    }

    void Start()
    {
        mainCamera = Camera.main;// Récupérer la caméra principale
        _conteneurIle.GetComponent<NavMeshSurface>().BuildNavMesh();// Construire le NavMesh
        _decor.SetActive(true);// Activer le décor
        PlacerEnnemis(_donneesNiveau.nbEnnemis - _donneesNiveau.nbTuer);// Instantier les ennemis
    }
    void CloseInstructions()
    {
        _prefabInstructions.SetActive(false);// Désactiver le panneau d'instructions
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;// Récupérer la position de la souris

        // Limiter la position à la zone définie
        Vector2 clampedPosition = ClampToBoundary(mousePosition);// Limiter la position à la zone définie

        // Déplacer l'image du curseur
        cursorImage.position = clampedPosition;// Positionner l'image du curseur
        if (!_prefabInstructions.activeSelf)// Vérifier si le panneau d'instructions est actif
        {
            // Vérifier si le bouton de la souris est pressé
            if (Input.GetMouseButtonDown(0))
            {
                if (_donneesPerso.nbBalleDeNeige > 0)// Vérifier si le joueur a une balle de neige
                {
                    GestAudio.instance.JouerEffetSonore(_sonLancer);// Jouer le son
                    LancerBalleDeNeige(clampedPosition);// Lancer la balle de neige
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && _donneesPerso.tutorielActif2 == false)// Vérifier si le joueur appuie sur E et qu'il est pas dans un tutoriel
            {
                _navigation.RetourJeu();// Retourner au jeu
            }
            else if (Input.GetKeyDown(KeyCode.E) && _donneesPerso.tutorielActif2 == true)// Vérifier si le joueur appuie sur E et qu'il est dans un tutoriel
            {
                _navigation.AllerSceneTutoriel2();// Aller au tutoriel 2
            }
        }

        if (_donneesNiveau.nbTuer == _donneesNiveau.nbEnnemis)// Vérifier si tous les ennemis sont tuer
        {
            RetourJeu();// Retourner au jeu
        }

    }

    /// <summary>
    /// Retourner au jeu
    /// </summary>
    private void RetourJeu()
    {
        if (_donneesPerso.tutorielActif2)// Vérifier si le joueur est dans un tutoriel
        {
            _navigation.Jouer();// Aller au jeu
        }
        else
        {
            _navigation.RetourJeu();// Retourner au jeu
        }

    }
    private Vector2 ClampToBoundary(Vector2 position)
    {
        if (boundary != null) // Si la zone de détection des limites existe
        {
            Rect boundaryRect = boundary.rect; // Récupère le rectangle des limites

            // Convertit la position de l'écran en coordonnées locales à l'intérieur de la zone boundary
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                boundary,
                position,
                null,
                out Vector2 localPoint
            );

            // Limite la position du curseur à l'intérieur de la zone
            Vector2 clampedLocalPoint = new Vector2(
                Mathf.Clamp(localPoint.x, boundaryRect.xMin, boundaryRect.xMax),
                Mathf.Clamp(localPoint.y, boundaryRect.yMin, boundaryRect.yMax)
            );

            return boundary.TransformPoint(clampedLocalPoint); // Convertit la position locale en position de l'écran et la retourne
        }

        return position; // Si la zone de détection n'existe pas, retourne la position d'origine
    }

    private void LancerBalleDeNeige(Vector2 targetPosition)
    {
        // Convertit la position du curseur en coordonnées du monde en prenant en compte la profondeur de la scène
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(
            targetPosition.x,
            targetPosition.y,
            10f // Distance à partir de la caméra (ajustée pour être correcte dans la scène)
        ));

        // Crée une balle de neige à la position du curseur dans le monde
        GameObject balleDeNeige = Instantiate(
            _prefabBalleDeNeigeAttaque,
            worldPosition, // Utilise la position calculée
            Quaternion.identity,
            _conteneurBalleDeNeige
        );

        // Applique une force à la balle de neige uniquement sur l'axe Z
        Rigidbody rb = balleDeNeige.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Déplace la balle de neige uniquement sur l'axe Z
            Vector3 direction = Vector3.forward; // Direction du mouvement dans l'axe Z
            rb.AddForce(direction * forceDeLancement, ForceMode.Impulse); // Applique la force de lancement
        }
        _donneesPerso.RetirerBalleDeNeige(); // Retire une balle de neige de l'inventaire du personnage
    }

    void PlacerEnnemis(int nbEnnemis)
    {
        for (int i = 0; i < nbEnnemis; i++) // Place les ennemis selon le nombre défini
        {
            Vector3 pos = _pos[i]; // Récupère la position du prochain ennemi
            pos.y = pos.y + 1.5f; // Ajuste la position sur l'axe Y pour placer l'ennemi à la hauteur souhaitée
            Vector3 cible = pos;
            cible.x = cible.x + 15f; // Ajuste la cible de l'ennemi

            // Instancie un ennemi à la position donnée
            GameObject unEnnemi = Instantiate((GameObject)Resources.Load("Ennemis/MiniJeu/Ennemi1"), pos + Vector3.up * 3, Quaternion.identity, _conteneurEnnemis);
            unEnnemi.transform.localScale = unEnnemi.transform.localScale; // Conserve la taille de l'ennemi
            unEnnemi.GetComponent<EnnemisEtatsManager>().infos.Add("maison", pos); // Ajoute des informations sur l'ennemi (sa maison et cible)
            unEnnemi.GetComponent<EnnemisEtatsManager>().infos.Add("cible", cible);
            unEnnemi.GetComponent<EnnemisEtatsManager>().infos.Add("ile", this);
            unEnnemi.GetComponent<EnnemisEtatsManager>().infos.Add("perso", "miniJeu");
            _ennemisListe.Add(unEnnemi); // Ajoute l'ennemi à la liste des ennemis
        }
    }
}