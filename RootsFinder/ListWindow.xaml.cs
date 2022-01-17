using System;
using System.Collections.Generic;
using System.Windows;

namespace RootsFinder
{
    public partial class ListWindow : Window
    {
        public ListWindow(string title, List<string> strings)
        {
            InitializeComponent();

            Title = title;
            listBox.ItemsSource = strings;
        }

        public ListWindow(string title, List<string> strings, int width, int height)
        {
            InitializeComponent();

            Title = title;
            listBox.ItemsSource = strings;
            Width = width;
            Height = height;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Focus();
        }
    }
}
