using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace AdminPanel
{
    public partial class CreatePackage : MetroForm
    {
        private string jsonResult;

        public CreatePackage()
        {
            InitializeComponent();
        }

        private void removeItem_Click(object sender, EventArgs e)
        {
            if (itemsList.SelectedItem != null)
                itemsList.Items.Remove(itemsList.SelectedItem);
        }

        private void addItem_Click(object sender, EventArgs e)
        {
            int count;
            int item;

            if (new AddItemForm().ShowDialog(out count, out item) == DialogResult.OK)
                for (int i = 0; i < count; i++)
                    itemsList.Items.Add(item);
        }

        private void saveBox_Click(object sender, EventArgs e)
        {
            var package = new package
            {
                name = this.packageName.Text,
                maxPurchase = int.Parse(this.maxPurchase.Text),
                weight = int.Parse(this.weight.Text),
                content = new content
                {
                    items = itemsList.Items.Cast<int>().ToArray(),
                    charSlots = int.Parse(charSlotsBox.Text),
                    vaultChests = int.Parse(vaultChestsBox.Text)
                },
                bgUrl = this.bgUrl.Text,
                price = int.Parse(this.price.Text),
                quantity = int.Parse(this.quantity.Text),
                endDate = db.Database.DateTimeToUnixTimestamp(endDate.Value)
            };

            JsonSerializer s = new JsonSerializer();
            var wtr = new StringWriter();
            s.Serialize(wtr, package, typeof(package));
            jsonResult = wtr.ToString();

            DialogResult = DialogResult.OK;
            Close();
        }

        public string PackageResult => HttpUtility.UrlEncode(this.jsonResult);
    }

    struct package
    {
        public string name;
        public int maxPurchase;
        public int weight;
        public content content;
        public string bgUrl;
        public int price;
        public int quantity;
        public int endDate;
    }

    struct content
    {
        public int[] items;
        public int charSlots;
        public int vaultChests;
    }
}
