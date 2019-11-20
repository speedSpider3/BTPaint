﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BTPaint.UserControls
{
    public sealed partial class Join : ContentDialog
    {
        public bool mainMenu { get; set; }

        public Join()
        {
            this.InitializeComponent();
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            mainMenu = false;
        }

        private async void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            mainMenu = true;
        }
    }
}
