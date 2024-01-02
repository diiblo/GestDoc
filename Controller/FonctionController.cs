using DelotransApp.Model;
using DelotransApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelotransApp.Controller
{
    internal class FonctionController
    {
        private readonly DataAccessAll _dataAccess;

        public FonctionController()
        {
            _dataAccess = new DataAccessAll();
        }

        // Méthode pour ajouter une nouvelle fonction
        public void AddFonction(FonctionModel fonction)
        {
            try
            {
                _dataAccess.InsertFonction(fonction);
            }
            catch (Exception ex)
            {
                // Gestion de l'exception, par exemple : 
                Console.WriteLine($"Erreur lors de l'ajout de la fonction : {ex.Message}");
            }
        }

        // Méthode pour mettre à jour une fonction existante
        public void UpdateFonction(FonctionModel fonction)
        {
            try
            {
                _dataAccess.UpdateFonction(fonction);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour de la fonction : {ex.Message}");
            }
        }

        // Méthode pour supprimer une fonction
        public void DeleteFonction(int idFonction)
        {
            try
            {
                _dataAccess.DeleteFonction(idFonction);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression de la fonction : {ex.Message}");
            }
        }

        // Méthode pour récupérer toutes les fonctions
        public List<FonctionModel> GetAllFonctions()
        {
            try
            {
                return _dataAccess.GetAllFonctions();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des fonctions : {ex.Message}");
                return new List<FonctionModel>();
            }
        }
    }
}
