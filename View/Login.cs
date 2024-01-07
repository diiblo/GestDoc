using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ComponentFactory.Krypton.Toolkit;
using DelotransApp.Controller;
using DelotransApp.Model;
using DelotransApp.View;
using DelotransApp.View.gestion;
using RestSharp.Validation;

namespace DelotransApp
{
    public partial class Login : KryptonForm
    {
        private readonly UserController _controllerUser;
        private bool _validate = true;
        public Login()
        {
            InitializeComponent();
            _controllerUser = new UserController();
        }

        private void BtnConnecter_Click(object sender, EventArgs e)
        {
            TextBoxIsEmpty(TextBoxEmail);
            TextBoxIsEmpty(TextBoxPassword);

            EstEmailValide(TextBoxEmail);

            if (_validate)
            {
                string email = TextBoxEmail.Text.Trim();
                string motdepasse = TextBoxPassword.Text.Trim();
                UserModel user = _controllerUser.GetUserByEmail(email, motdepasse);
                if (user == null)
                {
                    MessageAlert("Vérifier votre adresse email ou votre mot de passe", true);
                }
                else
                {
                    if (user.VerifyUser != 1)
                    {
                        MessageAlert("Veillez valider votre compte en entrant le code que vous avez reçu", true);
                        RegisterValidation registerValidation = new RegisterValidation(this, user);
                        registerValidation.Show();
                        this.Hide();
                    }
                    else if (user.EnableUser != 1)
                    {
                        MessageAlert("Votre compte à été désactivé veillez contacté l'admin", true);
                    }
                    else
                    {
                        ManageMain manage = new ManageMain(this, user);
                        MessageAlert($"Bienvenue {user.NomUser}", false);
                        manage.Show();
                        this.Hide();
                    }

                }
            }
        }

        private void BtnInscrire_Click(object sender, EventArgs e)
        {
            Register register = new Register(this);
            register.Show();
            this.Hide();
        }

        private void LabelForget_Click(object sender, EventArgs e)
        {
            RecoverPassword recoverPassword = new RecoverPassword(this);
            recoverPassword.Show();
            this.Hide();
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


    }
}
