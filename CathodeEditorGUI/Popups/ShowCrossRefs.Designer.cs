namespace CommandsEditor
{
    partial class ShowCrossRefs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowCrossRefs));
            this.overridesUI = new System.Windows.Forms.ListBox();
            this.proxiesUI = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.jumpToOverride = new System.Windows.Forms.Button();
            this.jumpToProxy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // overridesUI
            // 
            this.overridesUI.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overridesUI.FormattingEnabled = true;
            this.overridesUI.HorizontalScrollbar = true;
            this.overridesUI.Location = new System.Drawing.Point(12, 29);
            this.overridesUI.Name = "overridesUI";
            this.overridesUI.Size = new System.Drawing.Size(444, 381);
            this.overridesUI.TabIndex = 145;
            // 
            // proxiesUI
            // 
            this.proxiesUI.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.proxiesUI.FormattingEnabled = true;
            this.proxiesUI.HorizontalScrollbar = true;
            this.proxiesUI.Location = new System.Drawing.Point(462, 29);
            this.proxiesUI.Name = "proxiesUI";
            this.proxiesUI.Size = new System.Drawing.Size(444, 381);
            this.proxiesUI.TabIndex = 146;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 147;
            this.label1.Text = "Overrides pointing to this entity:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(459, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 148;
            this.label2.Text = "Proxies pointing to this entity:";
            // 
            // jumpToOverride
            // 
            this.jumpToOverride.Location = new System.Drawing.Point(320, 416);
            this.jumpToOverride.Name = "jumpToOverride";
            this.jumpToOverride.Size = new System.Drawing.Size(136, 23);
            this.jumpToOverride.TabIndex = 149;
            this.jumpToOverride.Text = "Jump To Override";
            this.jumpToOverride.UseVisualStyleBackColor = true;
            this.jumpToOverride.Click += new System.EventHandler(this.jumpToOverride_Click);
            // 
            // jumpToProxy
            // 
            this.jumpToProxy.Location = new System.Drawing.Point(770, 416);
            this.jumpToProxy.Name = "jumpToProxy";
            this.jumpToProxy.Size = new System.Drawing.Size(136, 23);
            this.jumpToProxy.TabIndex = 150;
            this.jumpToProxy.Text = "Jump To Proxy";
            this.jumpToProxy.UseVisualStyleBackColor = true;
            this.jumpToProxy.Click += new System.EventHandler(this.jumpToProxy_Click);
            // 
            // CathodeEditorGUI_ShowCrossRefs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 448);
            this.Controls.Add(this.jumpToProxy);
            this.Controls.Add(this.jumpToOverride);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.proxiesUI);
            this.Controls.Add(this.overridesUI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CathodeEditorGUI_ShowCrossRefs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Linked Overrides and Proxies";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox overridesUI;
        private System.Windows.Forms.ListBox proxiesUI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button jumpToOverride;
        private System.Windows.Forms.Button jumpToProxy;
    }
}