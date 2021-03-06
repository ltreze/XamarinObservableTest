﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cf.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilView : ContentPage
    {
        MediaFile File;
        bool modoEdicao = true;
        Stream Stream = null;
        string NomeUsuarioValorInicial;

        public PerfilView()
        {
            InitializeComponent();

            CrossMedia.Current.Initialize();

            this.BindingContext = App.UsuarioVM.Usuario;
        }

        protected void Salvar_Clicked(object sender, EventArgs e)
        {
            //chamada do metodo que persiste
        }

        protected async void OnAvatarImageTapped(object sender, EventArgs e)
        {
            if (modoEdicao)
            {
                //var cameraNaoDisponivel = !CrossMedia.Current.IsCameraAvailable;
                var escolherFotoNaoSuportado = !CrossMedia.Current.IsPickPhotoSupported;

                if (/*cameraNaoDisponivel || */escolherFotoNaoSuportado)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                File = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { CompressionQuality = 10 });

                if (File == null)
                    return;

                App.UsuarioVM.EditouAvatar = true;

                avatarImage.Source = ImageSource.FromStream(() =>
                {
                    Stream stream = File.GetStream();
                    Stream = File.GetStream();
                    File.Dispose();

                    return stream;
                });
            }
        }

        protected void EditarClicked(object sender, EventArgs e)
        {
            App.UsuarioVM.TempEmail = App.UsuarioVM.Usuario.Email;
            App.UsuarioVM.TempNomeArquivoAvatar = App.UsuarioVM.Usuario.NomeArquivoAvatar;
            App.UsuarioVM.TempNomeUsuario = App.UsuarioVM.Usuario.Nome;

            modoEdicao = true;
            NomeUsuarioValorInicial = nomeUsuarioEntry.Text;

            nomeUsuarioEntry.IsEnabled = true;
            nomeUsuarioEntry.IsVisible = true;

            emailEntry.IsEnabled = true;
            emailEntry.IsVisible = true;

            cancelarButton.IsVisible = true;
            cancelarButton.IsEnabled = true;

            salvarButton.IsVisible = true;
            salvarButton.IsEnabled = true;

            editarButton.IsVisible = false;
            editarButton.IsEnabled = false;
        }

        protected void CancelarClicked(object sender, EventArgs e)
        {
            nomeUsuarioEntry.IsEnabled = false;
            emailEntry.IsEnabled = false;

            cancelarButton.IsVisible = false;
            cancelarButton.IsEnabled = false;

            salvarButton.IsVisible = false;
            salvarButton.IsEnabled = false;

            editarButton.IsVisible = true;
            editarButton.IsEnabled = true;

            nomeUsuarioEntry.Text = NomeUsuarioValorInicial;
            modoEdicao = false;
        }


        protected async void SalvarClicked(object sender, EventArgs e)
        {
            nomeUsuarioEntry.IsEnabled = false;
            emailEntry.IsEnabled = false;

            cancelarButton.IsVisible = false;
            cancelarButton.IsEnabled = false;

            salvarButton.IsVisible = false;
            salvarButton.IsEnabled = false;

            editarButton.IsVisible = true;
            editarButton.IsEnabled = true;

            /////App.UsuarioVM.Usuario.SetarAvatarStream(Stream);

            //var bytes = ReadFully();
            var resultado = await App.UsuarioVM.AtualizarCadastro(Stream);

            //switch (resultado)
            //{
            //    case RespostaStatus.Sucesso:
            //        break;
            //    case RespostaStatus.Inexistente:
            //        await DisplayAlert("ops", "erro estranho", "volta lá");
            //        break;
            //    case RespostaStatus.JaExiste:
            //        await DisplayAlert("ops", "ja existe nome de usuario", "volta lá");
            //        nomeUsuarioEntry.Focus();
            //        break;
            //}
            modoEdicao = false;
        }
    }
}
