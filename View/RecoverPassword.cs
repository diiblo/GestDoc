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

namespace DelotransApp.View
{
    public partial class RecoverPassword : KryptonForm
    {
        private readonly Login _parentForm;
        public RecoverPassword(Login parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void kryptonLabelLoginPage_Click(object sender, EventArgs e)
        {
            _parentForm.Show();
            this.Close();
        }

        private void kryptonButtonValider_Click(object sender, EventArgs e)
        {

        }
    }
}
