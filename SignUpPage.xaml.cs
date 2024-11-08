using IbragimovIlshat_Autoservice;
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
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>

    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;


            var _currentClient = IbragimovI_AutoserviceEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;

        }

        private ClientService _currentClientService = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null) errors.AppendLine("Укажите ФИО клиента!");
            if (StartDate.SelectedDate == null)
                errors.AppendLine("Укажите дату услуги!");
            else
                if (StartDate.SelectedDate.Value.Date < DateTime.Today) errors.AppendLine("Дата услуги должна быть не ранее сегодняшнего дня!");
            
            if (TBStart.Text == "") errors.AppendLine("Укажите время начала услуги!");

            string s = TBStart.Text;
            if (s.Length <= 3 || !s.Contains(':'))
                TBEnd.Text = "";
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString());
                int startMin = Convert.ToInt32(start[1].ToString());
                if (startHour >= 24)
                    errors.AppendLine("Укажите верные часы");
                if (startMin >= 60)
                    errors.AppendLine("Укажите верные минуты");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }


            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                IbragimovI_AutoserviceEntities.GetContext().ClientService.Add(_currentClientService);
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

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            if (s.Length <= 3 || !s.Contains(':'))
                TBEnd.Text = "";
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());

                int sum = startHour + startMin + _currentService.Duration;

                int EndHour = sum / 60;
                int EndMin = sum % 60;

                EndHour = EndHour % 24;

                if (EndMin > 10)
                    s = EndHour.ToString() + ":" + EndMin.ToString();
                else
                    s = EndHour.ToString() + ":" + "0" + EndMin.ToString();

                TBEnd.Text = s;
            }
        }
    }
}




