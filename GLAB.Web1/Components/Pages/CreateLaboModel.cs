using System.ComponentModel.DataAnnotations;

namespace GLAB.Web1.Components.Pages
{
    public class CreateLaboModel
    {
        [Required(ErrorMessage = "nom obligatiore" )]
        public string Name { get; set; }
        [Required(ErrorMessage = "Adresse obligatiore")]
        public string Adresse { get; set; }
        [Required(ErrorMessage = "University obligatiore")] 
        public string University { get; set; }
        [Required(ErrorMessage = "Faculty obligatiore")]
        public string Faculty { get; set; }
        [Required(ErrorMessage = "Departement obligatiore")]
        public string Departement { get; set; }
        [Required(ErrorMessage = "Acronyme obligatiore")]
            public string Acronyme { get; set; }
        [Required(ErrorMessage = "PhoneNumber obligatiore")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email obligatiore")] 
        public string Email { get; set; }
        [Required(ErrorMessage = "NumAgrement obligatiore")] 
        public string NumAgrement { get; set; }
        [Required(ErrorMessage = "Logo obligatiore")] 
        public byte[] Logo { get; set; } = new byte[] { };

     
        public string WebSite { get; set; }
    }
}


