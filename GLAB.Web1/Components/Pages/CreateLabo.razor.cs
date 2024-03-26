using Glab.App.Laboratoires;
using Microsoft.AspNetCore.Components;
namespace GLAB.Web1.Components.Pages
{
    public partial class CreateLabo
    {
        [Inject]
        private ILabService labService { get; set; }
        private CreateLaboModel newlabo;

        private async Task save() { }
    
    }
}


