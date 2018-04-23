using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Users myUsers = new Users();
        private void Form1_Load(object sender, EventArgs e)
        {
            myUsers.Add(new User("Antuan", "Acapulco", 23));
            myUsers.Add(new User("Bill", "Brussels", 17));
            myUsers.Add(new User("Charli", "Chicago", 45));
            myUsers.Add(new User("Denn", "Denver", 20));
            myUsers.Add(new User("Eva", "Everton", 23));
            customGridControl1.DataSource = myUsers;
            customGridColumn1.Caption = customGridColumn1.FieldName = "Name";
            customGridColumn2.Caption = customGridColumn2.FieldName = "City";
            customGridColumn3.Caption = customGridColumn3.FieldName = "Age";
        }
    }
    public class User
    {
        string name, city;
        int age;
        public User(string name, string city, int age)
        {
            this.name = name;
            this.city = city;
            this.age = age;
        }
        public int Age { set { age = value; } get { return age; } }
        public string Name { set { name = value; } get { return name; } }
        public string City { set { city = value; } get { return city; } }
    }
    public class Users : ArrayList
    {
        public override object this[int index] { get { return (User)base[index]; } set { base[index] = value; } }
    }
}