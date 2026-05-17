using System;
using System.Windows;
using System.Windows.Controls;

namespace SMSProject
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowPanel(Grid panelToShow, string sectionTitle)
        {
            dashboardPanel.Visibility = Visibility.Collapsed;
            studentFormPanel.Visibility = Visibility.Collapsed;
            studentsManagePanel.Visibility = Visibility.Collapsed;
            courseFormPanel.Visibility = Visibility.Collapsed;
            coursesManagePanel.Visibility = Visibility.Collapsed;
            resultFormPanel.Visibility = Visibility.Collapsed;
            resultsManagePanel.Visibility = Visibility.Collapsed;
            panelToShow.Visibility = Visibility.Visible;
            sectionTitleText.Text = sectionTitle;
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(dashboardPanel, "Panel główny");
            // TODO: wczytywanie danych:
            // dashStudentsCountText.Text = ...;
            // dashCoursesCountText.Text = ...;
            // dashResultsCountText.Text = ...;
            // dashAverageGradeText.Text = ...;
            // dashRecentResultsDataGrid.ItemsSource = ...;
        }

        private void RaportyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Moduł raportów.", "Raporty", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StatystykiButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Moduł statystyk.", "Statystyki", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RegisterStudentButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(studentFormPanel, "Rejestracja studenta");
        }

        private void ManageStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(studentsManagePanel, "Zarządzanie studentami");
        }

        private void NewCourseButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(courseFormPanel, "Nowy kurs");
        }

        private void ManageCoursesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(coursesManagePanel, "Zarządzanie kursami");
        }

        private void NewResultButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(resultFormPanel, "Nowy wynik");
            if (resultDatePicker.SelectedDate == null) resultDatePicker.SelectedDate = DateTime.Now;
        }

        private void ManageResultsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(resultsManagePanel, "Zarządzanie wynikiem");
        }

        private void SaveStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(lastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(albumNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(emailTextBox.Text) ||
                genderComboBox.SelectedItem == null ||
                groupComboBox.SelectedItem == null)
            {
                MessageBox.Show("Uzupełnij wszystkie wymagane pola.", "Brakujące dane", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Dane studenta zostały przygotowane do zapisu.", "Zapisz studenta",  MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearStudentButton_Click(object sender, RoutedEventArgs e)
        {
            firstNameTextBox.Clear();
            lastNameTextBox.Clear();
            albumNumberTextBox.Clear();
            emailTextBox.Clear();
            genderComboBox.SelectedIndex = -1;
            groupComboBox.SelectedIndex = -1;
        }

        private void CancelStudentButton_Click(object sender, RoutedEventArgs e)
        {
            ClearStudentButton_Click(sender, e);
        }

        private void SearchStudentButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wyszukiwanie studentó.", "Szukaj", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshStudentButton_Click(object sender, RoutedEventArgs e)
        {
            searchStudentTextBox.Clear();
            groupFilterComboBox.SelectedIndex = 0;
            genderFilterComboBox.SelectedIndex = 0;
            // TODO: odświeżenie danych z bazy
        }

        private void DetailsStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Zaznacz studenta z listy.", "Szczegóły", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Zaznacz studenta z listy.", "Edycja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Zaznacz studenta z listy.", "Usuwanie", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Czy na pewno chcesz usunąć zaznaczonego studenta?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: usunięcie z bazy
            }
        }

        private void SaveCourseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(courseNameTextBox.Text) || ectsComboBox.SelectedItem == null || semesterComboBox.SelectedItem == null)
            {
                MessageBox.Show("Uzupełnij wszystkie wymagane pola.", "Brakujące dane", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Dane kursu zostały przygotowane do zapisu.", "Zapisz kurs", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearCourseButton_Click(object sender, RoutedEventArgs e)
        {
            courseNameTextBox.Clear();
            ectsComboBox.SelectedIndex = -1;
            semesterComboBox.SelectedIndex = -1;
            teacherNameTextBox.Clear();
        }

        private void CancelCourseButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCourseButton_Click(sender, e);
        }

        private void SearchCourseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wyszukiwanie kursów.", "Szukaj", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshCourseButton_Click(object sender, RoutedEventArgs e)
        {
            searchCourseTextBox.Clear();
            semesterFilterComboBox.SelectedIndex = 0;
        }

        private void DetailsCourseButton_Click(object sender, RoutedEventArgs e)
        {
            if (coursesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Zaznacz kurs z listy.", "Szczegóły", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditCourseButton_Click(object sender, RoutedEventArgs e)
        {
            if (coursesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Zaznacz kurs z listy.", "Edycja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteCourseButton_Click(object sender, RoutedEventArgs e)
        {
            if (coursesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Zaznacz kurs z listy.", "Usuwanie", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Czy na pewno chcesz usunąć zaznaczony kurs?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: usunięcie z bazy
            }
        }

        private void SaveResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentResultComboBox.SelectedItem == null ||
                courseResultComboBox.SelectedItem == null ||
                gradeComboBox.SelectedItem == null ||
                resultDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Uzupełnij wszystkie wymagane pola.", "Brakujące dane", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Wynik został przygotowany do zapisu.", "Zapisz wynik", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearResultButton_Click(object sender, RoutedEventArgs e)
        {
            studentResultComboBox.SelectedIndex = -1;
            courseResultComboBox.SelectedIndex = -1;
            gradeComboBox.SelectedIndex = -1;
            resultDatePicker.SelectedDate = DateTime.Now;
        }

        private void CancelResultButton_Click(object sender, RoutedEventArgs e)
        {
            ClearResultButton_Click(sender, e);
        }

        private void RefreshResultButton_Click(object sender, RoutedEventArgs e)
        {
            studentFilterComboBox.SelectedIndex = 0;
            courseFilterComboBox.SelectedIndex = 0;
            groupResultFilterComboBox.SelectedIndex = 0;
        }

        private void EditResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Zaznacz wynik z listy.", "Edycja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Zaznacz wynik z listy.", "Usuwanie", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Czy na pewno chcesz usunąć zaznaczony wynik?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: usunięcie z bazy
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}