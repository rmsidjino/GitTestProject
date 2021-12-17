using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuAuthMgt
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //로그인을 위해서 아디이와 비밀번호로 사용자정보 체크
            //사용자정보가 적합하면 사용자 정보를 조회
            this.Hide();
            frnMain main = new frnMain(textBox1.Text.Trim());
            main.Show();
        }
    }
}
