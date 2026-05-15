using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Projekt_PO_KW.Helpers
{
    internal class DialogHelper
    {
        public static string? Show(string tytul, string etykieta, string domyslna = "")
        {
            var okno = new Window
            {
                Title = tytul,
                Width = 300,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize
            };

            var stack = new StackPanel { Margin = new Thickness(20) };
            var label = new TextBlock { Text = etykieta, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 8) };
            var textbox = new System.Windows.Controls.TextBox { Text = domyslna, Padding = new Thickness(6) };

            var button = new System.Windows.Controls.Button
            {
                Content = "Zapisz",
                Margin = new Thickness(10, 10, 10, 0),
                Background = System.Windows.Media.Brushes.LightPink,
                Width = 150,
                Height = 35,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };

            string? wynik = null;
            button.Click += (s, e) => { wynik = textbox.Text; okno.Close(); };

            stack.Children.Add(label);
            stack.Children.Add(textbox);
            stack.Children.Add(button);
            okno.Content = stack;
            okno.ShowDialog();

            return wynik;
        }
    }
}
