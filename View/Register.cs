using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using DelotransApp.Model;
using DelotransApp.Controller;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
namespace DelotransApp.View
{
    public partial class Register : KryptonForm
    {
        private readonly FonctionController _controllerFonction;
        private readonly UserController _controllerUser;
        private readonly Login _parentForm;
        private bool _validate = true;
        public Register(Login parentForm)
        {
            InitializeComponent();
            _controllerFonction = new FonctionController();
            _controllerUser = new UserController();
            LoadFunctionsIntoComboBox();
            _parentForm = parentForm;
        }
        private void labelDejaUnCompte_Click(object sender, EventArgs e)
        {
            _parentForm.Show();
            this.Close();
        }

        private void kryptonButtonRegister_Click(object sender, EventArgs e)
        {
            
            //Test si les Texbox sont vides
            TextBoxIsEmpty(kryptonTextBoxNom);
            TextBoxIsEmpty(kryptonTextBoxPrenom);
            TextBoxIsEmpty(kryptonTextBoxEmail);
            TextBoxIsEmpty(kryptonTextBoxPhone);
            ComboBoxIsEmpty();

            //Test si l'email est valide
            EstEmailValide(kryptonTextBoxEmail);

            //Test si le numéro est correcte
            EstNumeroValide(kryptonTextBoxPhone);

            //Test si les mots de passe sont similaires
            SiMotDePasseSimilaire(kryptonTextBoxMDP, kryptonTextBoxRepeatMDP);

            if (_validate)
            {
                Random random = new Random();

                string nom = kryptonTextBoxNom.Text;
                string prenom = kryptonTextBoxPrenom.Text;
                string email = kryptonTextBoxEmail.Text;
                int phone = int.Parse(kryptonTextBoxPhone.Text);
                string motdepasse = kryptonTextBoxMDP.Text;
                int idFonction = SelectedItem();
                int emailVerifyCodeUser = random.Next(1000, 10000);

                UserModel user = new UserModel
                {
                    IdType = 2,
                    IdFonction = idFonction,
                    NomUser = nom,
                    PrenomUser = prenom,
                    EmailUser = email,
                    PhoneUser = phone,
                    MotdepasseUser = BCrypt.Net.BCrypt.HashPassword(motdepasse),
                    EmailVerifyCodeUser = emailVerifyCodeUser,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                string response = _controllerUser.AddUser(user);

                if (response == "succès")
                {

                    MessageAlert("Compte créé avec succès veillez saisir le code envoyé à votre adresse email", false);
                    Form registrationValidation = new RegisterValidation(_parentForm, user);
                    registrationValidation.Show();
                    this.Close();
                }
                else
                {
                    MessageAlert(response, true);
                }

                
            }
            else
            {
                MessageAlert("Veillez remplir les champs en rouge", true);
            }

        }
        private void LoadFunctionsIntoComboBox()
        {
            try
            {
                var functions = _controllerFonction.GetAllFonctions();

                // Nettoyer le ComboBox avant de charger les nouvelles données
                kryptonComboBoxFonction.Items.Clear();

                //Gère l'affichage des éléments en KeyValuePair
                kryptonComboBoxFonction.DisplayMember = "Value";  // Affiche le nom
                kryptonComboBoxFonction.ValueMember = "Key"; // Stocke l'ID

                // Ajouter les fonctions au ComboBox
                foreach (var function in functions)
                {
                    //kryptonComboBoxFonction.Items.Add(function.NomFonction);
                    kryptonComboBoxFonction.Items.Add(new KeyValuePair<int, string>(function.IdFonction, function.NomFonction));
                }

                // Sélectionnez la première fonction par défaut (si vous le souhaitez)
                if (kryptonComboBoxFonction.Items.Count > 0)
                {
                    kryptonComboBoxFonction.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des fonctions : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int SelectedItem()
        {
                var fonctionSelectionnee = (KeyValuePair<int, string>)kryptonComboBoxFonction.SelectedItem;
                int idFonctionSelectionnee = fonctionSelectionnee.Key;
                return idFonctionSelectionnee;         
        }

        private bool TextBoxIsEmpty(KryptonTextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                textBox.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);
                _validate = false;
                return true;
            }
            else
            {
                textBox.StateCommon.Border.Color1 = Color.FromArgb(224, 224, 224);
                textBox.StateCommon.Border.Color2 = Color.FromArgb(224, 224, 224);

                _validate = true;
                return false;
            }

        }

        private void ComboBoxIsEmpty()
        {
            if (kryptonComboBoxFonction.SelectedItem == null)
            {
                kryptonComboBoxFonction.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(231, 41, 58);
                kryptonComboBoxFonction.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(231, 41, 58);

                _validate = false;
            }
            else
            {
                kryptonComboBoxFonction.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(224, 224, 224);
                kryptonComboBoxFonction.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(224, 224, 224);

                _validate = true;

            }
        }
        private void MessageAlert(string message, bool type)
        {
            if (type)
            {
                MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(message, "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EstEmailValide(KryptonTextBox email)
        {
            // Expression régulière pour vérifier une adresse e-mail valide
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(email.Text.Trim()))
            {
                email.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                email.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);
                _validate = false;
                MessageAlert("Veillez saisir une adress mail valide", true);
            }
            else
            {
                email.StateCommon.Border.Color1 = Color.FromArgb(224, 224, 224);
                email.StateCommon.Border.Color1 = Color.FromArgb(224, 224, 224);

                _validate = true;
            }
        }

        private void SiMotDePasseSimilaire(KryptonTextBox motdepasse, KryptonTextBox repeat)
        {
            string MotdePasse = motdepasse.Text.Trim();
            string Repeat = repeat.Text.Trim();

            if (!TextBoxIsEmpty(motdepasse) && !TextBoxIsEmpty(repeat))
            {
                motdepasse.StateCommon.Border.Color1 = Color.FromArgb(224, 224, 224);
                motdepasse.StateCommon.Border.Color1 = Color.FromArgb(224, 224, 224);

                repeat.StateCommon.Border.Color1 = Color.FromArgb(224, 224, 224);
                repeat.StateCommon.Border.Color1 = Color.FromArgb(224, 224, 224);

                _validate = true;
            }
            else
            {
                motdepasse.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                motdepasse.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                repeat.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                repeat.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                _validate = false;

                MessageAlert("Mot de passe différent", true);
            }

        }

        private void EstNumeroValide(KryptonTextBox Phone)
        {
            try
            {
                int phone = int.Parse(Phone.Text);

                // Vérifier que le numéro de téléphone commence par 6 ou 2
                string phoneString = phone.ToString();
                if (phoneString.StartsWith("6") || phoneString.StartsWith("2"))
                {
                    Phone.StateCommon.Border.Color1 = Color.FromArgb(224, 224, 224);
                    Phone.StateCommon.Border.Color2 = Color.FromArgb(224, 224, 224);

                    _validate = true;
                }
                else
                {
                    Phone.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                    Phone.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                    _validate = false;
                    MessageAlert("Le numéro de téléphone doit commencer par 6 ou 2.", true);
                }
            }
            catch (FormatException)
            {
                Phone.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                Phone.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                _validate = false;
                MessageAlert("Le numéro de téléphone n'est pas valide. Assurez-vous d'entrer un nombre entier.",true);
            }
            catch (OverflowException)
            {
                Phone.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                Phone.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                _validate = false;
                MessageAlert("Le numéro de téléphone est trop grand ou trop petit.",true);
            }
            catch (Exception ex)
            {
                Phone.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                Phone.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                _validate = false;
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        private void Register_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _parentForm.Show();
            }
            
        }
    }
}
