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
    public partial class frmMenuAuth : Form
    {
        MenuDAC db = new MenuDAC();
        DataTable dtMenuAuth = null;
        int sel_menu_id = 0;

        public frmMenuAuth()
        {
            InitializeComponent();
        }

        private void frmMenuAuth_Load(object sender, EventArgs e)
        {
            MenuBinding(); //트리뷰컨트롤에 메뉴를 바인딩
            AuthBinding(); //리스트박스컨트롤에 모든 권한을 바인딩
            MenuAuthBinding(); //모든 메뉴의 권한정보를 조회
        }

        private void MenuAuthBinding()
        {
            dtMenuAuth = db.GetMenuAuthList();
        }

        private void AuthBinding()
        {
            DataTable dtAuth = db.GetAuthList();

            lstAll.Items.Clear();
            foreach(DataRow dr in dtAuth.Rows)
            {
                lstAll.Items.Add($"{dr["auth_id"].ToString()}|{dr["auth_name"].ToString()}");
            }
        }

        private void MenuBinding()
        {
            DataTable dtMenu = db.GetMenuList();

            DataView dv1 = new DataView(dtMenu);
            dv1.RowFilter = "menu_level=1";
            dv1.Sort = "menu_sort";
            for(int i=0; i<dv1.Count; i++)
            {
                TreeNode node = new TreeNode(dv1[i]["menu_name"].ToString());
                node.Tag = $"{dv1[i]["menu_level"].ToString()}|{dv1[i]["menu_id"].ToString()}";
                tvMenu.Nodes.Add(node);

                DataView dv2 = new DataView(dtMenu);
                dv2.RowFilter = "menu_level=2 and pnt_menu_id=" + dv1[i]["menu_id"].ToString();
                dv2.Sort = "menu_sort";

                for(int k=0; k<dv2.Count; k++)
                {
                    TreeNode c_node = new TreeNode(dv2[k]["menu_name"].ToString());
                    c_node.Tag = $"{dv2[k]["menu_level"].ToString()}|{dv2[k]["menu_id"].ToString()}";
                    node.Nodes.Add(c_node);
                }
            }

            tvMenu.ExpandAll();
        }

        private void tvMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //2레벨의 노드일때만 메뉴권한을 셋팅
            string[] arrMenu = e.Node.Tag.ToString().Split('|');
            if (arrMenu.Length != 2) return;

            if (arrMenu[0] == "1")
            {
                lstSelect.Items.Clear();
                lstSelect.Enabled = lstAll.Enabled = false;
            }
            else
            {
                sel_menu_id = int.Parse(arrMenu[1]);
                e.Node.BackColor = Color.Yellow;

                //해당 메뉴의 메뉴권한목록을 보여준다.
                DataTable dt = dtMenuAuth.Copy();
                DataRow[] rows = dt.Select("menu_id=" + sel_menu_id);
                lstSelect.Items.Clear();
                foreach(DataRow dr in rows)
                {
                    lstSelect.Items.Add($"{dr["auth_id"].ToString()}|{dr["auth_name"].ToString()}");
                }

                lstSelect.Enabled = lstAll.Enabled = true;
            }
        }

        private void tvMenu_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //foreach(TreeNode node in tvMenu.Nodes)
            //{
            //    foreach(TreeNode c_node in node.Nodes)
            //    {
            //        c_node.BackColor = Color.White;
            //    }
            //}

            foreach (TreeNode node in tvMenu.Nodes)
            {
                ClearRecursive(node);
            }
        }

        private void ClearRecursive(TreeNode node)
        {
            foreach (TreeNode c_node in node.Nodes)
            {
                c_node.BackColor = Color.White;
                ClearRecursive(c_node);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //lstAll의 선택된 아이템이 기존에 없다면 lstSelect에 아이템 추가 
            foreach (var item in lstAll.SelectedItems)
            {
                if (lstSelect.Items.IndexOf(item.ToString()) < 0)
                {
                    lstSelect.Items.Add(item.ToString());
                }
            }

            //lstAll의 선택된 아이템의 선택취소
            for (int i=0; i<lstAll.Items.Count; i++)
            {
                lstAll.SetSelected(i, false);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            //lstSelect에서 선택된 아이템을 삭제
            lstSelect.Items.Remove(lstSelect.SelectedItem);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<int> authList = new List<int>();
            foreach (var item in lstSelect.Items)
            {
                string[] temp = item.ToString().Split('|');
                if (temp.Length == 2)
                {
                    authList.Add(int.Parse(temp[0]));
                }
            }

            bool bResult = db.SaveMenuAuth(sel_menu_id, authList);
            if (bResult)
            {
                MessageBox.Show("저장 완료되었습니다.");
                MenuAuthBinding();
            }
            else
            {
                MessageBox.Show("다시 저장하여 주십시오.");
            }
        }
    }
}
