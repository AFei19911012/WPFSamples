using Demos.Entity;
using SqlSugar;
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
    /// SqlSugarSqliteDemo.xaml 的交互逻辑
    /// </summary>
    public partial class SqlSugarSqliteDemo : UserControl
    {
        public SqlSugarSqliteDemo()
        {
            InitializeComponent();

            Init();
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <returns></returns>
        private ISqlSugarClient CreateDB()
        {
            var connectionString = "Data Source=DemoDB.db";
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,//连接符字串
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true
            });
            return db;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private void Init()
        {
            var db = CreateDB();
            db.Ado.ExecuteCommand("VACUUM");
            db.CodeFirst.InitTables(typeof(Table_Demo));

            // 查询数据
            var list = db.Queryable<Table_Demo>().ToList();
            // 如果没有数据则初始化数据
            if (list.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    db.Insertable(new Table_Demo()
                    {
                        Uid = Guid.NewGuid().ToString(),
                        Name = "default",
                        Remark = "",
                        IsChecked = false,
                    }).ExecuteCommand();
                }
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Table_Demo data = new Table_Demo
            {
                Uid = Guid.NewGuid().ToString(),
                Name = "new",
                IsChecked = true,
            };
            var db = CreateDB();
            db.Insertable(data).ExecuteCommand();
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var db = CreateDB();
            var data = db.Queryable<Table_Demo>().Where(p => p.Name == "new").First();
            db.Deleteable(data).ExecuteCommand();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var db = CreateDB();
            var data = db.Queryable<Table_Demo>().Where(p => p.Name == "new").First();
            data.Remark = "Updated";
            db.Updateable(data).ExecuteCommand();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            var db = CreateDB();
            var list = db.Queryable<Table_Demo>().ToList();
            MessageBox.Show(string.Format("现有数据 {0} 条", list.Count));
        }
    }
}