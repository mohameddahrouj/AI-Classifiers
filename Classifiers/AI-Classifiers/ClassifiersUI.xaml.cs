using System.Windows;
using Classifiers.ViewModel;
using System.IO;

namespace AI_Classifiers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            _filePath = Directory.GetParent(_filePath).FullName;
            _filePath = Directory.GetParent(Directory.GetParent(_filePath).FullName).FullName;
            _filePath += @"\AI-Classifiers\Datasets\ArtificialData.csv";
            this.DataContext = new ClassifierViewModel(_filePath);
            InitializeComponent();
        }

        private void IndependentClassification(object sender, RoutedEventArgs e)
        {
            ((ClassifierViewModel)this.DataContext).IndependentClassification();
        }

        private void DependentClassification(object sender, RoutedEventArgs e)
        {
            ((ClassifierViewModel)this.DataContext).DependentClassifcation();
        }

        private void DecisionClassification(object sender, RoutedEventArgs e)
        {
            ((ClassifierViewModel)this.DataContext).DecisionClassification();
        }
    }
}
