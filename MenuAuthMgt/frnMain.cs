using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace MenuAuthMgt
{
    public partial class frnMain : Form
    {
        string userID;
        DataTable dtMenu;
        Panel panel1;
        TreeView treeView1;
        Button btnInit;

        public frnMain(string userID)
        {
            InitializeComponent();
            this.userID = userID;
        }

        private void frnMain_Load(object sender, EventArgs e)
        {
            //아이디로 접근할 수 있는 메뉴 동적 바인딩
            MenuDAC db = new MenuDAC();
            dtMenu = db.GetUserMenuList(this.userID);

            DrawMenuStrip();
            DrawMenuPanel();

            btnInit.PerformClick();
        }

        private void DrawMenuPanel()
        {
            DataView dv1 = new DataView(dtMenu);
            dv1.RowFilter = "menu_level = 1";
            dv1.Sort = "menu_sort";
            for (int i = 0; i < dv1.Count; i++)
            {
                Button p_menu = new Button();
                p_menu.Name = $"p_btn{dv1[i]["menu_id"].ToString()}";
                p_menu.Text = dv1[i]["menu_name"].ToString();
                p_menu.Dock = DockStyle.Top;
                p_menu.Location = new Point(0, 0);
                p_menu.Margin = new Padding(0);
                p_menu.Size = new System.Drawing.Size(196, 36);
                p_menu.Tag = i.ToString();
                p_menu.Click += button1_Click;

                flowLayoutPanel1.Controls.Add(p_menu);

                if (i == 0)
                {
                    btnInit = p_menu;
                }
            }

            panel1 = new Panel();
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, (dv1.Count * 40));
            panel1.Name = "panel1";
            panel1.Size = new Size(193, 300);
            flowLayoutPanel1.Controls.Add(this.panel1);

            treeView1 = new TreeView();
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new System.Drawing.Size(193, 300);
            treeView1.AfterSelect += TreeView1_AfterSelect;
            panel1.Controls.Add(this.treeView1);            
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OpenCreateForm(e.Node.Tag.ToString(), e.Node.Text);
        }

        private void DrawMenuStrip()
        {
            DataView dv1 = new DataView(dtMenu);
            dv1.RowFilter = "menu_level = 1";
            dv1.Sort = "menu_sort";
            for(int i=0; i<dv1.Count; i++)
            {
                ToolStripMenuItem p_menu = new ToolStripMenuItem();
                p_menu.Name = $"p_menu{dv1[i]["menu_id"].ToString()}";
                p_menu.Text = dv1[i]["menu_name"].ToString();
                p_menu.Size = new Size(180, 22);

                DataView dv2 = new DataView(dtMenu);
                dv2.RowFilter = "menu_level = 2 and pnt_menu_id = " + dv1[i]["pnt_menu_id"].ToString();
                dv2.Sort = "menu_sort";
                for (int k = 0; k < dv2.Count; k++)
                {
                    ToolStripMenuItem c_menu = new ToolStripMenuItem();
                    c_menu.Name = $"p_menu{dv2[k]["menu_id"].ToString()}";
                    c_menu.Text = dv2[k]["menu_name"].ToString();
                    c_menu.Tag = dv2[k]["program_name"].ToString();
                    c_menu.Size = new Size(180, 22);
                    c_menu.Click += Menu_Click;
                    p_menu.DropDownItems.Add(c_menu);
                }
                this.menuStrip1.Items.Add(p_menu);
            }
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            //MessageBox.Show(menu.Tag.ToString());
            OpenCreateForm(menu.Tag.ToString(), menu.Text);
        }

        private void OpenCreateForm(string pgmName, string formText)
        {
            string appName = Assembly.GetEntryAssembly().GetName().Name;
            Type frmType = Type.GetType($"{appName}.{pgmName}");

            foreach(Form frm in Application.OpenForms)
            {
                if (frm.GetType() == frmType)
                {
                    frm.Activate();
                    frm.BringToFront();
                    return;
                }
            }

            try
            {
                Form frm = (Form)Activator.CreateInstance(frmType);
                frm.MdiParent = this;
                frm.WindowState = FormWindowState.Maximized;
                frm.Text = formText;
                frm.Show();
            }
            catch
            {
                MessageBox.Show("등록된 프로그램이 존재하지 않습니다.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            flowLayoutPanel1.Controls.SetChildIndex(panel1, Convert.ToInt32(btn.Tag) + 1);
            flowLayoutPanel1.Invalidate();

            treeView1.Nodes.Clear();

            DataView dv2 = new DataView(dtMenu);
            dv2.RowFilter = "menu_level = 2 and pnt_menu_id = " + btn.Name.Replace("p_btn", "");
            dv2.Sort = "menu_sort";
            for (int k = 0; k < dv2.Count; k++)
            {
                TreeNode c_node = new TreeNode(dv2[k]["menu_name"].ToString());
                c_node.Tag = dv2[k]["program_name"].ToString();
                treeView1.Nodes.Add(c_node);
            }
        }

        private void frnMain_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
            {
                tabControl1.Visible = false;
            }
            else
            {
                this.ActiveMdiChild.WindowState = FormWindowState.Maximized;

                if (this.ActiveMdiChild.Tag == null)
                {
                    TabPage tp = new TabPage(this.ActiveMdiChild.Text + "   ");
                    tp.Parent = tabControl1;
                    tp.Tag = this.ActiveMdiChild;
                    tabControl1.SelectedTab = tp;

                    this.ActiveMdiChild.FormClosed += ActiveMdiChild_FormClosed;
                    this.ActiveMdiChild.Tag = tp;
                }
                else
                {
                    tabControl1.SelectedTab = (TabPage)this.ActiveMdiChild.Tag;
                }

                if (!tabControl1.Visible)
                    tabControl1.Visible = true;
            }
        }

        private void ActiveMdiChild_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((TabPage)((Form)sender).Tag).Dispose();
        }

        private void menuStrip1_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (e.Item.Text == "" || e.Item.Text == "닫기(&C)")
                e.Item.Visible = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Tag != null)
            {
                Form frm = (Form)tabControl1.SelectedTab.Tag;
                frm.Select();
            }
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                var r = tabControl1.GetTabRect(i);
                var closeImage = Properties.Resources.close_grey;
                var closeRect = new Rectangle((r.Right - closeImage.Width), r.Top + (r.Height - closeImage.Height) / 2,
                    closeImage.Width, closeImage.Height);

                if (closeRect.Contains(e.Location))
                {
                    this.ActiveMdiChild.Close();
                    break;
                }
            }
        }
    }
}
