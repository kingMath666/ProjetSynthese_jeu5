using UnityEngine;

public abstract class EnnemisEtatsBase
{
    public abstract void InitEtat(EnnemisEtatsManager ennemi);
    public abstract void ExitEtat(EnnemisEtatsManager ennemi);
    public abstract void TriggerEnterEtat(EnnemisEtatsManager ennemi, Collider other); //Fonction de 
}
