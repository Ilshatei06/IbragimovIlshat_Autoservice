using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IbragimovIlshat_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();

            var currentServices = IbragimovI_AutoserviceEntities.GetContext().Service.ToList();

            ServiceListView.ItemsSource = currentServices;


            ComboType.SelectedIndex = 0;


            UpdateServices();

        }

        private void UpdateServices()
        {
            var currentSevices = IbragimovI_AutoserviceEntities.GetContext().Service.ToList();

            if (ComboType.SelectedIndex == 0)
                currentSevices = currentSevices.Where(p => p.DiscountInt >= 0 && p.DiscountInt <= 100).ToList();

            if (ComboType.SelectedIndex == 1)
                currentSevices = currentSevices.Where(p => p.DiscountInt >= 0 && p.DiscountInt < 5).ToList();

            if (ComboType.SelectedIndex == 2)
                currentSevices = currentSevices.Where(p => p.DiscountInt >= 5 && p.DiscountInt < 15).ToList();

            if (ComboType.SelectedIndex == 3)
                currentSevices = currentSevices.Where(p => p.DiscountInt >= 15 && p.DiscountInt < 30).ToList();

            if (ComboType.SelectedIndex == 4)
                currentSevices = currentSevices.Where(p => p.DiscountInt >= 30 && p.DiscountInt < 70).ToList();

            if (ComboType.SelectedIndex == 5)
                currentSevices = currentSevices.Where(p => p.DiscountInt >= 70 && p.DiscountInt <= 100).ToList();


            currentSevices = currentSevices.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();


            ServiceListView.ItemsSource = currentSevices.ToList();


            if (RButtonDown.IsChecked.Value)
                ServiceListView.ItemsSource = currentSevices.OrderByDescending(p => p.Cost).ToList();
            if (RButtonUp.IsChecked.Value)
                ServiceListView.ItemsSource = currentSevices.OrderBy(p => p.Cost).ToList();



        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }
        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }
        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }


 

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Service));
        }

        //обновление актуальной страницы
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                IbragimovI_AutoserviceEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ServiceListView.ItemsSource = IbragimovI_AutoserviceEntities.GetContext().Service.ToList();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var currentService = (sender as Button).DataContext as Service;

            var currentClientServices = IbragimovI_AutoserviceEntities.GetContext().ClientService.ToList();
            currentClientServices = currentClientServices.Where(p => p.ServiceID == currentService.ID).ToList();

            if (currentClientServices.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, так как существуют записи на эту услугу");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        IbragimovI_AutoserviceEntities.GetContext().Service.Remove(currentService);
                        IbragimovI_AutoserviceEntities.GetContext().SaveChanges();

                        ServiceListView.ItemsSource = IbragimovI_AutoserviceEntities.GetContext().Service.ToList();

                        UpdateServices();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }

        }
    }
}
