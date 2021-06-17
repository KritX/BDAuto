# BDAuto
## Manual/Development
To work with the MVVM pattern, let's create a new project. By default, the starting window MainWindow is added to the project - this will be the view. And now we need a Model and a ViewModel.
(Для работы с паттерном MVVM создадим новый проект. По умолчанию в проект добавляется стартовое окно MainWindow - это и будет представление. И теперь нам нужна Model и ViewModel.)

**Model**.
The class that describes the car and is the model in the application.
Linking requires you to be able to track changes in Model. Therefore, its class is implemented using the INotifyPropertyChanged interface.
The code for this class:
(Класс, который описывает автомобиль и является моделью в приложении.
Для обеспечения возможности связывания требуется, чтобы можно было отслеживать изменения в Model. Поэтому её класс реализован с помощью интерфейса INotifyPropertyChanged.
Код этого класса:)
```
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
This is the view model class through which the Car model and the MainWindow.xaml view will be linked. This class defines a list of Car objects and a property that points to the selected item in this list.
The view model class has a private field and a property for working with a car selected from the catalog.
(Это класс модели представления, через который будут связаны модель Car и представление MainWindow.xaml. В этом классе определен список объектов Car и свойство, которое указывает на выделенный элемент в этом списке.
В классе модели представления есть закрытое поле и свойство для работы с выбранным из каталога автомобилем.)
```
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
The catalog itself is presented as a collection, which will be filled in the constructor.
(Сам каталог представлен в виде коллекции, которая будет заполняться в конструкторе.)
```
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
Also included in the ViewModel are two methods. To add a new car and delete an existing one.
(Также включено в ViewModel два метода. Для добавления нового автомобиля и удаления уже существующего.)
```
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
The complete source code for the ViewModel class.
(Полный исходный код класса ViewModel.)
```
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
In the case of WPF View, this is the application window.
The binding of specific controls to data is done in XAML.
(В случае WPF View(представление), это окно приложения.
Связывание конкретный элементов управления с данными осуществляется в XAML.)
```
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
This defines a ListBox element that is bound to the Cars property of the ApplicationViewModel, and also defines a set of elements that are bound to the properties of the Car object selected in the ListBox.
Markup that displays the catalog of cars in the ListBox, and information about the selected car (the item selected with the mouse in the ListBox).
(Здесь определен элемент ListBox, который привязан к свойству Cars объекта ApplicationViewModel, а также определен набор элементов, которые привязаны к свойствам объекта Car, выделенного в ListBox.
Разметка, которая отображает каталог автомобилей в ListBox, а информацию о выбранном автомобиле (выбранный мышью элемент в ListBox).)
```
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
**Testing the program.**
Deleting information about Mazda car (Click on delete button)
(Удаление информации о машине Mazda(Клик по кнопки удалить))
![image](https://user-images.githubusercontent.com/70913346/122312942-f4068000-cf1d-11eb-9236-2f1645092867.png)
![image](https://user-images.githubusercontent.com/70913346/122312997-15676c00-cf1e-11eb-826e-2534045c04ca.png)

Adding information about the new car Lamborghini Urus (Click on the add button)
(Добавление информации о новой машине Lamborghini Urus(Клик по кнопки добавить))
![image](https://user-images.githubusercontent.com/70913346/122313287-9aeb1c00-cf1e-11eb-9d44-a87c61349da5.png)
![image](https://user-images.githubusercontent.com/70913346/122313337-b3f3cd00-cf1e-11eb-8dd7-9e50595ba0b6.png)

(Daf 95 vehicle information change (Click on Daf 95))
Изменение информации автомобиля Daf 95 (Клик по Daf 95)
![image](https://user-images.githubusercontent.com/70913346/122313463-ef8e9700-cf1e-11eb-9fcc-4db54b73dd4d.png)
![image](https://user-images.githubusercontent.com/70913346/122313539-0c2acf00-cf1f-11eb-887a-aac85ab93bf3.png)

## Credits
BDAuto here was coded me KritX.
(BDAuto здесь был закодирован мной KritX.)

## License
Code MIT License: https://kritxx.mit-license.org/.

## Logo
Copyright 2015, Dmitriy Kritchenkov. Licensed to BDAuto Here under the MDH contributor license agreement.

## Other images
Creative Commons Attribution 3.0 Unported (CC BY 3.0) License
