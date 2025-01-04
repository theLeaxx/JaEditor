using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JaEditor
{
    public partial class JaEditor : Form
    {
        private Dictionary<int, int> _fields = new Dictionary<int, int>();
        private int editedItemIndex = -1;

        public JaEditor()
        {
            InitializeComponent();

            PopulateListbox1();

            //listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            //listBox2.DrawMode = DrawMode.OwnerDrawFixed;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the name of the field that was selected
            string fieldName = listBox1.SelectedItem.ToString();

            listBox2.Items.Clear();
            var value = typeof(SaveFile).GetField(fieldName).GetValue(Program.loadedSave);

            if (value.GetType() == typeof(List<int>))
            {
                var list = value as List<int>;
                listBox2.Items.Add(list.Count);

                foreach (var item in list)
                    listBox2.Items.Add(item);
            }
            else if(value.GetType() == typeof(List<string>))
            {
                var list = value as List<string>;
                listBox2.Items.Add(list.Count);

                foreach (var item in list)
                    listBox2.Items.Add(item);
            }
            else
                listBox2.Items.Add(value);

            listBox2.Items.Add("Type: " + value.GetType());
        }
        private void PopulateListbox1()
        {
            // get all fields from SaveFile
            var fields = typeof(SaveFile).GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                listBox1.Items.Add(fields[i].Name);

            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                string selectedItem = listBox2.SelectedItem.ToString();

                string newValue = Microsoft.VisualBasic.Interaction.InputBox("Enter new value:", "Edit Item", selectedItem);

                if (!string.IsNullOrEmpty(newValue))
                {
                    listBox2.Items[listBox2.SelectedIndex] = newValue;

                    string fieldName = listBox1.SelectedItem.ToString();

                    var value = typeof(SaveFile).GetField(fieldName).GetValue(Program.loadedSave);
                    if (value != null) 
                    {
                        if (value.GetType() == typeof(List<int>))
                        {
                            var list = value as List<int>;
                            list[listBox2.SelectedIndex - 1] = int.Parse(newValue);
                        }
                        else if (value.GetType() == typeof(List<string>))
                        {
                            var list = value as List<string>;
                            list[listBox2.SelectedIndex - 1] = newValue;
                        }
                        else
                        {
                            try
                            {
                                typeof(SaveFile).GetField(fieldName).SetValue(Program.loadedSave, Convert.ChangeType(newValue, value.GetType()));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

                editedItemIndex = listBox1.SelectedIndex;
                listBox2.Invalidate();
            }
        }
    }
}
