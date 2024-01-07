using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using DelotransApp.DataAccess;
using DelotransApp.Model;
using System.Net;
using System.Windows.Forms;

namespace DelotransApp.Controller
{
    internal class UserController
    {
        private readonly DataAccessAll _dataAccess;

        public UserController()
        {
            _dataAccess = new DataAccessAll();
        }

        // Insérer un utilisateur
        public string AddUser(UserModel user)
        {
            try
            {
                if (SendEmailConfirmation(user.EmailUser, user.EmailVerifyCodeUser))
                {
                    _dataAccess.InsertUser(user);
                    return "succès";
                }
                return "échec";


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cet e-mail est déjà utilisé."))
                {
                    return "L'e-mail fourni est déjà utilisé par un autre utilisateur.";
                    //MessageBox.Show("L'e-mail fourni est déjà utilisé par un autre utilisateur.", "Erreur d'Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex.Message.Contains("Ce numéro de téléphone est déjà utilisé."))
                {
                    return "Le numéro de téléphone fourni est déjà utilisé par un autre utilisateur.";
                    //MessageBox.Show("Le numéro de téléphone fourni est déjà utilisé par un autre utilisateur.", "Erreur de Numéro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    return "Une erreur s'est produite lors de la création de l'utilisateur.";
                    //MessageBox.Show("Une erreur s'est produite lors de la création de l'utilisateur.", "Erreur de Connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        // Mettre à jour un utilisateur
        public bool UpdateUser(UserModel user)
        {
            try
            {
                _dataAccess.UpdateUser(user);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        //Activer un utilisateur
        public bool ActivateUSer(UserModel user)
        {
            try
            {
                _dataAccess.ActivateUser(user);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Changer de mot de passe
        public bool UpdateUserPassword(string email, string password)
        {
            try
            {
                if (_dataAccess.UpdateUserPassword(email, password))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        // Check if user exist by email
        public bool CheckIfUserExistByEmail(string email)
        {
            try
            {

                var user = _dataAccess.GetUserByEmail(email);
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Renvoyer le code de validation

        public bool ResendActivation(UserModel user)
        {
            try
            {
                SendEmailConfirmation(user.EmailUser, user.EmailVerifyCodeUser);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Supprimer un utilisateur par ID
        public bool RemoveUser(int userId)
        {
            return _dataAccess.DeleteUser(userId);
        }

        // Récupérer un utilisateur par ID
        public UserModel GetUserById(int userId)
        {
            return _dataAccess.GetUserById(userId);
        }

        //Récupérer un utilisateur par Email
        public UserModel GetUserByEmail(string email, string motdepasse)
        {
            var user = _dataAccess.GetUserByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(motdepasse, user.MotdepasseUser))
            {
                return null;
            }
            else
            {
                return user;

            }
        }

        // Autres méthodes de contrôleur si nécessaire
        public bool SendEmailConfirmation(string userEmail, int code)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.freesmtpservers.com"); // Remplace avec ton serveur SMTP

                mail.From = new MailAddress("basirukingrahman98@gmail.com");
                mail.To.Add(userEmail);
                mail.Subject = "Confirmation d'inscription";
                mail.IsBodyHtml = true;
                mail.Body = @"
                            <html>
                            <head>
                                <style>
                                    body {
                                        font-family: Arial, sans-serif;
                                        background-color: #f4f4f4;
                                        color: #333;
                                    }
                                    .container {
                                        width: 80%;
                                        margin: auto;
                                        padding: 20px;
                                        background-color: #fff;
                                        border-radius: 5px;
                                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                    }
                                    h1 {
                                        color: #007bff;
                                    }
                                    h2 {
                                        color: #2459b2;
                                    }
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <h1>Bienvenue chez DelotransApp!</h1>
                                    <p>Merci pour votre inscription.</p>
                                    <p>Utilisez le code suivant pour valider votre compte :</p>
                                    <h2>" + code + @"</h2>
                                </div>
                            </body>
                            </html>
                            ";


                SmtpServer.Port = 25; // Port SMTP (peut varier selon le fournisseur)
                //SmtpServer.Credentials = new NetworkCredential("basirukingrahman98@gmail.com", "illegalking1998"); // Remplace avec ton adresse e-mail et mot de passe
                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception)
            {
                // Gérer les exceptions (par exemple, enregistrer dans un fichier journal ou afficher un message à l'utilisateur)
                return false;
                //MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("L'envoie du mal à échoué, vérifier la connexion à internet","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
