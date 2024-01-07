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
using ComponentFactory.Krypton.Toolkit;
using DelotransApp.Controller;
using RestSharp.Validation;

namespace DelotransApp.View
{
    public partial class RecoverPassword : KryptonForm
    {
        private readonly Login _parentForm;
        private readonly UserController _userController;
        private bool _validate = true;
        public RecoverPassword(Login parentForm)
        {
            InitializeComponent();
            _userController = new UserController();
            _parentForm = parentForm;
        }

        private void LabelLoginPage_Click(object sender, EventArgs e)
        {
            _parentForm.Show();
            this.Close();
        }

        private void BtnValider_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            TextBoxIsEmpty(TextBoxEmail);
            EstEmailValide(TextBoxEmail);

            string email = TextBoxEmail.Text.Trim();
            int emailVerifyCodeUser = random.Next(1000, 10000);

            if (_validate)
            {
                if (_userController.CheckIfUserExistByEmail(email))
                {
                    if (_userController.SendEmailConfirmation(email, emailVerifyCodeUser))
                    {
                        Form validatePassword = new ValidatePassword(_parentForm, email);
                        validatePassword.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageAlert("Une erreur c'est produite, verifier votre connexion ou recommencer",true);
                    }
                }
                else
                {
                    MessageAlert("Votre Email n'existe pas dans le système, " +
                        "vérifier ne pas avoir fait d'erreur ou créer un compte", true);
                }
            }
            else
            {
                MessageAlert("Veuillez valider les champs en rouge", true);
            }
        }

        private void RecoverPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _parentForm.Show();
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
