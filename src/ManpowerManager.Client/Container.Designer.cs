namespace ManpowerManager.Client;

partial class Container {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing ) {
        if( disposing && (components != null) ) {
            components.Dispose();
        }
        base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.SuspendLayout();
        // 
        // Container
        // 
        this.AutoScaleDimensions = new SizeF( 10F, 25F );
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size( 1017, 529 );
        this.IsMdiContainer = true;
        this.Name = "Container";
        this.Text = "Manpower Manager";
        this.ResumeLayout( false );
    }

    #endregion
}