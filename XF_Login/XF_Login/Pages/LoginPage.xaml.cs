using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF_Login.ViewModels;

namespace XF_Login.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            vm.ExibirAvisoDeLoginInvalido += () => DisplayAlert("Erro", "Login Inválido, tente novamente", "OK");
            InitializeComponent();

            Email.Completed += (object sender, EventArgs e) =>
            {
                Senha.Focus();
            };

            Senha.Completed += (object sender, EventArgs e) =>
            {
                vm.SubmitCommand.Execute(null);
            };

        }
    }
}