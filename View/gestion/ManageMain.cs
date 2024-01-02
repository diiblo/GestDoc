using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using DelotransApp.Model;
using DelotransApp.View.gestion.profil;

namespace DelotransApp.View.gestion
{
    public partial class ManageMain : KryptonForm
    {
        private readonly UserModel _user;
        private readonly Login _parentForm;
        public ManageMain(Login parentForm, UserModel user)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _user = user;
            //openChildForm(new Dashboard());

        }

        //private void hideShowSubMenu()
        //{
        //    if (panelFolderSubmenu.Visible == false)
        //    {
        //        panelFolderSubmenu.Visible = true;
        //    }
        //    else
        //    {
        //        panelFolderSubmenu.Visible = false;
        //    }

        //}
        //private Form activeForm = null;
        //private void openChildForm(Form childForm)
        //{
        //    if (activeForm != null)
        //    {
        //        activeForm.Close();
        //        activeForm = childForm;
        //        childForm.TopLevel = false;
        //        childForm.FormBorderStyle = FormBorderStyle.None;
        //        childForm.Dock = DockStyle.Fill;
        //        panelChildForm.Controls.Add(childForm);
        //        panelChildForm.Tag = childForm;
        //        childForm.BringToFront();
        //        childForm.Show();

        //    }
        //    else
        //    {
        //        activeForm = childForm;
        //        childForm.TopLevel = false;
        //        childForm.FormBorderStyle = FormBorderStyle.None;
        //        childForm.Dock = DockStyle.Fill;
        //        panelChildForm.Controls.Add(childForm);
        //        panelChildForm.Tag = childForm;
        //        childForm.BringToFront();
        //        childForm.Show();
        //    }
        //}

    }
}
