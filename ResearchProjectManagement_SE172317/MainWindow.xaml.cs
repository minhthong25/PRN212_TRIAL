using BLL.Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace ResearchProjectManagement_SE172317
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserAccountService service;
        public MainWindow()
        {
            InitializeComponent();
            service = new UserAccountService();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtEmail.Text;
            string upassword = txtPassword.Password;

            bool checklogin = service.CheckLogin(username, upassword);

            if (checklogin == true)
            {
                var user = service.GetAll().FirstOrDefault(u => u.Email == username);
                if (user != null && user.Role == 4)
                {
                    System.Windows.MessageBox.Show("You have no permission to Login", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                System.Windows.MessageBox.Show("Login successful!");

                int userRole = (int)(user != null ? user.Role : 1);
                bool isAuthenticated = true;
                ResearchProjectManagement researchProjectManagement = new ResearchProjectManagement(userRole, isAuthenticated);
                researchProjectManagement.Show();

                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}