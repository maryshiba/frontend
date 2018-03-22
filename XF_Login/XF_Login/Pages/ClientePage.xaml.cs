using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF_Login.ViewModels;

namespace XF_Login.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientePage : ContentPage
    {
        public ClientePage()
        {
            var vm = new ClienteViewModel();
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}