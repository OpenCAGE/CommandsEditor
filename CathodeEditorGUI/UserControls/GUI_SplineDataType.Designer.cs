namespace CommandsEditor.UserControls
{
    partial class GUI_SplineDataType
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SPLINE_CONTAINER = new System.Windows.Forms.GroupBox();
            this.openSplineEditor = new System.Windows.Forms.Button();
            this.SPLINE_CONTAINER.SuspendLayout();
            this.SuspendLayout();
            // 
            // SPLINE_CONTAINER
            // 
            this.SPLINE_CONTAINER.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SPLINE_CONTAINER.Controls.Add(this.openSplineEditor);
            this.SPLINE_CONTAINER.Location = new System.Drawing.Point(3, 3);
            this.SPLINE_CONTAINER.Name = "SPLINE_CONTAINER";
            this.SPLINE_CONTAINER.Size = new System.Drawing.Size(334, 56);
            this.SPLINE_CONTAINER.TabIndex = 18;
            this.SPLINE_CONTAINER.TabStop = false;
            this.SPLINE_CONTAINER.Text = "Parameter Name (00-00-00-00)";
            // 
            // openSplineEditor
            // 
            this.openSplineEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.openSplineEditor.Location = new System.Drawing.Point(17, 21);
            this.openSplineEditor.Name = "openSplineEditor";
            this.openSplineEditor.Size = new System.Drawing.Size(304, 23);
            this.openSplineEditor.TabIndex = 1;
            this.openSplineEditor.Text = "Edit Spline";
            this.openSplineEditor.UseVisualStyleBackColor = true;
            this.openSplineEditor.Click += new System.EventHandler(this.openSplineEditor_Click);
            // 
            // GUI_SplineDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SPLINE_CONTAINER);
            this.DoubleBuffered = true;
            this.Name = "GUI_SplineDataType";
            this.Size = new System.Drawing.Size(340, 61);
            this.SPLINE_CONTAINER.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SPLINE_CONTAINER;
        private System.Windows.Forms.Button openSplineEditor;
    }
}
