using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentFactory.Krypton.Toolkit;
using System.Windows.Forms;
using DelotransApp.Controller;
using RestSharp.Validation;

namespace DelotransApp.View
{
    public partial class ValidatePassword : KryptonForm
    {
        private readonly UserController _userController;
        private readonly Login _parentForm;
        private readonly string _email;
        private bool _validate = true;
        public ValidatePassword(Login parent ,string email)
        {
            InitializeComponent();
            _email = email;
            _userController = new UserController();
            _parentForm = parent;
            TextBoxCode.KeyPress += KryptonTextBoxCode_KeyPress;
        }



        private void KryptonTextBoxCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Vérifiez si le caractère saisi est un chiffre ou la touche de contrôle (par exemple, pour permettre le copier-coller)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Si le caractère n'est pas un chiffre, supprimez-le
                e.Handled = true;
            }
        }

        private void BtnValider_Click(object sender, EventArgs e)
        {
            TextBoxIsEmpty(TextBoxCode);
            TextBoxIsEmpty(TextBoxPassword);
            TextBoxIsEmpty(TextBoxRepeatPassword);

            EstCodeValide(TextBoxCode);
            SiMotDePasseSimilaire(TextBoxPassword, TextBoxRepeatPassword);

            if (_validate)
            {
                string motdepasse = BCrypt.Net.BCrypt.HashPassword(TextBoxPassword.Text.Trim());

                if (_userController.UpdateUserPassword(_email, motdepasse))
                {
                    MessageAlert("Mot de passe modifié avec succès", false);
                    this.Close();
                }
                else
                {
                    MessageAlert("Une erreur est survenue, sans doute un problème de connexion", true);
                    this.Close();
                }
            }
            else
            {
                MessageAlert("Veillez remplir les champs en rouge", true);
            }

        }

        private void ValidatePassword_FormClosing(object sender, FormClosingEventArgs e)
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

        private void EstCodeValide(KryptonTextBox Code)
        {
            try
            {
                int code = int.Parse(Code.Text);
                _validate = true;
            }
            catch (FormatException)
            {
                Code.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                Code.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                _validate = false;
                MessageAlert("Le code n'est pas valide. Assurez-vous d'entrer un nombre entier.", true);
            }
            catch (OverflowException)
            {
                Code.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                Code.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                _validate = false;
                MessageAlert("Le code est trop grand ou trop petit.", true);
            }
            catch (Exception ex)
            {
                Code.StateCommon.Border.Color1 = Color.FromArgb(231, 41, 58);
                Code.StateCommon.Border.Color2 = Color.FromArgb(231, 41, 58);

                _validate = false;
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
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

        
    }
}
