
namespace MenuAuthMgt
{
    partial class frmMenuAuth
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tvMenu = new System.Windows.Forms.TreeView();
            this.lstAll = new System.Windows.Forms.ListBox();
            this.lstSelect = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tvMenu
            // 
            this.tvMenu.Location = new System.Drawing.Point(13, 13);
            this.tvMenu.Name = "tvMenu";
            this.tvMenu.Size = new System.Drawing.Size(209, 376);
            this.tvMenu.TabIndex = 0;
            this.tvMenu.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvMenu_BeforeSelect);
            this.tvMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMenu_AfterSelect);
            // 
            // lstAll
            // 
            this.lstAll.FormattingEnabled = true;
            this.lstAll.ItemHeight = 12;
            this.lstAll.Location = new System.Drawing.Point(588, 13);
            this.lstAll.Name = "lstAll";
            this.lstAll.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAll.Size = new System.Drawing.Size(200, 232);
            this.lstAll.TabIndex = 1;
            // 
            // lstSelect
            // 
            this.lstSelect.FormattingEnabled = true;
            this.lstSelect.ItemHeight = 12;
            this.lstSelect.Location = new System.Drawing.Point(269, 13);
            this.lstSelect.Name = "lstSelect";
            this.lstSelect.Size = new System.Drawing.Size(220, 232);
            this.lstSelect.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(304, 338);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(152, 35);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd.Location = new System.Drawing.Point(517, 69);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 30);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "<-";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDel.Location = new System.Drawing.Point(517, 118);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(48, 30);
            this.btnDel.TabIndex = 5;
            this.btnDel.Text = "->";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // frmMenuAuth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 402);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lstSelect);
            this.Controls.Add(this.lstAll);
            this.Controls.Add(this.tvMenu);
            this.Name = "frmMenuAuth";
            this.Text = "frmMenuAuth";
            this.Load += new System.EventHandler(this.frmMenuAuth_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvMenu;
        private System.Windows.Forms.ListBox lstAll;
        private System.Windows.Forms.ListBox lstSelect;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
    }
}