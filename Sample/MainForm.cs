using SharpConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ScintillaNET;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Sample
{
    // Demonstrates the basic usage of SharpConfig.
    public partial class MainForm : Form
    {
        private Scintilla txtBox;
        private Configuration currentConfig;

        public MainForm()
        {
            InitializeComponent();
        }

        // Parses the configuration that is written in the TextBox,
        // and represents its contents in a logical structure inside the TreeView.
        private void ParseConfig()
        {
            // Clear the TreeView first.
            trViewCfg.Nodes.Clear();

            if ( string.IsNullOrEmpty( txtBox.Text ) )
                return;

            //try
            //{
                // We want to measure how long SharpConfig needs to parse the configuration.
                var watch = Stopwatch.StartNew();

                Configuration.ValidCommentChars = txtCommentChars.Text.ToCharArray();

                // Load the configuration from the TextBox's text.
                currentConfig = Configuration.LoadFromText( txtBox.Text );

                // Stop measuring the time.
                watch.Stop();

                double timeMs = (double)( (double)watch.ElapsedTicks / TimeSpan.TicksPerMillisecond );

                // Just a simple notification for the user.
                LogMessage( string.Format(
                    CultureInfo.InvariantCulture.NumberFormat,
                    "I just needed {0}ms to parse the configuration!",
                    Math.Round( timeMs, 2 ) ), Color.Green );

                // List the contents of the configuration in the TreeView.
                foreach ( var section in currentConfig )
                {
                    var sectionNode = new TreeNode( section.Name );
                    sectionNode.ForeColor = Color.Blue;

                    TreeNode commentNode = null;

                    if ( section.Comment != null )
                    {
                        commentNode = new TreeNode( "[Comment]: " + section.Comment.ToString() );
                        commentNode.ForeColor = Color.Green;
                        sectionNode.Nodes.Add( commentNode );
                    }

                    foreach ( var setting in section )
                    {
                        var settingNode = new TreeNode( setting.Name );

                        var valNode = new TreeNode( string.Format(
                            "[Value]{0}: {1}", setting.IsArray ? " [Array]" : "", setting.Value ) );

                        valNode.ForeColor = Color.Goldenrod;
                        settingNode.Nodes.Add( valNode );

                        if ( setting.Comment != null )
                        {
                            commentNode = new TreeNode( "[Comment]: " + setting.Comment.ToString() );
                            commentNode.ForeColor = Color.Green;
                            settingNode.Nodes.Add( commentNode );
                        }

                        sectionNode.Nodes.Add( settingNode );
                    }

                    trViewCfg.Nodes.Add( sectionNode );
                }

                trViewCfg.ExpandAll();
            //}
            //catch ( Exception ex )
            //{
            //    // Some error occurred! Notify the user.
            //    LogMessage( ex.Message, Color.Red );
            //}
        }

        // -----------------------------------------------------------
        // The code below has nothing to do with SharpConfig,
        // so you're free to ignore it!
        // -----------------------------------------------------------

        private void MainForm_Load( object sender, EventArgs e )
        {
            txtBox = new Scintilla();
            txtBox.Dock = DockStyle.Fill;
            txtBox.Margins[0].Width = 20;

            txtBox.Lexing.LexerLanguageMap["ini"] = "cpp";
            txtBox.ConfigurationManager.CustomLocation = Path.GetFullPath( "ScintillaNET.xml" );
            txtBox.ConfigurationManager.Language = "ini";
            txtBox.ConfigurationManager.Configure();

            txtBox.Text = Properties.Resources.SampleCfg;

            foreach (var ch in Configuration.ValidCommentChars)
                txtCommentChars.Text += ch;

            // HACK: parse the sample config, so the Stopwatch won't report any JIT time.
            ParseConfig();

            txtBox.TextChanged += OnConfigChanged;
            pnlTxtBox.Controls.Add( txtBox );

            txtBox.Text = Properties.Resources.SampleCfg;
        }

        private void OnConfigChanged( object sender, EventArgs e )
        {
            // If the timer is already running, extend its interval.
            // Otherwise, just start it.

            if ( mainTimer.Enabled )
                mainTimer.Interval += 20;
            else
                mainTimer.Start();

            LogMessage( "Editing...", Color.Black );
        }

        private void mainTimer_Tick( object sender, EventArgs e )
        {
            ParseConfig();
            mainTimer.Stop();

            // Reset the interval.
            mainTimer.Interval = 500;
        }

        private void LogMessage( string msg, Color color )
        {
            trViewCfg.Nodes.Clear();

            trViewCfg.Nodes.Add( new TreeNode( msg )
                {
                    ForeColor = color
                } );
        }

        private void picBoxLogo_Click( object sender, EventArgs e )
        {
            Process.Start( @"https://github.com/cemdervis/SharpConfig" );
        }

        private void picBoxLogo_MouseEnter( object sender, EventArgs e )
        {
            Cursor = Cursors.Hand;
            picBoxLogo.BackColor = Color.LightSlateGray;

            var tt = new ToolTip();
            tt.SetToolTip( picBoxLogo, "Visit the SharpConfig website" );
        }

        private void picBoxLogo_MouseLeave( object sender, EventArgs e )
        {
            Cursor = Cursors.Default;
            picBoxLogo.BackColor = this.BackColor;
        }

        private void btnOpenConfig_Click( object sender, EventArgs e )
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Configuration file|*.ini;*.cfg";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtBox.Text = File.ReadAllText( dialog.FileName );
                }
            }
        }

        private void btnSaveConfig_Click( object sender, EventArgs e )
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Configuration file|*.ini;*.cfg";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    currentConfig.Save( dialog.FileName );
                }
            }
        }

    }
}
