using DehaxOS.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DehaxOS
{
    public partial class PropertiesForm : Form
    {
        public Attributes Attributes { get; set; }
        public AccessRights AccessRights { get; set; }

        public PropertiesForm()
        {
            InitializeComponent();
        }

        public PropertiesForm(Attributes attributes, AccessRights accessRights)
            : this()
        {
            Attributes = attributes;
            AccessRights = accessRights;
        }

        private void PropertiesForm_Load(object sender, EventArgs e)
        {
            readOnlyCheckBox.Checked = Attributes.readOnly;
            hiddenCheckBox.Checked   = Attributes.hidden;
            systemCheckBox.Checked   = Attributes.system;

            userReadCheckBox.Checked      = AccessRights.user.canRead;
            userWriteCheckBox.Checked     = AccessRights.user.canWrite;
            userExecuteCheckBox.Checked   = AccessRights.user.canExecute;
            groupReadCheckBox.Checked     = AccessRights.group.canRead;
            groupWriteCheckBox.Checked    = AccessRights.group.canWrite;
            groupExecuteCheckBox.Checked  = AccessRights.group.canExecute;
            othersReadCheckBox.Checked    = AccessRights.others.canRead;
            othersWriteCheckBox.Checked   = AccessRights.others.canWrite;
            othersExecuteCheckBox.Checked = AccessRights.others.canExecute;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Attributes attributes = new Attributes();
            attributes.readOnly = readOnlyCheckBox.Checked;
            attributes.hidden   = hiddenCheckBox.Checked;
            attributes.system   = systemCheckBox.Checked;
            Attributes = attributes;

            AccessRights accessRights = new AccessRights();
            accessRights.user.canRead      = userReadCheckBox.Checked;
            accessRights.user.canWrite     = userWriteCheckBox.Checked;
            accessRights.user.canExecute   = userExecuteCheckBox.Checked;
            accessRights.group.canRead     = groupReadCheckBox.Checked; 
            accessRights.group.canWrite    = groupWriteCheckBox.Checked;
            accessRights.group.canExecute  = groupExecuteCheckBox.Checked;
            accessRights.others.canRead    = othersReadCheckBox.Checked;
            accessRights.others.canWrite   = othersWriteCheckBox.Checked;
            accessRights.others.canExecute = othersExecuteCheckBox.Checked;
            AccessRights = accessRights;
        }
    }
}
