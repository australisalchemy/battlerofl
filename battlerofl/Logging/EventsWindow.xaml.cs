﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace battlerofl
{
    /// <summary>
    /// Interaction logic for EventsWindow.xaml
    /// </summary>
    public partial class EventsWindow : Window
    {
        public EventsWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide(); // hide, do not close ;)
        }
    }
}
