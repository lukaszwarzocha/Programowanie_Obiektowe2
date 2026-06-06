    //Metoda NavClick

    PanelMojeKonto.Visibility = Visibility.Collapsed;
    PanelMojePupile.Visibility = Visibility.Collapsed;
    PanelMojeWizyty.Visibility = Visibility.Collapsed;
    PanelDoladujSrodki.Visibility = Visibility.Collapsed;

    BtnDoladujSrodki.Tag = null;
    BtnMojeKonto.Tag = null;
    BtnMojePupile.Tag = null;
    BtnMojeWizyty.Tag = null;

    var przycisk = sender as System.Windows.Controls.Button;
    przycisk!.Tag = "active";

    if (przycisk == BtnMojeKonto)
        PanelMojeKonto.Visibility = Visibility.Visible;
    else if (przycisk == BtnMojePupile)
    {
        PanelMojePupile.Visibility = Visibility.Visible;
        WczytajPupile();
    }
    else if (przycisk == BtnMojeWizyty)
    {
        PanelMojeWizyty.Visibility = Visibility.Visible;
        WczytajWizyty();
    }
    else if (przycisk == BtnDoladujSrodki)
    {
        PanelDoladujSrodki.Visibility = Visibility.Visible;
    }
