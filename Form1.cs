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
        private string currentFilePath = "";
        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "XML Files|*.xml";

            if (open.ShowDialog() == DialogResult.OK)
            {
                string xml = File.ReadAllText(open.FileName);
                rtbXmlPreview.Text = xml;

                LoadXmlToTextbox(xml);
                LoadXmlToTreeView(xml);
                xmlLoaded = true;
            }
        }
        private void LoadXmlToTextbox(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            txtFreq.Text = doc.Root
                .Element("General")
                .Element("Frequency").Value.Replace("MHz", "").Trim();

            txtPowerdBm.Text = doc.Root
                .Element("General")
                .Element("PowerDBm").Value.Replace("dBm", "").Trim();

            txtBw.Text = doc.Root
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

        //  private XDocument doc;
        //private void LoadConfigFromXml(string path)
        //{
        //    XDocument doc = XDocument.Load(path);

        //    var general = doc.Root.Element("General");

        //    txtFreq.Text = general.Element("Frequency").Value;
        //    txtPowerdBm.Text = general.Element("PowerDBm").Value;
        //    txtPowerMw.Text = general.Element("PowerMW").Value;
        //    txtBw.Text = general.Element("Bandwidth").Value;
        //    txtRRU.Text = general.Element("RRUSN").Value;

        //     Load danh sách Ports
        //    var ports = doc.Root.Element("PortList").Elements("Port").Select(x => x.Value).ToList();
        //    LoadPortCheckboxList(ports);
        //}

        //private void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    if (doc == null)
        //    {
        //        MessageBox.Show("Chưa load file XML!");
        //        return;
        //    }

        //    var general = doc.Root.Element("General");
        //    general.Element("Frequency").Value = txtFreq.Text;
        //    general.Element("PowerDBm").Value = txtPowerdBm.Text;
        //    general.Element("Bandwidth").Value = txtBw.Text;
        //    general.Element("RRUSN").Value = txtRRU.Text;

        //    // Cập nhật view
        //    rtbXmlPreview.Text = doc.ToString();
        //}
    }
}
