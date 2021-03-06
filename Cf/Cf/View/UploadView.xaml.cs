﻿using Cf.Model;
using Cf.Data;
using Cf.ViewModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cf.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadView : ContentPage
    {
        private Stream Stream = null;
        private MediaFile File;
        private TabbedPage MainPage;

        public ObservableCollection<PostModel> posts { get; set; }

        public string Arquivo = string.Empty;

        public UploadView(TabbedPage mainPage)
        {
            InitializeComponent();

            this.MainPage = mainPage;
            this.Title = "enviar catioro fofo";
            CrossMedia.Current.Initialize();

            this.BindingContext = App.UsuarioVM;
        }

        public async void EscolherFoto()
        {
            try
            {
                //var cameraNaoDisponivel = !CrossMedia.Current.IsCameraAvailable;
                var escolherFotoNaoSuportado = !CrossMedia.Current.IsPickPhotoSupported;

                if (/*cameraNaoDisponivel || */escolherFotoNaoSuportado)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    // return;
                }

                File = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    CompressionQuality = 50
                });

                if (File == null)
                    return;

                PostarButton.IsEnabled = true;
                PostarButton.IsVisible = true;

                FotoImage.Source = ImageSource.FromStream(ObterStream);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected async void PostarButtonClicked(object o, EventArgs args)
        {
            PostModel post = new PostModel(Stream)
            {
                Legenda = LegendaEntry.Text,
                Usuario = App.UsuarioVM.Usuario
            };
            var resposta = await App.PostVM.Salvar(post);

            if (resposta == RespostaStatus.ErroGenerico)
            {
                await DisplayAlert("erro", "ocorreu um erro", "cancelar");
                return;
            }

            var expViewCode = new ExplorarView();
            MainPage.CurrentPage = MainPage.Children[0];
        }


        protected async void OnImageTapped(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            File = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            {
                CompressionQuality = 50
            });

            if (File == null)
                return;

            FotoImage.Source = ImageSource.FromStream(ObterStream);
        }
        private Stream ObterStream()
        {
            Stream stream = File.GetStream();
            Stream = File.GetStream();
            File.Dispose();

            return stream;
        }
    }
}
