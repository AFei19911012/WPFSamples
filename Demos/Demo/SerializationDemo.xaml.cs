using Demos.Helper;
using Demos.Model;
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

namespace Demos.Demo
{
    /// <summary>
    /// SerializationDemo.xaml 的交互逻辑
    /// </summary>
    public partial class SerializationDemo : UserControl
    {
        public SerializationDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Content.ToString();
            DataModel data = new DataModel()
            {
                Name = "Big Wang",
                Content = "Content",
                InspectResult = "Nothing"
            };

            if (name.StartsWith("OjbectToBinary"))
            {
                string filename = "data/binary.model";
                SerializeHelper.ObjectToBinary(data, filename);
            }
            else if (name.StartsWith("OjbectToJson"))
            {
                string filename = "data/json.model";
                SerializeHelper.ObjectToJson(data, filename);
            }
            else if (name.StartsWith("OjbectToXml"))
            {
                string filename = "data/xml.model";
                SerializeHelper.ObjectToXml(data, filename);
            }
            else if (name.StartsWith("BinaryToObject"))
            {
                string filename = "data/binary.model";
                var obj = SerializeHelper.BinaryToObject<DataModel>(filename);
                MessageBox.Show(obj.Name);
            }
            else if (name.StartsWith("JsonToObject"))
            {
                string filename = "data/json.model";
                var obj = SerializeHelper.JsonToObject<DataModel>(filename);
                MessageBox.Show(obj.Name);
            }
            else if (name.StartsWith("XmlToObject"))
            {
                string filename = "data/xml.model";
                var obj = SerializeHelper.XmlToObject<DataModel>(filename);
                MessageBox.Show(obj.Name);
            }
        }
    }
}