namespace CommandsEditor
{
    partial class RadiosityEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadiosityEditor));
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gRadiosityAlbedoSaturationAmount = new System.Windows.Forms.NumericUpDown();
            this.gRadiositySpecularGlossScale = new System.Windows.Forms.NumericUpDown();
            this.gRadiosityMultiBounceScale = new System.Windows.Forms.NumericUpDown();
            this.gRadiosityEmissiveSurfaceScale = new System.Windows.Forms.NumericUpDown();
            this.gRadiosityAlbedoOverbrightAmount = new System.Windows.Forms.NumericUpDown();
            this.gRadiosityFirstBounceScale = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gDeferredEmissiveSurfaceExponent = new System.Windows.Forms.NumericUpDown();
            this.gDeferredEmissiveSurfaceScale = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityAlbedoSaturationAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiositySpecularGlossScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityMultiBounceScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityEmissiveSurfaceScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityAlbedoOverbrightAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityFirstBounceScale)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gDeferredEmissiveSurfaceExponent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gDeferredEmissiveSurfaceScale)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(485, 213);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(136, 33);
            this.btnSave.TabIndex = 410;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 414;
            this.label2.Text = "Emissive Surface Scale";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 416;
            this.label3.Text = "First Bounce Scale";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(208, 25);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(99, 13);
            this.label35.TabIndex = 418;
            this.label35.Text = "Multi Bounce Scale";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(208, 64);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(131, 13);
            this.label34.TabIndex = 420;
            this.label34.Text = "Albedo Overbright Amount";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(401, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 422;
            this.label4.Text = "Albedo Saturation Amount";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(401, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 424;
            this.label5.Text = "Specular Gloss Scale";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 13);
            this.label7.TabIndex = 427;
            this.label7.Text = "Emissive Surface Scale";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 13);
            this.label8.TabIndex = 429;
            this.label8.Text = "Emissive Surface Exponent";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gRadiosityAlbedoSaturationAmount);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.gRadiositySpecularGlossScale);
            this.groupBox2.Controls.Add(this.label34);
            this.groupBox2.Controls.Add(this.gRadiosityMultiBounceScale);
            this.groupBox2.Controls.Add(this.gRadiosityEmissiveSurfaceScale);
            this.groupBox2.Controls.Add(this.gRadiosityAlbedoOverbrightAmount);
            this.groupBox2.Controls.Add(this.label35);
            this.groupBox2.Controls.Add(this.gRadiosityFirstBounceScale);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(609, 117);
            this.groupBox2.TabIndex = 431;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Radiosity Lighting Settings";
            // 
            // gRadiosityAlbedoSaturationAmount
            // 
            this.gRadiosityAlbedoSaturationAmount.DecimalPlaces = 6;
            this.gRadiosityAlbedoSaturationAmount.Location = new System.Drawing.Point(404, 41);
            this.gRadiosityAlbedoSaturationAmount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gRadiosityAlbedoSaturationAmount.Name = "gRadiosityAlbedoSaturationAmount";
            this.gRadiosityAlbedoSaturationAmount.Size = new System.Drawing.Size(187, 20);
            this.gRadiosityAlbedoSaturationAmount.TabIndex = 440;
            // 
            // gRadiositySpecularGlossScale
            // 
            this.gRadiositySpecularGlossScale.DecimalPlaces = 6;
            this.gRadiositySpecularGlossScale.Location = new System.Drawing.Point(404, 81);
            this.gRadiositySpecularGlossScale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gRadiositySpecularGlossScale.Name = "gRadiositySpecularGlossScale";
            this.gRadiositySpecularGlossScale.Size = new System.Drawing.Size(187, 20);
            this.gRadiositySpecularGlossScale.TabIndex = 439;
            // 
            // gRadiosityMultiBounceScale
            // 
            this.gRadiosityMultiBounceScale.DecimalPlaces = 6;
            this.gRadiosityMultiBounceScale.Location = new System.Drawing.Point(211, 41);
            this.gRadiosityMultiBounceScale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gRadiosityMultiBounceScale.Name = "gRadiosityMultiBounceScale";
            this.gRadiosityMultiBounceScale.Size = new System.Drawing.Size(187, 20);
            this.gRadiosityMultiBounceScale.TabIndex = 438;
            // 
            // gRadiosityEmissiveSurfaceScale
            // 
            this.gRadiosityEmissiveSurfaceScale.DecimalPlaces = 6;
            this.gRadiosityEmissiveSurfaceScale.Location = new System.Drawing.Point(19, 41);
            this.gRadiosityEmissiveSurfaceScale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gRadiosityEmissiveSurfaceScale.Name = "gRadiosityEmissiveSurfaceScale";
            this.gRadiosityEmissiveSurfaceScale.Size = new System.Drawing.Size(187, 20);
            this.gRadiosityEmissiveSurfaceScale.TabIndex = 436;
            // 
            // gRadiosityAlbedoOverbrightAmount
            // 
            this.gRadiosityAlbedoOverbrightAmount.DecimalPlaces = 6;
            this.gRadiosityAlbedoOverbrightAmount.Location = new System.Drawing.Point(211, 81);
            this.gRadiosityAlbedoOverbrightAmount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gRadiosityAlbedoOverbrightAmount.Name = "gRadiosityAlbedoOverbrightAmount";
            this.gRadiosityAlbedoOverbrightAmount.Size = new System.Drawing.Size(187, 20);
            this.gRadiosityAlbedoOverbrightAmount.TabIndex = 437;
            // 
            // gRadiosityFirstBounceScale
            // 
            this.gRadiosityFirstBounceScale.DecimalPlaces = 6;
            this.gRadiosityFirstBounceScale.Location = new System.Drawing.Point(19, 81);
            this.gRadiosityFirstBounceScale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gRadiosityFirstBounceScale.Name = "gRadiosityFirstBounceScale";
            this.gRadiosityFirstBounceScale.Size = new System.Drawing.Size(187, 20);
            this.gRadiosityFirstBounceScale.TabIndex = 435;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gDeferredEmissiveSurfaceExponent);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.gDeferredEmissiveSurfaceScale);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(12, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 111);
            this.groupBox1.TabIndex = 432;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Deferred Lighting Settings";
            // 
            // gDeferredEmissiveSurfaceExponent
            // 
            this.gDeferredEmissiveSurfaceExponent.DecimalPlaces = 6;
            this.gDeferredEmissiveSurfaceExponent.Location = new System.Drawing.Point(19, 77);
            this.gDeferredEmissiveSurfaceExponent.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gDeferredEmissiveSurfaceExponent.Name = "gDeferredEmissiveSurfaceExponent";
            this.gDeferredEmissiveSurfaceExponent.Size = new System.Drawing.Size(187, 20);
            this.gDeferredEmissiveSurfaceExponent.TabIndex = 434;
            // 
            // gDeferredEmissiveSurfaceScale
            // 
            this.gDeferredEmissiveSurfaceScale.DecimalPlaces = 6;
            this.gDeferredEmissiveSurfaceScale.Location = new System.Drawing.Point(19, 38);
            this.gDeferredEmissiveSurfaceScale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.gDeferredEmissiveSurfaceScale.Name = "gDeferredEmissiveSurfaceScale";
            this.gDeferredEmissiveSurfaceScale.Size = new System.Drawing.Size(187, 20);
            this.gDeferredEmissiveSurfaceScale.TabIndex = 433;
            // 
            // RadiosityEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 260);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RadiosityEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Radiosity Editor";
            this.Load += new System.EventHandler(this.RadiosityEditor_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityAlbedoSaturationAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiositySpecularGlossScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityMultiBounceScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityEmissiveSurfaceScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityAlbedoOverbrightAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gRadiosityFirstBounceScale)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gDeferredEmissiveSurfaceExponent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gDeferredEmissiveSurfaceScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown gDeferredEmissiveSurfaceScale;
        private System.Windows.Forms.NumericUpDown gDeferredEmissiveSurfaceExponent;
        private System.Windows.Forms.NumericUpDown gRadiosityFirstBounceScale;
        private System.Windows.Forms.NumericUpDown gRadiosityEmissiveSurfaceScale;
        private System.Windows.Forms.NumericUpDown gRadiosityAlbedoOverbrightAmount;
        private System.Windows.Forms.NumericUpDown gRadiosityMultiBounceScale;
        private System.Windows.Forms.NumericUpDown gRadiosityAlbedoSaturationAmount;
        private System.Windows.Forms.NumericUpDown gRadiositySpecularGlossScale;
    }
}