namespace Monaden.Cont.Beispiel
{
    partial class Main
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
            this.Aktivieren = new System.Windows.Forms.Button();
            this.eingabeGruppe = new System.Windows.Forms.GroupBox();
            this.Ok = new System.Windows.Forms.Button();
            this.Nachricht = new System.Windows.Forms.TextBox();
            this.eingabeGruppe.SuspendLayout();
            this.SuspendLayout();
            // 
            // Aktivieren
            // 
            this.Aktivieren.Location = new System.Drawing.Point(42, 35);
            this.Aktivieren.Name = "Aktivieren";
            this.Aktivieren.Size = new System.Drawing.Size(75, 23);
            this.Aktivieren.TabIndex = 0;
            this.Aktivieren.Text = "aktivieren";
            this.Aktivieren.UseVisualStyleBackColor = true;
            // 
            // eingabeGruppe
            // 
            this.eingabeGruppe.Controls.Add(this.Nachricht);
            this.eingabeGruppe.Controls.Add(this.Ok);
            this.eingabeGruppe.Location = new System.Drawing.Point(42, 86);
            this.eingabeGruppe.Name = "eingabeGruppe";
            this.eingabeGruppe.Size = new System.Drawing.Size(200, 50);
            this.eingabeGruppe.TabIndex = 1;
            this.eingabeGruppe.TabStop = false;
            this.eingabeGruppe.Text = "Eingabe";
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(167, 17);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(27, 23);
            this.Ok.TabIndex = 0;
            this.Ok.Text = "ok";
            this.Ok.UseVisualStyleBackColor = true;
            // 
            // Nachricht
            // 
            this.Nachricht.Location = new System.Drawing.Point(6, 19);
            this.Nachricht.Name = "Nachricht";
            this.Nachricht.Size = new System.Drawing.Size(155, 20);
            this.Nachricht.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.eingabeGruppe);
            this.Controls.Add(this.Aktivieren);
            this.Name = "Main";
            this.Text = "ContM Beispiel";
            this.Load += new System.EventHandler(this.Main_Load);
            this.eingabeGruppe.ResumeLayout(false);
            this.eingabeGruppe.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Aktivieren;
        private System.Windows.Forms.GroupBox eingabeGruppe;
        private System.Windows.Forms.TextBox Nachricht;
        private System.Windows.Forms.Button Ok;
    }
}

