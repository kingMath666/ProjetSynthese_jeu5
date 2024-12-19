using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

/// <summary>
/// Classe permettant de générer une île procédurale.
/// #Élyzabelle Rollin
/// </summary>
public class GenerateurIle : MonoBehaviour
{
    [Header("Paramètres de l'île")]
    [SerializeField, Range(10, 1000)] int _ileLargeur; // x
    [SerializeField, Range(10, 1000)] int _ileProfondeur; // z
    [SerializeField, Range(2, 100)] float _forcePerlin = 14f; // "zoom" de la carte
    [SerializeField, Range(2, 100)] float _forcePerlinVariants = 14f; // "zoom" de la carte
    [SerializeField, Range(2, 100)] float _forcePerlinBiomes = 14f; // "zoom" de la carte
    [SerializeField, Range(1, 50)] float _k = 8f; // k = degré d'immersion de l'île
    [SerializeField, Range(0f, 1f)] float _c = 0.4f; // % de l'île hors de l'eau
    [SerializeField, Range(0f, 1f)] int _bruitMin = 0; // Valeur minimale du bruit
    [SerializeField, Range(10, 50)] int _coefHauteur = 20; // Augmente l'amplitude des "vagues"
    [SerializeField, Range(0f, 1f)] int _bruitMax = 100000; // Valeur maximale du bruit

    [Header("Perso")]
    [SerializeField] SOPerso _donneesPerso; // Données du personnage
    BoxCollider _colliderPerso; // Collider du personnage

    [Header("Conteneurs")]
    [SerializeField] Transform _conteneurIle; // Parent qui contient les cubes
    [SerializeField] Transform _conteneurEnnemis; // Parent qui contient les personnages
    [SerializeField] Transform _conteneurNPC; // Parent qui contient les personnages
    [SerializeField] Transform _conteneurObjets; // Parent qui contient les personnages

    [Header("Prefabs")]
    [SerializeField] GameObject _prefabPerso; // Prefab du personnage
    [SerializeField] GameObject _prefabCube; // Cube utilisé pour former l'île
    [SerializeField] GameObject _plane; // Plane servant à visualiser l'île avant le mode jeu 
    [SerializeField] Renderer _textureRenderer; // Texture du plane
    [SerializeField] GameObject _prefabLac; //Prefab du lac


    [Header("NPC")]
    [SerializeField] GameObject _prefabNPC; // Prefab du NPC
    [SerializeField, Range(0, 500)] int _nbNPC = 1; // Nombre de NPC à ajouter au jeu

    [Header("Ennemi")]
    private int _nbEnnemis = 1; // Nombre d'ennemis à ajouter au jeu
    [SerializeField] GameObject _prefabEnnemi; // Prefab de l'ennemi
    List<GameObject> _ennemisListe = new List<GameObject>(); // Liste des ennemis #Patrick Watt-Charron
    [SerializeField] SONiveau _donneesNiveau; // Données du niveau

    [Header("Biômes et variantes")]
    List<Vector3> _posCubesVisibles = new List<Vector3>(); // Liste des positions des cubes visibles
    List<List<Material>> _biomesMats = new List<List<Material>>(); // Contient les biomes et leurs variantes
    private List<BiomesEtatsManager> _biomesListe = new List<BiomesEtatsManager>(); // Liste des biomes

    void Awake()
    {
        GenererCarte(); // Genère l'île
        _colliderPerso = _prefabPerso.GetComponent<BoxCollider>(); // Rechercher le collider du personnage
    }

    void Start()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh(); // Construire le NavMesh
        //Instantier le lac:
        GameObject lac = Instantiate(_prefabLac, ChoisirCube() + Vector3.up, Quaternion.identity);
        _nbEnnemis = _donneesNiveau.nbEnnemis - _donneesNiveau.nbTuer;
        PlacerEnnemi(_nbEnnemis, lac); // Instantier les ennemis
        PlacerNPC(lac); // Instantier les NPCs
        _donneesPerso.nbPingouinsIni = _nbNPC; // Passer le nombre de pingouins à capturer au personnage
    }

    /// <summary>
    /// méthode qui reçoit une clé et une valeur et qui retourne 
    /// une liste de tous les biomes qui ont cette correspondance
    ///  clé/valeur dans leur dictionnaire d'infos.
    /// </summary>
    /// <param name="info"> Clé du dictionnaire d'infos </param>
    /// <param name="valeur"> Valeur correspondante à la clé </param>
    /// <returns></returns>
    public List<BiomesEtatsManager> ChercheBiomes(string info, dynamic valeur)
    {
        //création d'une liste temporaire vide tempListe de BiomesEtatsManager:
        List<BiomesEtatsManager> tempListe = new List<BiomesEtatsManager>();
        //itération for(i…) à travers biomeListe (la liste de tous les biomes de l'ile):
        for (int i = 0; i < _biomesListe.Count; i++)
        {
            //si le dictionnaire infos du biome contient la clé passée en paramètre - string info):
            if (_biomesListe[i]._infos.ContainsKey(info))
            {
                //et si la valeur stockée dans cette clé est égale a la valeur passée en paramètre - dynamic valeur):
                if (_biomesListe[i]._infos[info].Equals(valeur)) // on utilise Equals() plutôt que == car on manipule des valeurs dynamiques
                {
                    // alors ajoute le biome à la liste temporaire:
                    tempListe.Add(_biomesListe[i]);
                }
            }
        }
        //retourne la liste temporaire qui contient tous les biomes qui ont la valeur recherchée dans la clé demandée:
        return tempListe;
    }

    /// <summary>
    /// Fonction qui permet de charger les matériaux de façon dynamique
    /// à l'aide des biomes et du nombre de variantes.
    /// Ajoute ces matériaux à la liste de matériaux globale.
    /// </summary>
    private void LoadRessources()
    {
        int nbBiomes = 1; // Nombre de biomes
        int nbVariantes = 1; // Nombre de variantes pour chaque biome
        bool resteDesMats = true; // Vrai s'il y a encore des matériaux
        List<Material> tpBiome = new List<Material>();  // Contient les matériaux pour chaque biome

        do
        {
            // Resources.Load va chercher un objet basé sur un chemin (string) :
            Object mats = Resources.Load("Biomes/Biome" + nbBiomes + "/b1_" + nbVariantes);
            if (mats) // Si le matériau existe
            {
                tpBiome.Add((Material)mats); // Ajoute le matériau à la liste
                nbVariantes++; // Incrémente le nombre de variantes
            }
            else // Si le matériau n'existe pas
            {
                if (nbVariantes == 1) resteDesMats = false; // Il ne reste plus de matériaux
                else // Passage au biome suivant
                {
                    _biomesMats.Add(tpBiome); // Ajoute les matériaux collectés à la liste globale
                    tpBiome = new List<Material>(); // Vide la liste
                    nbBiomes++; // Incrémente le nombre de biomes
                    nbVariantes = 1; // Réinitialise le nombre de variantes
                }
            }
        }
        while (resteDesMats); // Tant qu'il reste des matériaux
    }


    /// <summary>
    /// Fonction qui permet de générer une carte à 
    /// l'aide d'une largeur, d'une profondeur et 
    /// d'une force Perlin
    /// </summary>
    void GenererCarte()
    {
        LoadRessources();
        // AjouterMats(); //Ajoute les biomes et leurs variantes
        float[,] uneCarte = Terraformer(_ileLargeur, _ileProfondeur, _forcePerlin); // Crée une carte
        float[,] uneCarteBiomes = Terraformer(_ileLargeur, _ileProfondeur, _forcePerlinBiomes); // Crée une nouvelle carte pour les objets (variant)
        float[,] uneCarteVariants = Terraformer(_ileLargeur, _ileProfondeur, _forcePerlinVariants); // Crée une nouvelle carte pour les objets (variant)
        uneCarte = AquaFormeCirculaire(uneCarte); //Rend la carte circulaire
        AfficherIle(uneCarte, uneCarteBiomes, uneCarteVariants); // Affiche l'île
    }

    /// <summary>
    /// Fonction qui crée un terrain 
    /// avec variation  
    /// </summary>
    /// <param name="largeur">Largeur du terrain</param>
    /// <param name="profondeur">Profondeur du terrain</param>
    /// <param name="fP">Zoom du bruit</param>
    /// <returns>Retourne un terrain dans un tableau 2D</returns>
    float[,] Terraformer(int largeur, int profondeur, float fP)
    {
        float[,] terrain = new float[largeur, profondeur]; // Creer un terrain avec les dimensions demandées
        int bruit = Random.Range(_bruitMin, _bruitMax); // Valeur aléatoire du bruit
        for (int z = 0; z < profondeur; z++) //Crée la profondeur du terrain
        {
            for (int x = 0; x < largeur; x++) //Crée la largeur du terrain
            {
                float y = Mathf.PerlinNoise((x / fP) + bruit, (z / fP) + bruit); // Calcul Perlin
                terrain[x, z] = y; // Affecte Perlin à la carte
            }
        }
        return terrain; // Retourne le terrain
    }

    /// <summary>
    /// Rend le terrain au bord de l'eau plat
    /// </summary>
    /// <param name="terrain">Terrain à modifier</param>
    /// <returns>Terrain modifié</returns>
    float[,] AquaForme(float[,] terrain)
    {
        int l = terrain.GetLength(0); // Largeur
        int p = terrain.GetLength(1); // Profondeur
        for (int z = 0; z < p; z++) //Parcourt la profondeur
        {
            for (int x = 0; x < l; x++) //Parcourt la largeur
            {
                float dx = x / (float)l * 2 - 1; //Tente de voir si le point en x est sur le bord 
                float dz = z / (float)p * 2 - 1; //Tente de voir si le point en y est sur le bord
                float val = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dz)); //Prend la valeur la plus grande
                val = Sigmoid(val); //Transforme la valeur en un float entre 0 et 1
                terrain[x, z] = Mathf.Clamp01(terrain[x, z] - val); // Ajuste le terrain
            }
        }
        return terrain; // Retourne le terrain
    }

    /// <summary>
    /// Rend le terrain au bord 
    /// de l'eau plat et circulaire
    /// </summary>
    /// <param name="terrain">Terrain à modifier</param>
    /// <returns>Terrain modifié</returns>
    float[,] AquaFormeCirculaire(float[,] terrain)
    {
        int l = terrain.GetLength(0); // Largeur
        int p = terrain.GetLength(1); // Profondeur
        for (int z = 0; z < p; z++) //Parcourt la profondeur
        {
            for (int x = 0; x < l; x++) //Parcourt la largeur
            {
                // Calcul de la distance entre le point au centre et les coordonnées (x, z):
                float d = Vector2.Distance(new Vector2(_ileLargeur / 2, _ileProfondeur / 2), new Vector2(x, z));
                // Sert à déterminer si les points sont sur la circonférence du cercle:
                float ratioLargeur = d / (_ileLargeur / 2); //Sert à déterminer si les points sont sur la circonférence du cercle
                float ratioProfondeur = d / (_ileProfondeur / 2); //Sert à déterminer si les points sont sur la circonférence du cercle
                float val = Mathf.Max(Mathf.Abs(ratioLargeur), Mathf.Abs(ratioProfondeur)); //Prend la valeur la plus grande
                val = Sigmoid(val); // Résultat entre 0 et 1
                terrain[x, z] = Mathf.Clamp01(terrain[x, z] - val); // Ajuste le terrain
            }
        }
        return terrain; // Retourne le terrain
    }



    /// <summary>
    /// Affiche l'île sur la scène
    /// </summary>
    /// <param name="ile">Terrain à afficher</param>
    void AfficherIle(float[,] ile, float[,] ileBiomes, float[,] ileVariants)
    {
        int l = ile.GetLength(0); // Largeur
        int p = ile.GetLength(1); // Profondeur

        Texture2D texture = new Texture2D(l, p); // Taille de la texture

        for (int z = 0; z < p; z++) // Parcourt la profondeur
        {
            for (int x = 0; x < l; x++) // Parcourt la largeur
            {
                float y = ile[x, z]; // Donne la valeur de y pour chaque cube
                Color couleur = new Color(y, y, y); // Choisir une couleur
                texture.SetPixel(x, z, couleur); // Fixer la couleur d'un pixel
                if (y > 0) //Si la valeur de y est supérieure à 0
                {

                    GameObject unCube = Instantiate(_prefabCube, new Vector3(x - (_ileLargeur / 2), y * _coefHauteur, z - (_ileProfondeur / 2)), Quaternion.identity, _conteneurIle); // Instancie le cube
                    _posCubesVisibles.Add(new Vector3(x, y * _coefHauteur, z)); // Ajoute la position du cube à la liste

                    int quelBiome = Mathf.RoundToInt(ileBiomes[x, z] * (_biomesMats.Count - 1)); // Choisit le biome
                    int quelVariant = Mathf.RoundToInt(ileVariants[x, z] * (_biomesMats[0].Count - 1)); // Choisit le variant
                    unCube.GetComponent<Renderer>().material = _biomesMats[quelBiome][quelVariant]; //Change le matériel du cube en fonction du biome et de du variant
                    unCube.GetComponent<BiomesEtatsManager>()._biomesMats = _biomesMats; //Référence aux biomes pour BiomesEtatsManager
                    unCube.GetComponent<BiomesEtatsManager>()._infos.Add("biomePos", unCube.transform.position); // Envoi la position du biome à BiomesEtatsManager
                    unCube.GetComponent<BiomesEtatsManager>()._infos.Add("biome", quelBiome + 1); // Envoi le biome à BiomesEtatsManager
                    unCube.GetComponent<BiomesEtatsManager>()._infos.Add("variant", quelVariant + 1); // Envoi le variant à BiomesEtatsManager
                    unCube.GetComponent<BiomesEtatsManager>()._infos.Add("perso", _prefabPerso); // Envoi le prefab du perso à BiomesEtatsManager
                    unCube.GetComponent<BiomesEtatsManager>()._infos.Add("conteneurObjets", _conteneurObjets); // Envoi le conteneur des objets à BiomesEtatsManager
                    unCube.GetComponent<BiomesEtatsManager>()._infos.Add("ile", this); // Envoi l'ile à BiomesEtatsManager
                    _biomesListe.Add(unCube.GetComponent<BiomesEtatsManager>()); // Ajoute le cube à la liste des biomes de l'ile
                }
            }
        }
        texture.Apply(); // Applique et enregistre les changements
        _textureRenderer.sharedMaterial.mainTexture = texture; // Change la texture principale pour la texture créée
    }

    /// <summary>
    /// Simplifie une valeur pour 
    /// obtenir un float entre 0 et 1
    /// </summary>
    /// <param name="value">Valeur à simplifier</param>
    /// <returns>Retourne un float entre 0 et 1</returns>
    float Sigmoid(float value)
    {
        return 1 / (1 + Mathf.Exp(-_k * (value - _c))); // Simplifie la valeur entre 0 et 1
    }

    /// <summary>
    /// Choisi un cube parmis ceux diponibles
    /// Retire le cube de la liste par la suite
    /// <summary>
    ///<returns>Un vecteur 3 qui contient la position du cube choisi</returns>
    Vector3 ChoisirCube()
    {
        // Sélectionner une position aléatoire et la supprimer ensuite:
        int randomIndex = Random.Range(0, _posCubesVisibles.Count);
        // Prend en référence la position:
        Vector3 positionAleatoire = _posCubesVisibles[randomIndex] - new Vector3(_ileLargeur / 2, 0, _ileProfondeur / 2);
        // Supprimer la position de la liste:
        _posCubesVisibles.RemoveAt(randomIndex);
        return positionAleatoire; // Retourne la position du cube
    }

    /// <summary>
    /// Fonction pour placer les ennemis
    /// </summary>
    void PlacerEnnemi(int nbEnnemis, GameObject lac)
    {
        // Récupérer la position centrale du lac
        Vector3 centreLac = lac.transform.position;

        for (int i = 0; i < nbEnnemis; i++)
        {
            // Générer une position aléatoire dans un rayon de 5 unités autour du lac
            Vector2 positionAléatoireEnnemi = Random.insideUnitCircle * 10f;
            Vector3 positionEnnemi = new Vector3(
                centreLac.x + positionAléatoireEnnemi.x,
                centreLac.y,
                centreLac.z + positionAléatoireEnnemi.y
            );

            Vector2 positionAléatoireCible = Random.insideUnitCircle * 10f;
            Vector3 positionCible = new Vector3(
                centreLac.x + positionAléatoireCible.x,
                centreLac.y,
                centreLac.z + positionAléatoireCible.y
            );

            // Instancier l'ennemi à la position calculée
            GameObject unEnnemi = Instantiate(
                _prefabEnnemi,
                positionEnnemi + Vector3.up * 3, // Légèrement au-dessus pour éviter les collisions initiales
                Quaternion.identity,
                _conteneurEnnemis
            );

            // Ajouter des informations pertinentes à l'ennemi
            unEnnemi.GetComponent<EnnemisEtatsManager>().infos.Add("maison", positionEnnemi);
            unEnnemi.GetComponent<EnnemisEtatsManager>().infos.Add("cible", positionCible);
            unEnnemi.GetComponent<EnnemisEtatsManager>().infos.Add("ile", this);
            unEnnemi.GetComponent<EnnemisEtatsManager>().infos.Add("perso", _prefabPerso.transform);

            // Ajouter l'ennemi à la liste des ennemis
            _ennemisListe.Add(unEnnemi);
        }
    }

    /// <summary>
    /// Fonction pour placer les NPCs
    /// </summary>
    /// <param name="lac"> Le prefab du lac de l'ile </param>
    void PlacerNPC(GameObject lac)
    {
        //Pour chaque NPC:
        for (int i = 0; i < _nbNPC; i++)
        {
            Vector3 pos = ChoisirCube(); //Choisir une position valide et aléatoire
            //Instantier un NPC:
            GameObject unNPC = Instantiate(_prefabNPC, pos + Vector3.up, Quaternion.identity, _conteneurNPC);
            unNPC.GetComponent<NPCEtatsManager>().infos.Add("ile", this); // Envoi l'ile à NPCEtatsManager
            unNPC.GetComponent<NPCEtatsManager>().infos.Add("perso", _prefabPerso.transform); // Envoi le prefab du perso à NPCEtatsManager
            unNPC.GetComponent<NPCEtatsManager>().infos.Add("ennemiListe", _ennemisListe); // Envoi la liste des ennemis à NPCEtatsManager
            unNPC.GetComponent<NPCEtatsManager>().infos.Add("ennemiCount", _ennemisListe.Count - 1); // Envoi le nombre d'ennemis à NPCEtatsManager
            unNPC.GetComponent<NPCEtatsManager>().infos.Add("lac", lac); // Envoi le prefab du lac à NPCEtatsManager

        }
    }
}
