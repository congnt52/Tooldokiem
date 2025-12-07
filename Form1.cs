using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ToolDo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void trvTestCases_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        private bool xmlLoaded = false;
       
        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
                 //aaa     
        }
        


        private void LoadXmlToTextbox(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            txtFreq.Text = doc.Root
                .Element("General")
                .Element("Frequency").Value.Replace("MHz", "").Trim();
            txtFrequency.Text = doc.Root
                .Element("General")
                .Element("Frequency").Value.Replace("MHz", "").Trim();

            txtPowerdBm.Text = doc.Root
                .Element("General")
                .Element("PowerDBm").Value.Replace("dBm", "").Trim();
            txtPower.Text = doc.Root
                .Element("General")
                .Element("PowerDBm").Value.Replace("dBm", "").Trim();

            txtBw.Text = doc.Root
                .Element("General")
                .Element("Bandwidth").Value.Replace("MHz", "").Trim();
            textBandwidth.Text = doc.Root
                .Element("General")
                .Element("Bandwidth").Value.Replace("MHz", "").Trim();

            txtRRU.Text = doc.Root
                .Element("General")
                .Element("RRUSN").Value.Trim();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!xmlLoaded)
            {
                MessageBox.Show("Chưa load file XML!");
                return;
            }

            try
            {
                string xmlModified = rtbXmlPreview.Text;
                LoadXmlToTextbox(xmlModified);

                MessageBox.Show("Đã cập nhật dữ liệu từ XML view!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("XML lỗi hoặc sai cấu trúc:\n" + ex.Message);
            }
        }
        private void LoadXmlToTreeView(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            var testClause = doc.Root.Element("TestClause");
            if (testClause == null)  return; 
            tcTreeview.Nodes.Clear();
            tcTreeview.CheckBoxes = true;
            TreeNode rootNode = new TreeNode("TestClause");
            tcTreeview.Nodes.Add(rootNode);
            foreach(var clause in testClause.Elements("Clause"))
    {
                string name = clause.Element("Name")?.Value.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    TreeNode node = new TreeNode(name);
                    node.Checked = true;
                    rootNode.Nodes.Add(node);
                }
            }

            tcTreeview.ExpandAll();

        }
        private void tcTreeview_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void btnConfigPath_Click(object sender, EventArgs e)
        {
            string fileConfigPath = txtConfigPath.Text;
            string xml = File.ReadAllText(fileConfigPath);
            rtbXmlPreview.Text = xml;
            LoadXmlToTextbox(xml);
            LoadXmlToTreeView(xml);
            xmlLoaded = true;
        }


        private void btnPathConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            ofd.Title = "Chọn file config";

            // Nếu đã từng chọn file trước đó → mở lại đúng folder
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ConfigFilePath))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.ConfigFilePath);
                ofd.FileName = Path.GetFileName(Properties.Settings.Default.ConfigFilePath);
            }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Lấy đường dẫn đầy đủ
                string filePath = ofd.FileName;

                // Hiện lên textbox
                txtConfigPath.Text = filePath;

                // Lưu lại vào Settings
                Properties.Settings.Default.ConfigFilePath = filePath;
                Properties.Settings.Default.Save();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtConfigPath.Text = Properties.Settings.Default.ConfigFilePath;
        }
    }
}
