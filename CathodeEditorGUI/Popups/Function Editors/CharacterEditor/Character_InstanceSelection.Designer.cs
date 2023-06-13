namespace CommandsEditor.Popups.Function_Editors.CharacterEditor
{
    partial class Character_InstanceSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Character_InstanceSelection));
            this.addCharacter = new System.Windows.Forms.Button();
            this.characterInstances = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // addCharacter
            // 
            this.addCharacter.Location = new System.Drawing.Point(764, 11);
            this.addCharacter.Name = "addCharacter";
            this.addCharacter.Size = new System.Drawing.Size(88, 23);
            this.addCharacter.TabIndex = 22;
            this.addCharacter.Text = "Add";
            this.addCharacter.UseVisualStyleBackColor = true;
            this.addCharacter.Click += new System.EventHandler(this.addCharacter_Click);
            // 
            // characterInstances
            // 
            this.characterInstances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.characterInstances.FormattingEnabled = true;
            this.characterInstances.Location = new System.Drawing.Point(12, 12);
            this.characterInstances.Name = "characterInstances";
            this.characterInstances.Size = new System.Drawing.Size(746, 21);
            this.characterInstances.TabIndex = 21;
            // 
            // Character_InstanceSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 45);
            this.Controls.Add(this.addCharacter);
            this.Controls.Add(this.characterInstances);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Character_InstanceSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add New Character Definition";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addCharacter;
        private System.Windows.Forms.ComboBox characterInstances;
    }
}