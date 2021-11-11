namespace VETRISAccountsScheduler
{
    partial class frmTestcs
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
            this.btnUpdateBA = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnAcctQuery = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUpdateBA
            // 
            this.btnUpdateBA.Location = new System.Drawing.Point(45, 29);
            this.btnUpdateBA.Name = "btnUpdateBA";
            this.btnUpdateBA.Size = new System.Drawing.Size(228, 39);
            this.btnUpdateBA.TabIndex = 0;
            this.btnUpdateBA.Text = "Update Billing Account";
            this.btnUpdateBA.UseVisualStyleBackColor = true;
            this.btnUpdateBA.Click += new System.EventHandler(this.btnUpdateBA_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(45, 90);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(782, 195);
            this.txtResult.TabIndex = 1;
            // 
            // btnAcctQuery
            // 
            this.btnAcctQuery.Location = new System.Drawing.Point(325, 29);
            this.btnAcctQuery.Name = "btnAcctQuery";
            this.btnAcctQuery.Size = new System.Drawing.Size(228, 39);
            this.btnAcctQuery.TabIndex = 2;
            this.btnAcctQuery.Text = "Account Query";
            this.btnAcctQuery.UseVisualStyleBackColor = true;
            this.btnAcctQuery.Click += new System.EventHandler(this.btnAcctQuery_Click);
            // 
            // frmTestcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 409);
            this.Controls.Add(this.btnAcctQuery);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnUpdateBA);
            this.Name = "frmTestcs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTestcs";
            this.Load += new System.EventHandler(this.frmTestcs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdateBA;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnAcctQuery;
    }
}