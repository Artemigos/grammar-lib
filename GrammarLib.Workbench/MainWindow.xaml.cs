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

using GrammarLib.GrammarLang;

namespace GrammarLib.Workbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Parse_OnClick(object sender, RoutedEventArgs e)
        {
            var gg = Grammar.Create();
            var rast = gg.Parse(Lang.Text);
            var reader = new Reader();
            var rg = reader.Read(rast);

            var r = rg.Parse(Test.Text);
            var toStr = new ToStringVisitor<string, string>();
            var str = toStr.BuildString(r);

            MessageBox.Show(str, "AST");
        }
    }
}
