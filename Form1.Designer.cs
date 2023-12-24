using System.Diagnostics;
using System.Reflection;

namespace FilePumper
{
    public partial class Form1 : Form
    {
        private RadioButton rbKilobyte;
        private RadioButton rbMegabyte;
        private RadioButton rbGigabyte;
        private Button btnPump;
        private Button btnSelectFile;
        private TextBox tbSize;
        private OpenFileDialog openFileDialog;
        private PictureBox pbButton;
        private string selectedFilePath;

        private void Form1_Load(object sender, EventArgs e)
        {
            rbKilobyte = new RadioButton { Location = new Point(50, 80), Text = "Kilobyte" };
            rbMegabyte = new RadioButton { Location = new Point(50, 110), Text = "Megabyte" };
            rbGigabyte = new RadioButton { Location = new Point(50, 140), Text = "Gigabyte" };
            tbSize = new TextBox { Location = new Point(30, 50), PlaceholderText = "Size" };
            btnPump = new Button { Location = new Point(200, 210), Text = "Pump" };
            btnSelectFile = new Button { Location = new Point(30, 20), Text = "Select File" };
            openFileDialog = new OpenFileDialog();
            pbButton = new PictureBox
            {
                Location = new Point(200, 15),
                Size = new Size(90, 80),
                Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("FilePumper.discord.jpg")),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Add the controls to the form
            this.Controls.Add(rbKilobyte);
            this.Controls.Add(rbMegabyte);
            this.Controls.Add(rbGigabyte);
            this.Controls.Add(btnPump);
            this.Controls.Add(tbSize);
            this.Controls.Add(btnSelectFile);
            this.Controls.Add(pbButton);

            // Add event handlers
            btnPump.Click += BtnPump_Click;
            btnSelectFile.Click += BtnSelectFile_Click;
            pbButton.Click += PbButton_Click;
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;
            }
        }

        private void BtnPump_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a file.");
                return;
            }

            if (int.TryParse(tbSize.Text, out int size))
            {
                try
                {
                    int multiplier = rbKilobyte.Checked ? 1024 : rbMegabyte.Checked ? 1024 * 1024 : 1024 * 1024 * 1024;
                    int pumpSize = size * multiplier;
                    using (FileStream fs = new FileStream(selectedFilePath, FileMode.Append))
                    {
                        for (int i = 0; i < pumpSize; i++)
                        {
                            fs.WriteByte(0);
                        }
                    }
                    MessageBox.Show($"Successfully pumped file: {selectedFilePath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error with the file pumper: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Invalid size input. Please enter a number.");
            }
        }
        private void PbButton_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.gg/aRMDzvdekp", 
                UseShellExecute = true
            });
        }

        private void InitializeComponent()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 250);
            this.Text = "File Pumper";
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            this.Load += new System.EventHandler(this.Form1_Load);
        }
    }
}
