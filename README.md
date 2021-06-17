# BDAuto
## Manual/Development
Для работы с паттерном MVVM создадим новый проект. По умолчанию в проект добавляется стартовое окно MainWindow - это и будет представление. И теперь нам нужна Model и ViewModel.
**Model**.
Класс, который описывает автомобиль и является моделью в приложении.
Для обеспечения возможности связывания требуется, чтобы можно было отслеживать изменения в Model. Поэтому её класс реализован с помощью интерфейса INotifyPropertyChanged.
Код этого класса:
```С#
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

```
**ViewModel**.
Это класс модели представления, через который будут связаны модель Car и представление MainWindow.xaml. В этом классе определен список объектов Car и свойство, которое указывает на выделенный элемент в этом списке.
В классе модели представления есть закрытое поле и свойство для работы с выбранным из каталога автомобилем.
```С#
public Car SelectedCar
            {
                get { return _selectedCar; }
                set
                {
                    _selectedCar = value;
                    OnPropertyChanged("SelectedCar");
                }
            }
```
Сам каталог представлен в виде коллекции, которая будет заполняться в конструкторе.
```С#
public CarViewModel()
            {
                Cars = new ObservableCollection<Car>
        {
            new Car { Model="Daf 95", MaxSpeed=100, Price=1400000},
            new Car { Model="Mazda 6", MaxSpeed=150, Price=1200000},
            new Car { Model="Nissan Elgrand", MaxSpeed=186, Price=5300000}
        };
            }
```
Также включено в ViewModel два метода. Для добавления нового автомобиля и удаления уже существующего.
```С#
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
```
Полный исходный код класса ViewModel.
```C#
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
```
**View**
В случае WPF View(представление), это окно приложения.
Связывание конкретный элементов управления с данными осуществляется в XAML.
```C#
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
```
Здесь определен элемент ListBox, который привязан к свойству Cars объекта ApplicationViewModel, а также определен набор элементов, которые привязаны к свойствам объекта Car, выделенного в ListBox.
Разметка, которая отображает каталог автомобилей в ListBox, а информацию о выбранном автомобиле (выбранный мышью элемент в ListBox).
```XAML
<Window x:Class="WpfApp1.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfApp1"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="320" WindowStyle="ThreeDBorderWindow">
    <Grid Background ="White">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>
        <StackPanel Grid.Column="0" DataContext="{Binding SelectedCar}" Grid.ColumnSpan="2">
            <TextBlock FontSize="18" Text="Информация о модели" TextAlignment="Center" Opacity="0.9" FontFamily="Segoe Script" FontWeight="Normal"  >
                <TextBlock.Effect>
                    <DropShadowEffect BlurRadius="2" Color="#FF9B9B9B" Direction="307" ShadowDepth="4"/>
                </TextBlock.Effect>
            </TextBlock>
            <TextBlock FontSize="14" Text="Модель автомобиля" />
            <TextBox Text="{Binding Model, UpdateSourceTrigger=PropertyChanged}" />
            <TextBlock FontSize="14" Text="Максимальная скрорость, км/ч" />
            <TextBox Text="{Binding MaxSpeed, UpdateSourceTrigger=PropertyChanged}" />
            <TextBlock FontSize="14" Text="Цена, руб." />
            <TextBox Text="{Binding Price, UpdateSourceTrigger=PropertyChanged}" />
            <Button  Background ="DarkSeaGreen" Click="Add_Click" Content="Добавить"/>
            <Button Background ="IndianRed" Click="Delete_Click" Content="Удалить" />
        </StackPanel>
        <ListBox Grid.Column="0" Grid.Row="0" ItemsSource="{Binding Cars}"
                 SelectedItem="{Binding SelectedCar}" Grid.ColumnSpan="2" Margin="0,180,0,0" SelectionChanged="ListBox_SelectionChanged">
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <StackPanel Margin="5">
                        <TextBlock FontSize="24" Text="{Binding Path=Model}" />
                        <TextBlock FontSize="18" Text="{Binding Path=MaxSpeed}" />
                        <TextBlock FontSize="18" Text="{Binding Path=Price}" />
                    </StackPanel>
                </DataTemplate>
            </ListBox.ItemTemplate>
        </ListBox>
    </Grid>
</Window>
```
Тестирование работы программы.


## Credits
BDAuto here was coded me KritX.

## License
Code
MIT License: https://kritxx.mit-license.org/.

## Logo
Copyright 2015, Dmitriy Kritchenkov. Licensed to BDAuto Here under the MDH contributor license agreement.

## Other images
Creative Commons Attribution 3.0 Unported (CC BY 3.0) License
