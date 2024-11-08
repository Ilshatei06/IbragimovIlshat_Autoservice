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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    //

    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                _currentService = SelectedService;

            DataContext = _currentService;

            _currentService.DiscountInt = 0;  //устанавливает значение скидки по умочанию 0%
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги!");

            if (_currentService.Cost <= 0)
                errors.AppendLine("Укажите стоимост услуги!");

            if (_currentService.DiscountInt < 0 || _currentService.DiscountInt > 100)
                errors.AppendLine("Укажите скидку верно!");

            if (_currentService.Duration == 0)
                errors.AppendLine("Укажите длительность услуги!");
            if (_currentService.Duration > 240)
                errors.AppendLine("Длительность не может быть больше 240 минут!");
            if (_currentService.Duration < 0)
                errors.AppendLine("Длительность не может быть менее 0 минут!");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }


            var allServices = IbragimovI_AutoserviceEntities.GetContext().Service.Where(p => p.Title.ToLower() == _currentService.Title.ToLower() && p.ID != _currentService.ID).ToList();

            if (allServices.Count == 0)
            {
                if (_currentService.ID == 0)
                    IbragimovI_AutoserviceEntities.GetContext().Service.Add(_currentService);
                try
                {
                    IbragimovI_AutoserviceEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена!");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
                MessageBox.Show("Уже существует такая услуга!");
        }
    }
}
