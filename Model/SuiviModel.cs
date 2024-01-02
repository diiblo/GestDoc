using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelotransApp.Model
{
    internal class SuiviModel
    {
        public int IdSuivi { get; set; }
        public int IdDossier { get; set; }
        public string ActionSuivi { get; set; }
        public DateTime DateActionSuivi { get; set; }
        public int EmetteurSuivi { get; set; }
        public int DestinataireSuivi { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
