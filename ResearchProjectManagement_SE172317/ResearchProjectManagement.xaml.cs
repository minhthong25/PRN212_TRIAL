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
using System.Windows.Shapes;
using Repository.Models;
using BLL.Service;

namespace ResearchProjectManagement_SE172317
{
    /// <summary>
    /// Interaction logic for ResearchProjectManagement.xaml
    /// </summary>
    public partial class ResearchProjectManagement : Window
    {
        private ResearchProjectService _projectService;
        private ResearcherService _researcherService;
        private List<ResearchProject> _projects;
        private List<Researcher> _researchers;
        private int CurrentUserRole;
        private bool IsAuthenticated;

        public ResearchProjectManagement(int userRole, bool isAuthenticated)
        {
            InitializeComponent();
            _projectService = new ResearchProjectService();
            _researcherService = new ResearcherService();
            CurrentUserRole = userRole;
            IsAuthenticated = isAuthenticated;
            LoadData();
        }

        public ResearchProjectManagement() : this(1, true) { }

        private void LoadData()
        {   
            if (!IsAuthenticated)
            {
                MessageBox.Show("Invalid Email or Password!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentUserRole != 1 && CurrentUserRole != 2 && CurrentUserRole != 3)
            {
                MessageBox.Show("You have no permission to access this function!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _researchers = _researcherService.GetAll();
            cbLeadResearcher.ItemsSource = _researchers;

            _projects = _projectService.GetAll();
            dgProjects.ItemsSource = _projects;
        }

        private void dgProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsAuthenticated)
            {
                MessageBox.Show("Invalid Email or Password!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentUserRole != 1 && CurrentUserRole != 2 && CurrentUserRole != 3)
            {
                MessageBox.Show("You have no permission to access this function!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dgProjects.SelectedItem is ResearchProject selected)
            {
                txtTitle.Text = selected.ProjectTitle;
                txtResearchField2.Text = selected.ResearchField;
                txtBudget2.Text = selected.Budget.ToString();
                dpStartDate.SelectedDate = selected.StartDate.ToDateTime(TimeOnly.MinValue);
                dpEndDate.SelectedDate = selected.EndDate.ToDateTime(TimeOnly.MinValue);
                cbLeadResearcher.SelectedValue = selected.LeadResearcherId;
            }
        }

        private bool IsValidProjectTitle(string title)
        {
            if (title.Length < 5 || title.Length > 100)
                return false;
            if (title.Contains("$") || title.Contains("%") || title.Contains("^") || title.Contains("@"))
                return false;
            var words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (!(char.IsUpper(word[0]) || (word[0] >= '1' && word[0] <= '9')))
                    return false;
            }
            return true;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAuthenticated)
            {
                MessageBox.Show("Invalid Email or Password!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentUserRole != 1 && CurrentUserRole != 2)
            {
                MessageBox.Show("You have no permission to access this function!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (
                string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtResearchField2.Text) ||
                string.IsNullOrWhiteSpace(txtBudget2.Text) ||
                !dpStartDate.SelectedDate.HasValue ||
                !dpEndDate.SelectedDate.HasValue ||
                cbLeadResearcher.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsValidProjectTitle(txtTitle.Text))
            {
                MessageBox.Show("ProjectTitle must be 5-100 characters, each word starts with a capital letter or digit (1-9), and cannot contain $, %, ^, @.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!decimal.TryParse(txtBudget2.Text, out decimal budget))
            {
                MessageBox.Show("Budget must be a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var project = new ResearchProject
            {
                ProjectTitle = txtTitle.Text,
                ResearchField = txtResearchField2.Text,
                Budget = budget,
                StartDate = DateOnly.FromDateTime(dpStartDate.SelectedDate.Value),
                EndDate = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value),
                LeadResearcherId = cbLeadResearcher.SelectedValue as int? ?? 0
            };
            try
            {
                _projectService.AddService(project);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving project: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAuthenticated)
            {
                MessageBox.Show("Invalid Email or Password!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentUserRole != 1 && CurrentUserRole != 2)
            {
                MessageBox.Show("You have no permission to access this function!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dgProjects.SelectedItem is ResearchProject selected)
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                    string.IsNullOrWhiteSpace(txtResearchField2.Text) ||
                    string.IsNullOrWhiteSpace(txtBudget2.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!IsValidProjectTitle(txtTitle.Text))
                {
                    MessageBox.Show("ProjectTitle must be 5-100 characters, each word starts with a capital letter or digit (1-9), and cannot contain $, %, ^, @.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!decimal.TryParse(txtBudget2.Text, out decimal budget))
                {
                    MessageBox.Show("Budget must be a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                selected.ProjectTitle = txtTitle.Text.Trim();
                selected.ResearchField = txtResearchField2.Text.Trim();
                selected.Budget = budget;
                

                try
                {

                    _projectService.UpdateService(selected);
                    LoadData();
                    ClearForm();
                }
                catch (Exception )
                {
                    MessageBox.Show("Error updating project: " , "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAuthenticated)
            {
                MessageBox.Show("Invalid Email or Password!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentUserRole != 1)
            {
                MessageBox.Show("You have no permission to access this function!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dgProjects.SelectedItem is ResearchProject selected)
            {
                _projectService.DeleteService(selected.ProjectId);
                LoadData();
                ClearForm();
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtTitle.Text = "";
            txtResearchField2.Text = "";
            txtBudget2.Text = "";
            dpStartDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
            cbLeadResearcher.SelectedIndex = -1;
            dgProjects.SelectedIndex = -1;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAuthenticated)
            {
                MessageBox.Show("Invalid Email or Password!", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentUserRole != 1 && CurrentUserRole != 2 && CurrentUserRole != 3)
            {
                MessageBox.Show("You have no permission to access this function!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int? id = null;
            if (int.TryParse(txtSearchId2.Text, out int parsedId))
                id = parsedId;
            string title = txtSearchTitle2.Text.Trim();
            var results = _projectService.SearchService(id, title);
            dgProjects.ItemsSource = results;
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
