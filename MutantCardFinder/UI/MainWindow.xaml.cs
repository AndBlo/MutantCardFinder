using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using MutantCardFinder.DAL;
using MutantCardFinder.Model;
using MutantCardFinder.UI;

namespace MutantCardFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataAccess DataAccess { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataAccess = new DataAccess();
        }

        private void ButtonSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var category = ((ComboBoxItem)ComboBoxCategories.SelectedItem).Content;
            var nameLike = TextBoxSearch.Text;

            switch (category)
            {
                case "Talang":
                    var talents = DataAccess.GetTalentsByNameSearch(nameLike);
                    ListBoxResult.ItemsSource = talents;
                    break;
                case "Artefakt":
                    var artifacts = DataAccess.GetArtifactsByNameSearch(nameLike);
                    ListBoxResult.ItemsSource = artifacts;
                    break;
                case "Mutation":
                    var mutations = DataAccess.GetMutationsByNameSearch(nameLike);
                    ListBoxResult.ItemsSource = mutations;
                    break;
            }
        }


        private void ListBoxResult_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxResult.SelectedItem != null)
            {
                ModelBase model = (ModelBase)ListBoxResult.SelectedItem;
                AssignModelFields(model);
            }
        }

        private void AssignModelFields(ModelBase model)
        {
            if (model is TalentModel)
            {
                LabelCategory.Content = "Talang";
            }
            if (model is ArtifactModel)
            {
                LabelCategory.Content = "Artefakt";
            }
            if (model is MutationModel)
            {
                LabelCategory.Content = "Mutation";
            }

            LabelName.Content = model.Name;
            TextBoxDescription.Text = model.Description;
            TextBoxGameMechanics.Text = model.GameMechanics;
        }

        private void MenuItemEditDb_OnClick(object sender, RoutedEventArgs e)
        {
            var edit = new AddToDatabase();
            edit.Show();
        }

        private void MenuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();

        }


        private void MakeBackupOfDataBase(string mutantDb)
        {
            string dbFileName = mutantDb + DateTime.Now.ToString("yyyyMMddHHmmss") + ".db";
            string newPath = Directory.GetCurrentDirectory() + @"\DB_Backup\";
            if (!Directory.Exists(newPath))
            {
                System.IO.Directory.CreateDirectory(newPath);
            }
            var files = Directory.EnumerateFiles(newPath).ToList();

            if (files.Count >= 10)
            {
                var oldestFile = files.OrderBy(f => f).Select(f => f).FirstOrDefault();
                File.Delete(oldestFile);
            }

            if (File.Exists(mutantDb + ".db"))
            {
                File.Copy(mutantDb + ".db", newPath + dbFileName);
            }
        }

        private void ButtonRandomize_OnClick(object sender, RoutedEventArgs e)
        {
            var list = ListBoxResult.ItemsSource;
            Random rnd = new Random();

            if (list is BindingList<TalentModel> talents)
            {
                AssignModelFields(talents[rnd.Next(0, talents.Count)]);
            }
            if (list is BindingList<ArtifactModel> artifacts)
            {
                AssignModelFields(artifacts[rnd.Next(0, artifacts.Count)]);
            }
            if (list is BindingList<MutationModel> mutations)
            {
                AssignModelFields(mutations[rnd.Next(0, mutations.Count)]);
            }

        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            MakeBackupOfDataBase(@"Mutant");
        }
    }
}
