namespace CommandsEditor.ConfigEditors
{
    partial class SenseType
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
            this.DistanceEffectLower = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.DistanceEffectUpper = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.Light_meter_dark_level = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.positional_accuracy_scalar = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.last_sensed_expire_time = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.min_activated_time = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.decay_per_second = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.trace_threshold = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.activation_threshold = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lower_threshold = new System.Windows.Forms.TextBox();
            this.upper_threshold = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.combined_sense_activation_scalar = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.combined_sense_min_raw_activation = new System.Windows.Forms.TextBox();
            this.combined_sense_max_raw_activation = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.activation_scalar = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.min_raw_activation = new System.Windows.Forms.TextBox();
            this.max_raw_activation = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DistanceEffectLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DistanceEffectUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Light_meter_dark_level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // DistanceEffectLower
            // 
            this.DistanceEffectLower.DecimalPlaces = 3;
            this.DistanceEffectLower.Location = new System.Drawing.Point(5, 18);
            this.DistanceEffectLower.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.DistanceEffectLower.Name = "DistanceEffectLower";
            this.DistanceEffectLower.Size = new System.Drawing.Size(48, 20);
            this.DistanceEffectLower.TabIndex = 496;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(219, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 13);
            this.label11.TabIndex = 494;
            this.label11.Text = "Activation Scalar";
            // 
            // DistanceEffectUpper
            // 
            this.DistanceEffectUpper.DecimalPlaces = 3;
            this.DistanceEffectUpper.Location = new System.Drawing.Point(142, 18);
            this.DistanceEffectUpper.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.DistanceEffectUpper.Name = "DistanceEffectUpper";
            this.DistanceEffectUpper.Size = new System.Drawing.Size(48, 20);
            this.DistanceEffectUpper.TabIndex = 497;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(59, 21);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 13);
            this.label18.TabIndex = 495;
            this.label18.Text = "Min -------> Max";
            // 
            // Light_meter_dark_level
            // 
            this.Light_meter_dark_level.DecimalPlaces = 3;
            this.Light_meter_dark_level.Location = new System.Drawing.Point(222, 18);
            this.Light_meter_dark_level.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Light_meter_dark_level.Name = "Light_meter_dark_level";
            this.Light_meter_dark_level.Size = new System.Drawing.Size(185, 20);
            this.Light_meter_dark_level.TabIndex = 498;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(3, 2);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(79, 13);
            this.label29.TabIndex = 493;
            this.label29.Text = "Raw Activation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(118, 276);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 499;
            this.label1.Text = "Trace Threshold";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 3;
            this.numericUpDown1.Location = new System.Drawing.Point(121, 292);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(185, 20);
            this.numericUpDown1.TabIndex = 500;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(724, 503);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 13);
            this.label8.TabIndex = 569;
            this.label8.Text = "Positional Accuracy Scalar";
            // 
            // positional_accuracy_scalar
            // 
            this.positional_accuracy_scalar.Enabled = false;
            this.positional_accuracy_scalar.Location = new System.Drawing.Point(726, 519);
            this.positional_accuracy_scalar.Name = "positional_accuracy_scalar";
            this.positional_accuracy_scalar.Size = new System.Drawing.Size(187, 20);
            this.positional_accuracy_scalar.TabIndex = 570;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(926, 463);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 13);
            this.label7.TabIndex = 567;
            this.label7.Text = "Last Sensed Expire Time";
            // 
            // last_sensed_expire_time
            // 
            this.last_sensed_expire_time.Enabled = false;
            this.last_sensed_expire_time.Location = new System.Drawing.Point(929, 479);
            this.last_sensed_expire_time.Name = "last_sensed_expire_time";
            this.last_sensed_expire_time.Size = new System.Drawing.Size(187, 20);
            this.last_sensed_expire_time.TabIndex = 568;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(724, 463);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 565;
            this.label6.Text = "Min Activated Time";
            // 
            // min_activated_time
            // 
            this.min_activated_time.Enabled = false;
            this.min_activated_time.Location = new System.Drawing.Point(727, 479);
            this.min_activated_time.Name = "min_activated_time";
            this.min_activated_time.Size = new System.Drawing.Size(187, 20);
            this.min_activated_time.TabIndex = 566;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(724, 422);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 563;
            this.label5.Text = "Decay Per Second";
            // 
            // decay_per_second
            // 
            this.decay_per_second.Enabled = false;
            this.decay_per_second.Location = new System.Drawing.Point(727, 438);
            this.decay_per_second.Name = "decay_per_second";
            this.decay_per_second.Size = new System.Drawing.Size(187, 20);
            this.decay_per_second.TabIndex = 564;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(926, 422);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 561;
            this.label4.Text = "Trace Threshold";
            // 
            // trace_threshold
            // 
            this.trace_threshold.Enabled = false;
            this.trace_threshold.Location = new System.Drawing.Point(929, 438);
            this.trace_threshold.Name = "trace_threshold";
            this.trace_threshold.Size = new System.Drawing.Size(187, 20);
            this.trace_threshold.TabIndex = 562;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(925, 381);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 559;
            this.label2.Text = "Activation Threshold";
            // 
            // activation_threshold
            // 
            this.activation_threshold.Enabled = false;
            this.activation_threshold.Location = new System.Drawing.Point(928, 397);
            this.activation_threshold.Name = "activation_threshold";
            this.activation_threshold.Size = new System.Drawing.Size(187, 20);
            this.activation_threshold.TabIndex = 560;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(723, 381);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 555;
            this.label3.Text = "Threshold";
            // 
            // lower_threshold
            // 
            this.lower_threshold.Enabled = false;
            this.lower_threshold.Location = new System.Drawing.Point(727, 397);
            this.lower_threshold.Name = "lower_threshold";
            this.lower_threshold.Size = new System.Drawing.Size(49, 20);
            this.lower_threshold.TabIndex = 556;
            // 
            // upper_threshold
            // 
            this.upper_threshold.Enabled = false;
            this.upper_threshold.Location = new System.Drawing.Point(864, 397);
            this.upper_threshold.Name = "upper_threshold";
            this.upper_threshold.Size = new System.Drawing.Size(49, 20);
            this.upper_threshold.TabIndex = 557;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(779, 400);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 558;
            this.label9.Text = "Lower --> Upper";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(924, 340);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(170, 13);
            this.label21.TabIndex = 553;
            this.label21.Text = "Combined Sense Activation Scalar";
            // 
            // combined_sense_activation_scalar
            // 
            this.combined_sense_activation_scalar.Enabled = false;
            this.combined_sense_activation_scalar.Location = new System.Drawing.Point(927, 356);
            this.combined_sense_activation_scalar.Name = "combined_sense_activation_scalar";
            this.combined_sense_activation_scalar.Size = new System.Drawing.Size(187, 20);
            this.combined_sense_activation_scalar.TabIndex = 554;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(722, 340);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(162, 13);
            this.label22.TabIndex = 549;
            this.label22.Text = "Combined Sense Raw Activation";
            // 
            // combined_sense_min_raw_activation
            // 
            this.combined_sense_min_raw_activation.Enabled = false;
            this.combined_sense_min_raw_activation.Location = new System.Drawing.Point(726, 356);
            this.combined_sense_min_raw_activation.Name = "combined_sense_min_raw_activation";
            this.combined_sense_min_raw_activation.Size = new System.Drawing.Size(49, 20);
            this.combined_sense_min_raw_activation.TabIndex = 550;
            // 
            // combined_sense_max_raw_activation
            // 
            this.combined_sense_max_raw_activation.Enabled = false;
            this.combined_sense_max_raw_activation.Location = new System.Drawing.Point(863, 356);
            this.combined_sense_max_raw_activation.Name = "combined_sense_max_raw_activation";
            this.combined_sense_max_raw_activation.Size = new System.Drawing.Size(49, 20);
            this.combined_sense_max_raw_activation.TabIndex = 551;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(781, 359);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(77, 13);
            this.label26.TabIndex = 552;
            this.label26.Text = "Min -------> Max";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(924, 299);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(87, 13);
            this.label24.TabIndex = 547;
            this.label24.Text = "Activation Scalar";
            // 
            // activation_scalar
            // 
            this.activation_scalar.Enabled = false;
            this.activation_scalar.Location = new System.Drawing.Point(927, 315);
            this.activation_scalar.Name = "activation_scalar";
            this.activation_scalar.Size = new System.Drawing.Size(187, 20);
            this.activation_scalar.TabIndex = 548;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(722, 299);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(79, 13);
            this.label23.TabIndex = 543;
            this.label23.Text = "Raw Activation";
            // 
            // min_raw_activation
            // 
            this.min_raw_activation.Enabled = false;
            this.min_raw_activation.Location = new System.Drawing.Point(726, 315);
            this.min_raw_activation.Name = "min_raw_activation";
            this.min_raw_activation.Size = new System.Drawing.Size(49, 20);
            this.min_raw_activation.TabIndex = 544;
            // 
            // max_raw_activation
            // 
            this.max_raw_activation.Enabled = false;
            this.max_raw_activation.Location = new System.Drawing.Point(863, 315);
            this.max_raw_activation.Name = "max_raw_activation";
            this.max_raw_activation.Size = new System.Drawing.Size(49, 20);
            this.max_raw_activation.TabIndex = 545;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(781, 318);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(77, 13);
            this.label25.TabIndex = 546;
            this.label25.Text = "Min -------> Max";
            // 
            // SenseType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.positional_accuracy_scalar);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.last_sensed_expire_time);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.min_activated_time);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.decay_per_second);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trace_threshold);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.activation_threshold);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lower_threshold);
            this.Controls.Add(this.upper_threshold);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.combined_sense_activation_scalar);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.combined_sense_min_raw_activation);
            this.Controls.Add(this.combined_sense_max_raw_activation);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.activation_scalar);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.min_raw_activation);
            this.Controls.Add(this.max_raw_activation);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.DistanceEffectLower);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.DistanceEffectUpper);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.Light_meter_dark_level);
            this.Controls.Add(this.label29);
            this.Name = "SenseType";
            this.Size = new System.Drawing.Size(1318, 828);
            ((System.ComponentModel.ISupportInitialize)(this.DistanceEffectLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DistanceEffectUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Light_meter_dark_level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown DistanceEffectLower;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown DistanceEffectUpper;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown Light_meter_dark_level;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox positional_accuracy_scalar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox last_sensed_expire_time;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox min_activated_time;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox decay_per_second;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox trace_threshold;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox activation_threshold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox lower_threshold;
        private System.Windows.Forms.TextBox upper_threshold;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox combined_sense_activation_scalar;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox combined_sense_min_raw_activation;
        private System.Windows.Forms.TextBox combined_sense_max_raw_activation;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox activation_scalar;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox min_raw_activation;
        private System.Windows.Forms.TextBox max_raw_activation;
        private System.Windows.Forms.Label label25;
    }
}
