using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Formulyar
{
    /// <summary>
    /// Логика взаимодействия для WaitWindow.xaml
    /// </summary>
    public partial class WaitWindow : Window
    {
        public WaitWindow()
        {
            InitializeComponent();
            
            // инициализируем коллекцию
            //OnLoadItems = new ObservableCollection<string>();
            //progress.IsIndeterminate = true;
            // подписываемся на событие загрузки
            // можно и в XAML подписаться, сути дела не меняет
            //Loaded += MainWindow_Loaded;
            // сам себе источник данных
            DataContext = this;
        }
        async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            //for (int i = 10; i >= 0; i--)
            //{
            //    // Эмулируем задержку в 2 секунды на получение каждого следующего числа
            //    await Task.Delay(2000);
            //    // добавляем элемент в коллекцию
            //    OnLoadItems.Add(i.ToString());
            //}
            //await Task.Delay(1000); // и тут ещё секунду что-то происходит
            //OnLoadItems.Add("Всё загружено");
           
            //// скрываем полосу прогресса
            //progress.Visibility = System.Windows.Visibility.Hidden;
        }

        public ObservableCollection<string> OnLoadItems { get; private set; }
    }

}
