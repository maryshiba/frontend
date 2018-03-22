using Android.Widget;
using System;
using System.ComponentModel;
using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XF_Login.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        //public Action DisplayInvalidLoginPrompt;
        public Action ExibirAvisoDeLoginInvalido; 

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string senha;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Senha"));
            }
        }

        public ICommand SubmitCommand { protected set; get; }

        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmitAsync);
        }

        public void OnSubmitAsync()
        {
            string url = "http://webapp-180314153719.azurewebsites.net/cliente/autenticar?email=" +
          //string url = "http://192.168.56.1:8080/rest/cliente/autenticar?email=" +
                             email +
                             "&senha=" +
                             senha;
            RestCallAsync(url);
        }

        private async void RestCallAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    try
                    {
                        // Use this stream to build a JSON document object:
                        JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                        System.Diagnostics.Debug.WriteLine("AUTENTICADO: " + jsonDoc["nome"]);

                        var cliente = new ClienteViewModel();
                        cliente.Nome = jsonDoc["nome"];
                        cliente.Cpf = jsonDoc["cpf"];
                        cliente.Email = jsonDoc["email"];
                        cliente.Senha = jsonDoc["senha"];

                        var clientePage = new XF_Login.Pages.ClientePage();
                        clientePage.BindingContext = cliente;
                        await App.Current.MainPage.Navigation.PushAsync(clientePage);
                        // Return the JSON document:
                        //return jsonDoc;
                    } catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("ERRO: " + e);
                        Toast.MakeText(Android.App.Application.Context, "Email/Senha incorretos!", ToastLength.Long).Show();
                    }
                }
            }
        }

    }
}
