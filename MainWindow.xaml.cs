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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CarViewModel();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ((CarViewModel)DataContext).AddCar();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ((CarViewModel)DataContext).DeleteCar();
        }

        class Car : INotifyPropertyChanged
        {
            private string _model;
            private int _maxSpeed;
            private decimal _price;
            public string Model
            {
                get { return _model; }
                set
                {
                    _model = value;
                    OnPropertyChanged("Model");
                }
            }
            public int MaxSpeed
            {
                get
                {
                    return _maxSpeed;
                }
                set
                {
                    _maxSpeed = value;
                    OnPropertyChanged("MaxSpeed");
                }
            }
            public decimal Price
            {
                get { return _price; }
                set
                {
                    _price = value;
                    OnPropertyChanged("Price");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        class CarViewModel : INotifyPropertyChanged

        {
            private Car _selectedCar;
            public ObservableCollection<Car> Cars { get; set; }
            public Car SelectedCar
            {
                get { return _selectedCar; }
                set
                {
                    _selectedCar = value;
                    OnPropertyChanged("SelectedCar");
                }
            }
            public CarViewModel()
            {
                Cars = new ObservableCollection<Car>
        {
            new Car { Model="Daf 95", MaxSpeed=100, Price=1400000},
            new Car { Model="Mazda 6", MaxSpeed=150, Price=1200000},
            new Car { Model="Nissan Elgrand", MaxSpeed=186, Price=5300000}
        };
            }
            public void AddCar()
            {
                Car car = new Car();
                Cars.Insert(0, car);
                SelectedCar = car;
            }
            public void DeleteCar()
            {
                if (_selectedCar != null)
                {
                    Cars.Remove(SelectedCar);
                }
            }
            
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}