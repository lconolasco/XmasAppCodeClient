using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using XmasAppCodeCliet.Models;

namespace XmasAppCodeCliet.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        //Serviço de conexao para consumo REST API
        HttpClient client;

        //Configuraçao do JSON
        JsonSerializerOptions _serializerOptions;

        //Definiçao da Url base
        string baseUrl = "https://lconolasco.somee.com/api";

        [ObservableProperty]
        public int _id;

        [ObservableProperty]
        public string _barcode;

        [ObservableProperty]
        public string _nome;

        [ObservableProperty]
        public string imageUrl;
        
        [ObservableProperty]
        public string _descrizione;

        [ObservableProperty]
        public decimal _prezzo;

        [ObservableProperty]
        public decimal _pesoLordo;

        [ObservableProperty]
        public int _CategoriaId;

        [ObservableProperty]
        public ObservableCollection<Prodotto> _catalogoProdotti;

        [ObservableProperty]
        public Prodotto _prodotto;

        [ObservableProperty]
        public string _message;

        public MainViewModel()
        {
            //Instancia de HttpClient
           client = new HttpClient();

            //Instancia de Coleçao de objetos
           CatalogoProdotti = new ObservableCollection<Prodotto>();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        //** Consumo da API REST **//

        //retorna a coleçao de produtos
        public ICommand RicuperaProdotti => new Command(async () => await RicuperaProdottiAsync());

        private async Task RicuperaProdottiAsync()
        {
            var url = $"{baseUrl}/Prodotto";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<ObservableCollection<Prodotto>>(responseStream, _serializerOptions);
                    CatalogoProdotti = data;
                }
            }
        }

        //retorna um produto pelo barcode
        public ICommand RicuperaProdottoDalBarcode =>
            new Command(async () =>
               {
                   try
                   {
                       if (Barcode is not null)
                   {
                       var barcodeProdotto = Convert.ToInt64(Barcode);
                       if(barcodeProdotto > 0)
                       {
                           var url = $"{baseUrl}/Prodotto/{Barcode}";
                           var response = await client.GetAsync(url);

                           if (response.IsSuccessStatusCode)
                           {
                               using (var responseStream = await response.Content.ReadAsStreamAsync())
                               {
                                   var data = await JsonSerializer.DeserializeAsync<Prodotto>(responseStream, _serializerOptions);
                                   Prodotto = data;
                                   CatalogoProdotti.Add(data);

                               }
                               }
                               else
                               {
                                   Message = $"Conessione falita col DataBase. Controllare le conessione e riprova.";
                               }
                       }
                   }
                   }
                   catch (Exception)
                   {
                       _ = Shell.Current.DisplayAlert("Avviso", "Prodotto non Trovato", "Ok");
                      /* Prodotto vuoto = new Prodotto();
                       vuoto.Nome = "Nessuno Prodotto trovato con questo codice.";
                       vuoto.Descrizione = "Certifica che questo prodotto sia inserito nel Database in precedenza.";
                       CatalogoProdotti.Add(vuoto);*/
                   }
                   
               });
    }
}
