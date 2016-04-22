using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
using IkitMita.Mvvm.Views;

namespace BookStore
{
    /// <summary>
    /// Interaction logic for CreateOrderView.xaml
    /// </summary>
    [Export("CreateOrderView", typeof(IView))]
    public partial class CreateOrderView : IView
    {
        public CreateOrderView()
        {
            InitializeComponent();
        }
    }
}
