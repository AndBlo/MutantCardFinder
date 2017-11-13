using MutantCardFinder.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MutantCardFinder.Model;

namespace MutantCardFinder.UI
{
    /// <summary>
    /// Interaction logic for AddToDatabase.xaml
    /// </summary>
    public partial class AddToDatabase : Window
    {
        private DataAccess dataAccess { get; set; }

        public AddToDatabase()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void ComboBoxCategories_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListBoxBySelectedCategory();
        }

        private void UpdateListBoxBySelectedCategory()
        {
            if (ComboBoxCategories.SelectedItem != null)
            {
                var category = ((ComboBoxItem) ComboBoxCategories.SelectedItem).Content;
                switch (category)
                {
                    case "Talanger":
                        UpdateResultListBoxWithTalents();
                        break;
                    case "Mutationer":
                        UpdateResultListBoxWithMutations();
                        break;
                    case "Artefakter":
                        UpdateResultListBoxWithArtifacts();
                        break;
                }
            }
        }

        private void UpdateResultListBoxWithArtifacts()
        {
            var artifactList = dataAccess.GetAllArtifacts();
            ListBoxResult.ItemsSource = artifactList;
        }

        private void UpdateResultListBoxWithMutations()
        {
            var mutationList = dataAccess.GetAllMutations();
            ListBoxResult.ItemsSource = mutationList;
        }

        private void UpdateResultListBoxWithTalents()
        {
            var talentList = dataAccess.GetAllTalents();
            ListBoxResult.ItemsSource = talentList;
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBoxResult.SelectedItem != null)
            {
                var item = ListBoxResult.SelectedItem;
                bool isSuccess = false;
                string objektName = "";

                if (item is TalentModel talent)
                {
                    isSuccess = dataAccess.RemoveTalent(talent);
                    objektName = talent.Name;
                }
                else if (item is MutationModel mutation)
                {
                    isSuccess = dataAccess.RemoveMutation(mutation);
                    objektName = mutation.Name;
                }
                else if (item is ArtifactModel artifact)
                {
                    isSuccess = dataAccess.RemoveArtifact(artifact);
                    objektName = artifact.Name;
                }

                if (isSuccess)
                {
                    MessageBox.Show($"\"{objektName}\" togs bort.");
                    UpdateListBoxBySelectedCategory();
                }
                else
                {
                    MessageBox.Show($"\"{objektName}\" kunde inte tas bort. \n(Ring Andreas!)");
                }
            }
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            PopulateEditFieldsWithSelectedItem();

        }

        private void PopulateEditFieldsWithSelectedItem()
        {
            if (ListBoxResult.SelectedItem != null)
            {
                ModelBase model = (ModelBase)ListBoxResult.SelectedItem;

                if (model is TalentModel)
                {
                    ComboBoxAddCategory.Text = "Talanger";
                }
                else if (model is ArtifactModel)
                {
                    ComboBoxAddCategory.Text = "Artefakter";
                }
                else if (model is MutationModel)
                {
                    ComboBoxAddCategory.Text = "Mutationer";
                }

                TextBoxName.Text = model.Name;
                TextBoxDescription.Text = model.Description;
                TextBoxGameMechanics.Text = model.GameMechanics;
            }
        }

        private void ButtonAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                UpsertItemByCategory();
                MessageBox.Show("Ändringar sparade!");
                UpdateListBoxBySelectedCategory();
                ClearFields();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ClearFields()
        {
            ComboBoxAddCategory.Text = "";
            TextBoxName.Text = "";
            TextBoxDescription.Text = "";
            TextBoxGameMechanics.Text = "";
        }

        private void UpsertItemByCategory()
        {
            var category = ((ComboBoxItem)ComboBoxAddCategory.SelectedItem).Content;
            switch (category)
            {
                case "Talanger":
                    TalentModel talent = new TalentModel()
                    {
                        Name = TextBoxName.Text,
                        Description = TextBoxDescription.Text,
                        GameMechanics = TextBoxGameMechanics.Text
                    };
                    dataAccess.UpsertTalent(talent);
                    break;
                case "Artefakter":
                    ArtifactModel artifact = new ArtifactModel()
                    {
                        Name = TextBoxName.Text,
                        Description = TextBoxDescription.Text,
                        GameMechanics = TextBoxGameMechanics.Text
                    };
                    dataAccess.UpsertArtifact(artifact);
                    break;
                case "Mutationer":
                    MutationModel mutation = new MutationModel()
                    {
                        Name = TextBoxName.Text,
                        Description = TextBoxDescription.Text,
                        GameMechanics = TextBoxGameMechanics.Text
                    };
                    dataAccess.UpsertMutation(mutation);
                    break;
                default:
                    break;
            }
        }

        private void ButtonClearFields_OnClick(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ListBoxResult_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxResult.SelectedItem != null)
            {
                PopulateEditFieldsWithSelectedItem();
            }
        }
    }
}
