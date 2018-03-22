using Android.Widget;
using System;
using System.ComponentModel;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XF_Login.ViewModels
{
    public class ClienteViewModel : INotifyPropertyChanged
    {
        private string nome;
        public string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Nome"));
            }
        }

        private string cpf;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Cpf
        {
            get { return cpf; }
            set
            {
                cpf = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Cpf"));
            }
        }

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
        public ICommand ConsultCommand { protected set; get; }
        public ICommand NewCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }

        public ClienteViewModel()
        {
            SubmitCommand = new Command(OnSubmitAsync);
            ConsultCommand = new Command(OnConsultAsync);
            NewCommand = new Command(OnNewAsync);
            DeleteCommand = new Command(OnDeleteAsync);
        }

        public void OnSubmitAsync()
        {
            string url = "http://webapp-180314153719.azurewebsites.net/";
           // string url = "http://192.168.56.1:8080/rest";

            RestPostAsync(url);
        }

        private async void RestPostAsync(string url)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);

            string jsonData = "{\"cpf\": \""+cpf+"\", \"nome\": \""+nome+"\", \"email\": \""+email+"\", \"senha\": \""+senha+"\"}";
            System.Diagnostics.Debug.WriteLine("POST 1: " + jsonData);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/cliente/salvar/", content);

            var result = await response.Content.ReadAsStringAsync();
            Toast.MakeText(Android.App.Application.Context, "Cliente salvo com sucesso!", ToastLength.Long).Show();

        }

        public void OnConsultAsync()
        {
            string url = "http://webapp-180314153719.azurewebsites.net/cliente/consultar?cpf=" + cpf;
            //string url = "http://192.168.56.1:8080/rest/cliente/consultar?cpf=" + cpf;
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
                    try { 
                        // Use this stream to build a JSON document object:
                        JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                        System.Diagnostics.Debug.WriteLine("CONSULTAR: " + jsonDoc["nome"]);

                        Nome = jsonDoc["nome"];
                        Cpf = jsonDoc["cpf"];
                        Email = jsonDoc["email"];
                        Senha = jsonDoc["senha"];
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("ERRO: " + e);
                        Toast.MakeText(Android.App.Application.Context, "Cliente não encontrado!", ToastLength.Long).Show();
                        Nome = "";
                        Email = "";
                        Senha = "";
                    }
                }
            }
        }
        public void OnNewAsync()
        {
            Nome = "";
            Cpf = "";
            Email = "";
            Senha = "";
        }

        public void OnDeleteAsync()
        {
            string url = "http://webapp-180314153719.azurewebsites.net/";
           // string url = "http://192.168.56.1:8080/rest";

            RestPostDeleteAsync(url);
        }

        private async void RestPostDeleteAsync(string url)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);

            string jsonData = cpf;
            System.Diagnostics.Debug.WriteLine("POST 1: " + jsonData);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/cliente/excluir/", content);

            var result = await response.Content.ReadAsStringAsync();
            Toast.MakeText(Android.App.Application.Context, "Cliente excluído com sucesso!", ToastLength.Long).Show();

        }
    }
}
