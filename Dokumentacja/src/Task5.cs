    // Metoda WybierzZabieg

    foreach (var item in ListaZabiegow.Items)
    {
        var container = 
        ListaZabiegow.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;

        if (container != null)
        {
            var border = FindChild<Border>(container);
            if (border != null) border.BorderBrush = 
            new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FFE5E7EB"));
        }
    }

    var clicked = sender as Border;

    if (clicked != null)
    {
        clicked.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)
        System.Windows.Media.ColorConverter.ConvertFromString("#FFFB7185"));
        _wybranyZabieg = clicked.DataContext as Models.Zabieg;
    }

    
    // Metoda WybierzWeterynarza

    foreach (var item in ListaWeterynarzy.Items)
    {
        var container = 
        ListaWeterynarzy.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;

        if (container != null)
        {
            var border = FindChild<Border>(container);
            if (border != null) border.BorderBrush = 
            new SolidColorBrush((System.Windows.Media.Color)
            System.Windows.Media.ColorConverter.ConvertFromString("#FFE5E7EB"));
        }
    }

    var clickedBorder = sender as Border;

    if (clickedBorder != null)
    {
        clickedBorder.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)
        System.Windows.Media.ColorConverter.ConvertFromString("#FFFB7185"));
        _wybranyWeterynarz = clickedBorder.DataContext as Models.Weterynarz;
    }