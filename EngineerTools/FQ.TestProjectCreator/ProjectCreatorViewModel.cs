using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using FQ.Libraries.Tools.HighLevelElements;
using MessageBox = System.Windows.Forms.MessageBox;

namespace TestProjectCreator
{
    public class ProjectCreatorViewModel : NotifyPropertyChanged
    {
        public string FolderLocation
        {
            get => folderLocation;
            set
            {
                if (value != this.folderLocation)
                {
                    folderLocation = value;
                    OnPropertyChanged(nameof(FolderLocation));
                }
            }
        }
        private string folderLocation;

        public ICommand FindFolder => findFolder;
        private readonly ICommand findFolder;
        
        public ICommand CreateProjectAssembly => createProjectAssembly;
        private readonly ICommand createProjectAssembly;
        
        public string SelectedItem
        {
            get => selectedItem;
            set
            {
                if (value != this.selectedItem)
                {
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        private string selectedItem;
        
        public ObservableCollection<string> ProjectType
        {
            get => projectType;
            set
            {
                if (value != this.projectType)
                {
                    projectType = value;
                    OnPropertyChanged(nameof(ProjectType));
                }
            }
        }
        private ObservableCollection<string> projectType;
        
        public string ProjectName
        {
            get => projectName;
            set
            {
                if (value != this.projectName)
                {
                    projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }
        private string projectName;

        public ProjectCreatorViewModel()
        {
            this.findFolder = new Command(_ => { OnFindFolder();}, _ => true);
            this.createProjectAssembly = new Command(_ => { OnCreateProjectAssembly();}, _ => true);
            this.folderLocation = Path.Combine(@"E:\");
            ProjectType = new ObservableCollection<string>()
            {
                "Editor", "Player"
            };
        }

        private void OnCreateProjectAssembly()
        {
            if (!CanCreateProject(FolderLocation, ProjectName, SelectedItem))
            {
                return;
            }

            ProjectName = ProjectName.ToLower().Replace(".asmdef", "");
            
            string fileContents = "{\n" +
                $"\"name\": \"{ProjectName}\",\n" +
            "\"rootNamespace\": \"\",\n"+
                "\"references\": [],\n";

            if (SelectedItem == "Editor")
            {
                fileContents += "\"includePlatforms\": [\n" +
                                "\"Editor\"\n" +
                                "],\n";
            }
            else
            {
                fileContents += "\"includePlatforms\": [],\n";
            }
            
            fileContents += "\"excludePlatforms\": [],\n" +
                            "\"allowUnsafeCode\": false,\n" +
                            "\"overrideReferences\": true,\n" +
                            "\"precompiledReferences\": [\n" +
                            "\"nunit.framework.dll\",\n" +
                            "\"Castle.Core.dll\",\n" +
                            "\"Moq.dll\",\n" +
                            "\"log4netPlastic.dll\"\n" +
                            "],\n" +
                            "\"autoReferenced\": true,\n" +
                            "\"defineConstraints\": [\n" +
                            "\"UNITY_INCLUDE_TESTS\"\n" +
                            "],\n" +
                            "\"versionDefines\": [],\n" +
                            "\"noEngineReferences\": false\n" +
                            "}";

            File.WriteAllLines($@"{FolderLocation}\{ProjectName}.asmdef", new string[] {fileContents});

            MessageBox.Show("Written File");
        }

        private bool CanCreateProject(string folderLocation, string projectName, string projectType)
        {
            if (string.IsNullOrWhiteSpace(folderLocation))
            {
                MessageBox.Show("Please enter a folder.");
                return false;
            }

            if (!Directory.Exists(folderLocation))
            {
                MessageBox.Show("Folder does not exist.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                MessageBox.Show("Please enter a project name");
                return false;
            }

            if (string.IsNullOrWhiteSpace(projectType))
            {
                MessageBox.Show("Please enter a type of project.");
                return false;
            }
            
            return true;
        }

        private void OnFindFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = FolderLocation;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FolderLocation = dialog.SelectedPath;
            }
        }
    }
}