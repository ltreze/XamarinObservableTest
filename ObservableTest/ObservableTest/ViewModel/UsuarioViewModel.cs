﻿using ObservableTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableTest.ViewModel
{
    public class UsuarioViewModel : INotifyPropertyChanged
    {
        public UsuarioViewModel() { }

        
        #region Propriedades

        public int Id { get; set; }
        public string Nome { get; set; }
        public object AvatarResource { get; set; }
        public bool EditouAvatar { get; set; }

        public UsuarioModel Usuario
        {
            get
            {
                return App.Database.GetItemAsync(1).Result;
            }
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
    }
}