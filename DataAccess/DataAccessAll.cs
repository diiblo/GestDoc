using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Net;
using DelotransApp.Model;

namespace DelotransApp.DataAccess
{
    internal class DataAccessAll
    {
        private readonly string connectionString = "Server=localhost;Database=delotrans;Uid=root;Pwd=root;"; // Remplacez par votre chaîne de connexion

        // Méthode pour insérer une nouvelle fonction
        public void InsertFonction(FonctionModel fonction)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO VotreTable (NomFonction, CreatedAt, UpdatedAt) VALUES (@NomFonction, @CreatedAt, @UpdatedAt)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NomFonction", fonction.NomFonction);
                    cmd.Parameters.AddWithValue("@CreatedAt", fonction.CreatedAt);
                    cmd.Parameters.AddWithValue("@UpdatedAt", fonction.UpdatedAt);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Gérer l'exception ou re-lancer pour un traitement ultérieur
                    throw new Exception("Erreur lors de l'insertion de la fonction.", ex);
                }
            }
        }

        // Méthode pour mettre à jour une fonction existante
        public void UpdateFonction(FonctionModel fonction)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE VotreTable SET NomFonction = @NomFonction, UpdatedAt = @UpdatedAt WHERE IdFonction = @IdFonction";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NomFonction", fonction.NomFonction);
                    cmd.Parameters.AddWithValue("@UpdatedAt", fonction.UpdatedAt);
                    cmd.Parameters.AddWithValue("@IdFonction", fonction.IdFonction);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Erreur lors de la mise à jour de la fonction.", ex);
                }
            }
        }

        // Méthode pour supprimer une fonction
        public void DeleteFonction(int idFonction)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM VotreTable WHERE IdFonction = @IdFonction";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdFonction", idFonction);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Erreur lors de la suppression de la fonction.", ex);
                }
            }
        }

        // Méthode pour récupérer toutes les fonctions
        public List<FonctionModel> GetAllFonctions()
        {
            List<FonctionModel> fonctions = new List<FonctionModel>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM fonction";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fonctions.Add(new FonctionModel
                            {
                                IdFonction = Convert.ToInt32(reader["idFonction"]),
                                NomFonction = reader["nomFonction"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["updatedAt"])
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Erreur lors de la récupération des fonctions.", ex);
                }
            }
            return fonctions;
        }

        // Insérer un utilisateur
        public void InsertUser(UserModel user)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                // Ton code pour insérer un utilisateur dans la base de données
                conn.Open();
                string query = @"INSERT INTO utilisateur (
                                                            idType,
                                                            idFonction,
                                                            nomUser,
                                                            prenomUser,
                                                            emailUser,
                                                            phoneUser,
                                                            motdepasseUser,
                                                            emailVerifyCodeUser,
                                                            createdAt,
                                                            updatedAt
                                                        ) VALUES (
                                                                @IdType,
                                                                @IdFonction,
                                                                @NomUser,
                                                                @PrenomUser,
                                                                @EmailUser,
                                                                @PhoneUser,
                                                                @MotdepasseUser,
                                                                @EmailVerifyCodeUser,
                                                                @CreatedAt,
                                                                @UpdatedAt
                                                            )";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@IdType", user.IdType);
                cmd.Parameters.AddWithValue("@IdFonction", user.IdFonction);
                cmd.Parameters.AddWithValue("@NomUser", user.NomUser);
                cmd.Parameters.AddWithValue("@PrenomUser", user.PrenomUser);
                cmd.Parameters.AddWithValue("@EmailUser", user.EmailUser);
                cmd.Parameters.AddWithValue("@PhoneUser", user.PhoneUser);
                cmd.Parameters.AddWithValue("@MotdepasseUser", user.MotdepasseUser);
                cmd.Parameters.AddWithValue("@EmailVerifyCodeUser", user.EmailVerifyCodeUser);
                cmd.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
                cmd.Parameters.AddWithValue("@UpdatedAt", user.UpdatedAt);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1062) // Code d'erreur pour violation de contrainte d'unicité
                    {
                        if (ex.Message.Contains("emailUser"))
                        {
                            throw new Exception("Cet e-mail est déjà utilisé.");
                        }
                        else if (ex.Message.Contains("phoneUser"))
                        {
                            throw new Exception("Ce numéro de téléphone est déjà utilisé.");
                        }
                    }
                    else
                    {
                        // Gestion d'autres exceptions ou erreurs
                        throw new Exception("Une erreur c'est produite, vérifier votre connexion.");
                    }
                }
            }
        }

        // Mettre à jour un utilisateur
        public void UpdateUser(UserModel user)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {       
                try
                {
                    conn.Open();
                    string query = @"UPDATE Utilisateur 
                                    SET idType=@IdType,
                                        idFonction=@IdFonction,
                                        nomUser=@NomUser,
                                        emailUser=@EmailUser,
                                        prenomUser=@PrenomUser,
                                        phoneUser=@PhoneUser,
                                        motdepasseUser=@MotdepasseUser,
                                        emailVerifyCodeUser=@EmailVerifyCodeUser,
                                        createdAt=@CreatedAt,
                                        updatedAt=@UpdatedAt
                                  WHERE emailUser=@EmailUser";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@idType", user.IdType);
                    cmd.Parameters.AddWithValue("@idFonction", user.IdFonction);
                    cmd.Parameters.AddWithValue("@NomUser", user.NomUser);
                    cmd.Parameters.AddWithValue("@PrenomUser", user.PrenomUser);
                    cmd.Parameters.AddWithValue("@EmailUser", user.EmailUser);
                    cmd.Parameters.AddWithValue("@PhoneUser", user.PhoneUser);
                    cmd.Parameters.AddWithValue("@MotdepasseUser", user.MotdepasseUser);
                    cmd.Parameters.AddWithValue("@EmailVerifyCodeUser", user.EmailVerifyCodeUser);
                    cmd.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
                    cmd.Parameters.AddWithValue("@UpdatedAt", user.UpdatedAt);
                    int result = cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Erreur lors de la mise à jour de l'utilisateur.", ex);
                }
            }
        }

        // Activation d'un utilisateur
        public void ActivateUser(UserModel user)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Utilisateur 
                                    SET emailVerifyCodeUser=@EmailVerifyCodeUser,
                                        verifyUser=@VerifyUser,
                                        updatedAt=@UpdatedAt
                                  WHERE emailUser=@EmailUser";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@EmailVerifyCodeUser", 0);
                    cmd.Parameters.AddWithValue("@VerifyUser", 1);
                    cmd.Parameters.AddWithValue("@EmailUser", user.EmailUser);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Erreur lors de l'activation.", ex);
                }
            }
        }

        public bool UpdateUserPassword(string email, string motdepasse)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Utilisateur 
                                    SET emailVerifyCodeUser=@EmailVerifyCodeUser,
                                        motdepasseUser=@MotdepasseUSer,
                                        updatedAt=@UpdatedAt
                                  WHERE emailUser=@EmailUser";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@EmailVerifyCodeUser", 0);
                    cmd.Parameters.AddWithValue("@MotdepasseUser", motdepasse);
                    cmd.Parameters.AddWithValue("@EmailUser", email);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Erreur lors du changement de mot de passe.", ex);
                }
            }
        }



        // Supprimer un utilisateur
        public bool DeleteUser(int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Utilisateur WHERE idUser=@IdUser";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdUser", userId);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        // Récupérer un utilisateur par ID (ajoutez d'autres méthodes si nécessaire)
        public UserModel GetUserById(int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Utilisateur WHERE idUser=@IdUser";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdUser", userId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserModel
                        {
                            IdUser = Convert.ToInt32(reader["idUser"]),
                            IdType = Convert.ToInt32(reader["idType"]),
                            IdFonction = Convert.ToInt32(reader["idFonction"]),
                            NomUser = reader["nomUser"].ToString(),
                            // ... Remplir les autres propriétés
                        };
                    }
                    return null;
                }
            }
        }

        // Récupérer un utilisateur par Email (ajoutez d'autres méthodes si nécessaire)
        public UserModel GetUserByEmail(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Utilisateur WHERE emailUser=@EmailUser";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmailUser", email);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserModel
                        {
                            IdUser = Convert.ToInt32(reader["idUser"]),
                            IdType = Convert.ToInt32(reader["idType"]),
                            IdFonction = Convert.ToInt32(reader["idFonction"]),
                            NomUser = reader["nomUser"].ToString(),
                            PrenomUser = reader["prenomUser"].ToString(),
                            EmailUser = reader["emailUser"].ToString(),
                            PhoneUser = Convert.ToInt32(reader["phoneUser"]),
                            MotdepasseUser = reader["motdepasseUser"].ToString(),
                            VerifyUser = Convert.ToInt32(reader["verifyUser"]),
                            EnableUser = Convert.ToInt32(reader["enableUser"]),
                            // ... Remplir les autres propriétés
                        };
                    }
                    return null;
                }
            }
        }
    }
}
