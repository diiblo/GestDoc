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
using DelotransApp.Model;
using ReaLTaiizor.Forms;
using RestSharp.Validation;

namespace DelotransApp.View
{
    public partial class RegisterValidation : KryptonForm
    {
        private readonly UserModel _user;
        private readonly Login _parentForm;
        private readonly UserController _controllerUser;
        private bool _validate = true;
        public RegisterValidation(Login parentForm , UserModel user)
        {
            InitializeComponent();
            _user = user;
            _parentForm = parentForm;
            _controllerUser = new UserController();
            kryptonTextBoxCode.KeyPress += KryptonTextBoxCode_KeyPress;
        }

        private void kryptonButtonValider_Click(object sender, EventArgs e)
        {
            TextBoxIsEmpty(kryptonTextBoxCode);
            EstCodeValide(kryptonTextBoxCode);
            if (_validate)
            {
                if (_user.EmailVerifyCodeUser == int.Parse(kryptonTextBoxCode.Text) && _controllerUser.ActivateUSer(_user))
                {
                    MessageAlert("Compte activé avec succès", false);
                    _parentForm.Show();
                    this.Close();
                }
                else
                {
                    MessageAlert("Le code ne correspond pas", true);
                }


            }
            else
            {
                MessageAlert("Veuillez valider les champs en rouge",true);
            }
        }

        private void kryptonLabelLoginPage_Click(object sender, EventArgs e)
        {
            _parentForm.Show();
            this.Close();
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

    }
}
