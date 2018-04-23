namespace Sample
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.customGridControl1 = new Sample.CustomGridControl();
            this.customGridView1 = new Sample.CustomGridView();
            this.customGridColumn1 = new Sample.CustomGridColumn();
            this.customGridColumn2 = new Sample.CustomGridColumn();
            this.customGridColumn3 = new Sample.CustomGridColumn();
            this.customGridColumn4 = new Sample.CustomGridColumn();
            this.customGridColumn5 = new Sample.CustomGridColumn();
            this.customGridColumn6 = new Sample.CustomGridColumn();
            this.customGridColumn7 = new Sample.CustomGridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.customGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // customGridControl1
            // 
            this.customGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.customGridControl1.Location = new System.Drawing.Point(0, 0);
            this.customGridControl1.MainView = this.customGridView1;
            this.customGridControl1.Margin = new System.Windows.Forms.Padding(2);
            this.customGridControl1.Name = "customGridControl1";
            this.customGridControl1.Size = new System.Drawing.Size(808, 306);
            this.customGridControl1.TabIndex = 0;
            this.customGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.customGridView1});
            // 
            // customGridView1
            // 
            this.customGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.customGridColumn1,
            this.customGridColumn2,
            this.customGridColumn3,
            this.customGridColumn4,
            this.customGridColumn5,
            this.customGridColumn6,
            this.customGridColumn7});
            this.customGridView1.GridControl = this.customGridControl1;
            this.customGridView1.Name = "customGridView1";
            this.customGridView1.OptionsCustomization.QuickCustomizationIcons.Image = ((System.Drawing.Image)(resources.GetObject("customGridView1.OptionsCustomization.QuickCustomizationIcons.Image")));
            this.customGridView1.OptionsCustomization.QuickCustomizationIcons.TransperentColor = System.Drawing.Color.White;
            // 
            // customGridColumn1
            // 
            this.customGridColumn1.Caption = "customGridColumn1";
            this.customGridColumn1.Name = "customGridColumn1";
            this.customGridColumn1.Visible = true;
            this.customGridColumn1.VisibleIndex = 0;
            // 
            // customGridColumn2
            // 
            this.customGridColumn2.Caption = "customGridColumn2";
            this.customGridColumn2.Name = "customGridColumn2";
            this.customGridColumn2.Visible = true;
            this.customGridColumn2.VisibleIndex = 1;
            // 
            // customGridColumn3
            // 
            this.customGridColumn3.Caption = "customGridColumn3";
            this.customGridColumn3.Name = "customGridColumn3";
            this.customGridColumn3.Visible = true;
            this.customGridColumn3.VisibleIndex = 2;
            // 
            // customGridColumn4
            // 
            this.customGridColumn4.Caption = "No move";
            this.customGridColumn4.Name = "customGridColumn4";
            this.customGridColumn4.OptionsColumn.AllowMove = false;
            this.customGridColumn4.Visible = true;
            this.customGridColumn4.VisibleIndex = 3;
            // 
            // customGridColumn5
            // 
            this.customGridColumn5.Caption = "No hide";
            this.customGridColumn5.Name = "customGridColumn5";
            this.customGridColumn5.OptionsColumn.AllowQuickHide = false;
            this.customGridColumn5.Visible = true;
            this.customGridColumn5.VisibleIndex = 4;
            // 
            // customGridColumn6
            // 
            this.customGridColumn6.Caption = "No move No hide";
            this.customGridColumn6.Name = "customGridColumn6";
            this.customGridColumn6.OptionsColumn.AllowMove = false;
            this.customGridColumn6.OptionsColumn.AllowQuickHide = false;
            this.customGridColumn6.Visible = true;
            this.customGridColumn6.VisibleIndex = 5;
            // 
            // customGridColumn7
            // 
            this.customGridColumn7.Caption = "Dont show in form";
            this.customGridColumn7.Name = "customGridColumn7";
            this.customGridColumn7.OptionsColumn.ShowInCustomizationForm = false;
            this.customGridColumn7.Visible = true;
            this.customGridColumn7.VisibleIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 306);
            this.Controls.Add(this.customGridControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.customGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomGridControl customGridControl1;
        private CustomGridView customGridView1;
        private CustomGridColumn customGridColumn1;
        private CustomGridColumn customGridColumn2;
        private CustomGridColumn customGridColumn3;
        private CustomGridColumn customGridColumn4;
        private CustomGridColumn customGridColumn5;
        private CustomGridColumn customGridColumn6;
        private CustomGridColumn customGridColumn7;


    }
}

