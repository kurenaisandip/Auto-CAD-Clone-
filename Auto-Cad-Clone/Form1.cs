using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Cad_Clone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Auto Cad Clone";
        }

        private Button btn = new Button();

        public Button Btn
        {
            get => btn;
            set => btn = value;
        }
    }
}