using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Commerce_App
{
    public partial class Form1 : Form
    {
        public commercial_systemEntities ent;
        public List<Panel> Panels;
        public List<Panel> Reports;
        public Dictionary<Item_Store,int> data;

        public Form1()
        {
            InitializeComponent();
             ent = new commercial_systemEntities();
            Panels = new List<Panel>();
            Reports = new List<Panel>();
            data = new Dictionary<Item_Store, int>();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Panels.Add(panelStores);
            Panels.Add(panelItems);
            Panels.Add(panelItemsStore);
            Panels.Add(panelSupplier);
            Panels.Add(panelConsumers);
            Panels.Add(panelPurchaseOrder);
            Panels.Add(panelSalesOrder);
            Panels.Add(panelReports);

            foreach(var P in Panels)
            {
                P.Hide();
            }
            //---------------------------------------------------------
            Reports.Add(panelReport1);
            Reports.Add(panelReport2);
            Reports.Add(panelReport3);
            Reports.Add(panelReport4);
            Reports.Add(panelReport5);


            foreach (var P in Reports)
            {
                P.Hide();
            }
        }

        #region helper functions 

        #region validators functions
        public bool EmailValidator(string data)
        {
            Regex EmailRegex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            bool isValidEmail = EmailRegex.IsMatch(data);
            return isValidEmail;
        }
        public bool PhoneValidator(string data)
        {
            Regex PhoneRegex = new Regex(@"^[0-9]{9}$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            bool isValidPhone = PhoneRegex.IsMatch(data);
            return isValidPhone;
        }
        public bool MobileValidator(string data)
        {
            Regex MobileRegex = new Regex(@"^(010)|(011)|(012)[0-9]{8}$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            bool isValidMobile = MobileRegex.IsMatch(data);
            return isValidMobile;
        }
        public bool WebSiteValidator(string data)
        {
            Regex WebSiteRegex = new Regex(@"^www\.[\w-]+\.com$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            bool isValidWebSite = WebSiteRegex.IsMatch(data);
            return isValidWebSite;
        }
        public bool IntType(string data)
        {
            Regex IntRegex = new Regex(@"^[0-9]+$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            bool isValidInt = IntRegex.IsMatch(data);
            return isValidInt;
        }

        #endregion
        #region Rerender in comboboxes on Add or Update
        public void RerenderStoreNames(ComboBox combobox)
        {
            var storeNames = from store in ent.Stores select store;
            combobox.Items.Clear();
            foreach (var s in storeNames)
            {
                combobox.Items.Add(s.Name);
            }
        }
        public void RerenderItemIDs(ComboBox combobox)
        {
            var ItemIDs = from IDs in ent.Items orderby IDs.ID select IDs;
            combobox.Items.Clear();
            foreach (var ID in ItemIDs)
            {
                combobox.Items.Add(ID.ID);
            }
        }
        public void RerenderMesurmentValues(ComboBox combobox, int ID)
        {
            var mesurementValues = ent.Items.Find(ID).Item_MesurementValues;
            combobox.Items.Clear();
            foreach (var value in mesurementValues)
            {
                combobox.Items.Add(value.Mesurment_Value);
            }
        }
        public void RerenderSuppliersIDs(ComboBox combobox)
        {
            var SupplierIDs = from supplier in ent.Suppliers orderby supplier.ID select supplier;
            combobox.Items.Clear();
            foreach (var s in SupplierIDs)
            {
                combobox.Items.Add(s.ID);
            }
        }
        public void RerenderConsumersIDs(ComboBox combobox)
        {
            var ConsumerIDs = from Customer in ent.Customers orderby Customer.ID select Customer;
            combobox.Items.Clear();
            foreach (var c in ConsumerIDs)
            {
                combobox.Items.Add(c.ID);
            }
        }
        public void RerenderPurchaseOrderIDs(ComboBox combobox)
        {
            var PurchaseIDs = from Purchase_Order in ent.Purchase_Order orderby Purchase_Order.ID select Purchase_Order;
            combobox.Items.Clear();
            foreach (var p in PurchaseIDs)
            {
                combobox.Items.Add(p.ID);
            }
        }
        public void RerenderSalesOrderIDs(ComboBox combobox)
        {
            var SalesIDs = from sales_order in ent.Pay_Order orderby sales_order.ID select sales_order;
            combobox.Items.Clear();
            foreach (var s in SalesIDs)
            {
                combobox.Items.Add(s.ID);
            }
        }
        public void RerenderStoreItems(CheckedListBox checkedListBox, string StoreName)
        {
            var storeItems = (from Items in ent.Item_Store where Items.Store_Name == StoreName select Items).AsEnumerable();
            checkedListBox.Items.Clear();
            foreach (var I in storeItems)
            {
                checkedListBox.Items.Add("    ID: " + I.Item_iD + "        Name: " + I.Item_Name + "         Qty: " + I.Qty);
            }
        }
        public void RerenderStoreItems(ListBox ListBox, string StoreName)
        {
            var storeItems = (from Items in ent.Item_Store where Items.Store_Name == StoreName select Items).AsEnumerable();
            ListBox.Items.Clear();
            foreach (var I in storeItems)
            {
                ListBox.Items.Add("    ID: " + I.Item_iD + "        Name: " + I.Item_Name + "         Qty: " + I.Qty);
            }
        }
        public void RerenderPurchaseOrderItems(ComboBox combobox)
        {
            var PurchaseOrderItems = ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text)).Items_PurchaseOrder;
            combobox.Items.Clear();
            foreach (var item in PurchaseOrderItems)
            {
                combobox.Items.Add(item.Item_ID);
            }
        }
        public void RerenderSalesOrderItems(ComboBox combobox)
        {
            var salesOrderItems = ent.Pay_Order.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text)).Items_PayOrder;
            combobox.Items.Clear();
            foreach (var item in salesOrderItems)
            {
                combobox.Items.Add(item.Item_ID);
            }
        }
        public void RerenderDataInReport2(DataGridView dataGridView , Item item)
        {
            dataGridView.Columns.RemoveAt(dataGridView.Columns.Count - 1);
            dataGridView.Columns.RemoveAt(dataGridView.Columns.Count - 1);
            //change column name
            dataGridView.Columns["Date"].HeaderText = "Entered Store";
            //make 2 new columns related to item pdn and expiry dates
            if (dataGridView.Columns.Contains("productionDate") == false && dataGridView.Columns.Contains("expireDate") == false)
            {
                dataGridView.Columns.Add("productionDate", "Production Date");
                dataGridView.Columns.Add("expireDate", "Expiry Date");
            }
            //insert data in those 2 new columns
            for (var i = dataGridView.Rows.Count; i != 0; i--)
            {
                dataGridView.Rows[i - 1].Cells["productionDate"].Value = item.Production_date.ToShortDateString();
                dataGridView.Rows[i - 1].Cells["expireDate"].Value = item.Expiry_date.ToShortDateString();
            }
        }
        #endregion
        #region Show and Hide panels
        public void hideAllAndShowOnePanel(Panel P)
        {
            foreach (var Panel in Panels)
            {
                Panel.Hide();
            }
            P.Show();
        }
        public void hideAllAndShowOneReport(Panel R)
        {
            foreach (var Report in Reports)
            {
                Report.Hide();
            }
            R.Show();
        }
        #endregion
        #region delete items in purchase orders
        public void DeleteItemFromPurchaseOrder(int itemID, string storeName,Items_PurchaseOrder PurchaseOrderItem)
        {
            var item_Store = ent.Item_Store.Find(itemID, storeName);
            if (item_Store != null)
            {
                int newQty = int.Parse(item_Store.Qty.ToString()) + PurchaseOrderItem.Qty;
                item_Store.Qty = newQty;

                //remove item from purchase order
                ent.Items_PurchaseOrder.Remove(PurchaseOrderItem);
                comboBoxItemIDsInPurchaseOrderPanel.Text = textBoxQtyInPurchaseOrderPanel.Text = textBoxItemNameInPurchaseOrderPanel.Text = string.Empty;
                ent.SaveChanges();
            }
            else
            {
                var stores = (from S in ent.Item_Store where S.Item_iD == itemID select S.Store_Name).AsEnumerable();
                if (stores.Count() > 0)
                {
                    string msg = String.Join(", ", stores);
                    MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}. But it may exsists in {msg}");
                }
                else
                {
                    MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}");
                }
            }
        }

        #endregion
        #region enable/disable and style gridview in item store change
        public void disableGridView(List<int> cols)
        {
            foreach(var col in cols)
            {
                dataGridViewChange.Columns[col].ReadOnly = true;
                dataGridViewChange.Columns[col].DefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewChange.Columns[col].DefaultCellStyle.ForeColor = Color.DarkGray;
                dataGridViewChange.Columns[col].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                dataGridViewChange.Columns[col].DefaultCellStyle.SelectionForeColor = Color.Black;

            }
        }
        #endregion
        #region check item in item store
        public bool checkItemInItemStoreTable(Item_Store item)
        {
            bool flag;
            var id = item.Item_iD;
            var storeName = item.Store_Name;
            var result = ent.Item_Store.Find(id, storeName);
            if (result == null)
            {
                // new item
                flag = true;
            }
            else
            {
                //exists befor
                flag = false;
            }
            return flag;
        }
        #endregion
        #endregion

        #region panels show
        private void buttonStores_Click(object sender, EventArgs e)
        {
            hideAllAndShowOnePanel(panelStores);

            // add storeNames to combobox in store panel
            RerenderStoreNames(comboBoxstoreName);          
        }
        private void buttonItems_Click(object sender, EventArgs e)
        {
            hideAllAndShowOnePanel(panelItems);

            //add ItemIDs to combobox in items panel
            RerenderItemIDs(comboBoxItemId);
        }
        private void buttonChangeItemsStor_Click(object sender, EventArgs e)
        {
            hideAllAndShowOnePanel(panelItemsStore);

            // add storeNames to combobox in items Store panel
            RerenderStoreNames(comboBoxStoreInChangeItemStorePanel);
        }
        private void buttonSupplier_Click(object sender, EventArgs e)
        {
            hideAllAndShowOnePanel(panelSupplier);

            //add suppliers to suppliers panel
            RerenderSuppliersIDs(comboBoxIDInSupplierPanel);
        }
        private void buttonConsumer_Click(object sender, EventArgs e)
        {
            hideAllAndShowOnePanel(panelConsumers);

            //add consumers to consumers panel
            RerenderConsumersIDs(comboBoxConsumersIDs);
        }
        private void buttonPurchaseOrder_Click(object sender, EventArgs e)
        {
            hideAllAndShowOnePanel(panelPurchaseOrder);

            //add Purchase Orders to Purchase Order panel
            RerenderPurchaseOrderIDs(comboBoxPurchaseOrderIDs);

            //add store names to store names combobox in purchase order panel
            RerenderStoreNames(comboBoxStoreNameInPurchaseOrderPanel);

            //add consumers ids to consumer combobox in purchase order panel
            RerenderConsumersIDs(comboBoxConsumerIDsInPurchaseOrderPanel);

            textBoxConsumerNameInPurchaseOrderPanel.Text = string.Empty;
        }
        private void buttonSalesOrder_Click(object sender, EventArgs e)
        {
            hideAllAndShowOnePanel(panelSalesOrder);

            //add sales Orders to sales Order panel
            RerenderSalesOrderIDs(comboBoxSalesOrderIDsInSalesOrderPanel);

            //add store names to store name combobox in sales order panel
            RerenderStoreNames(comboBoxStoreNameInSalesOrderPanel);

            //add suppliers ids to suppliers combobox in sales order panel
            RerenderSuppliersIDs(comboBoxSupplierIdInSalesOrderPanel);

            textBoxSupplierNameInSalesOrderPanel.Text = string.Empty;
        }
        private void buttonReports_Click(object sender, EventArgs e)
        {
            hideAllAndShowOnePanel(panelReports);
        }

        #endregion

        #region Reports show
        private void buttonReport1_Click(object sender, EventArgs e)
        {
            hideAllAndShowOneReport(panelReport1);

            // add storeNames to combobox in report1 panel
            RerenderStoreNames(comboBoxStoreNamesInReport1);
        }
        private void buttonReport2_Click(object sender, EventArgs e)
        {
            hideAllAndShowOneReport(panelReport2);

            //add IDs to  itemID combobox in report2 panel
            var ItemNames = from Names in ent.Items orderby Names.ID select Names;
            comboBoxItemNamesReport2.Items.Clear();
            foreach (var item in ItemNames)
            {
                comboBoxItemNamesReport2.Items.Add(item.Name);
            }
        }
        private void buttonReport3_Click(object sender, EventArgs e)
        {
            hideAllAndShowOneReport(panelReport3);

            //add storeNames to  storeName checkedlist in report3 panel
            var storeNames = from store in ent.Stores select store;
            checkedListBoxReport3.Items.Clear();
            foreach (var s in storeNames)
            {
                checkedListBoxReport3.Items.Add(s.Name);
            }
        }
        private void buttonReport4_Click(object sender, EventArgs e)
        {
            hideAllAndShowOneReport(panelReport4);
        }
        private void buttonReport5_Click(object sender, EventArgs e)
        {
            hideAllAndShowOneReport(panelReport5);
        }
        #endregion

        #region stores Panel
        private void buttonAddStore_Click(object sender, EventArgs e)
        {
            if (comboBoxstoreName.Text != string.Empty && textBoxStoreLocation.Text != string.Empty && textBoxStoreLocation.Text != string.Empty)
            {
                if (ent.Stores.Find(comboBoxstoreName.Text) == null)
                {
                    Store store = new Store();
                    store.Name = comboBoxstoreName.Text;
                    store.Location = textBoxStoreLocation.Text;
                    store.Responsible_person = textBoxRespPerson.Text;
                    ent.Stores.Add(store);
                    ent.SaveChanges();
                    textBoxRespPerson.Text = textBoxStoreLocation.Text = comboBoxstoreName.Text = string.Empty;
                    RerenderStoreNames(comboBoxstoreName);
                }
                else
                {
                    MessageBox.Show("this store already exists");
                }
            }
            else
            {
                MessageBox.Show("Incomplete Store Info");
            }

        }
        private void comboBoxstoreName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var store = ent.Stores.Find(comboBoxstoreName.SelectedItem);
            if (store != null)
            {
                textBoxStoreLocation.Text = store.Location;
                textBoxRespPerson.Text = store.Responsible_person;
            }
            else
            {
                MessageBox.Show("Invalid Store Name ... ");
            }
        }
        private void buttonUpdateStore_Click(object sender, EventArgs e)
        {
            var store = ent.Stores.Find(comboBoxstoreName.Text);
            if (store != null)
            {
                if (comboBoxstoreName.Text != string.Empty && textBoxStoreLocation.Text != string.Empty && textBoxRespPerson.Text != string.Empty)
                {
                    store.Location = textBoxStoreLocation.Text;
                    store.Responsible_person = textBoxRespPerson.Text;
                    ent.SaveChanges();
                    textBoxStoreLocation.Text = textBoxRespPerson.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Incomplete Store Info");
                }
            }
            else
            {
                MessageBox.Show("Invalid Store Name");
            }
        }

        #endregion

        #region items panel
        
        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            // make sure all data of item and store name exists
            if (comboBoxItemId.Text != string.Empty && textBoxItemName.Text != string.Empty && monthCalendarItemPdnDate.SelectionStart.ToString() != string.Empty && monthCalendarItemExpDate.SelectionStart.ToString() != string.Empty)
            {
                    if(ent.Items.Find(int.Parse(comboBoxItemId.Text)) == null)
                    {
                        // insert into items table
                        Item item = new Item();
                        item.ID = int.Parse(comboBoxItemId.Text);
                        item.Name = textBoxItemName.Text;
                        //date time problemaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                        item.Production_date = monthCalendarItemPdnDate.SelectionStart;
                        item.Expiry_date = monthCalendarItemExpDate.SelectionStart;
                        TimeSpan shelfLife = (TimeSpan) (item.Expiry_date - item.Production_date);
                        item.shelf_life = shelfLife.Days;
                    if (item.shelf_life > 0)
                    {
                        ent.Items.Add(item);
                        ent.SaveChanges();
                        RerenderItemIDs(comboBoxItemId);
                        textBoxItemName.Text = textBoxItemQtyInItemPanel.Text = textBoxStoreNameItemsPanel.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Expiry date can't be less than Production Date");
                    }
                    
                }
                else
                    {
                        MessageBox.Show("this item already exists");
                    }
            }
            else
            {
                MessageBox.Show("Incomplete Item Info");
            }
        }
        private void comboBoxItemId_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = ent.Items.Find(int.Parse(comboBoxItemId.Text));
            if (item != null)
            {
                textBoxItemQtyInItemPanel.Text = textBoxItemName.Text = textBoxStoreNameItemsPanel.Text = string.Empty;           
                var id = int.Parse(comboBoxItemId.Text);
                textBoxItemName.Text = item.Name;
                monthCalendarItemPdnDate.SelectionStart = item.Production_date;
                monthCalendarItemExpDate.SelectionStart = item.Expiry_date;
                //var qty = (from Q in ent.Item_Store where Q.Item_Name == textBoxItemName.Text select Q.Qty).Sum();
                var qty = (from Q in ent.Item_Store where Q.Item_iD == id select Q.Qty).FirstOrDefault();
                if ( qty != null)
                {
                    textBoxItemQtyInItemPanel.Text = qty.ToString();
                }
                var Itemstore = (from I in ent.Item_Store where I.Item_iD == id select I.Store_Name).FirstOrDefault();
                textBoxStoreNameItemsPanel.Text = Itemstore;
                if(ent.Items.Find(int.Parse(comboBoxItemId.Text)).Item_MesurementValues != null)
                {
                    var mesurementValues = ent.Items.Find(int.Parse(comboBoxItemId.Text)).Item_MesurementValues;
                    comboBoxMesurmentValues.Items.Clear();
                    foreach(var value in mesurementValues)
                    {
                        comboBoxMesurmentValues.Items.Add(value.Mesurment_Value);
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Invalid Item ID ... ");
            }
        }
        private void buttonUpdateItem_Click(object sender, EventArgs e)
        {
            if (comboBoxItemId.Text != string.Empty)
            {
                var item = ent.Items.Find(int.Parse(comboBoxItemId.Text));
                if (item != null)
                {
                    if (textBoxItemName.Text != string.Empty && monthCalendarItemPdnDate.SelectionStart.ToString() != string.Empty && monthCalendarItemExpDate.SelectionStart.ToString() != string.Empty)
                    {
                        item.Name = textBoxItemName.Text;
                        //date time problemaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                        item.Production_date = monthCalendarItemPdnDate.SelectionStart;
                        item.Expiry_date = monthCalendarItemExpDate.SelectionStart;
                        TimeSpan shelfLife = (TimeSpan)(item.Expiry_date - item.Production_date);
                        item.shelf_life = shelfLife.Days;
                        ////------------------------------------------------------------------------------------------------//
                        ////delete from store_items table
                        //var storeName = (from S in ent.Item_Store where S.Item_iD == item.ID select S.Store_Name).First();
                        //var ItemStore = ent.Item_Store.Find(int.Parse(comboBoxItemId.Text), storeName);
                        //ent.Item_Store.Remove(ItemStore);
                        //ent.SaveChanges();
                        ////Re-insert into store_items table
                        //Item_Store item_store = new Item_Store();
                        //item_store.Item_iD = int.Parse(comboBoxItemId.Text);
                        //item_store.Item_Name = textBoxItemName.Text;
                        //item_store.Qty = 0;
                        //item_store.Store_Name = comboBoxStoreNameInItemsPanel.Text;
                        //ent.Item_Store.Add(item_store);
                        ent.SaveChanges();
                        textBoxItemName.Text = textBoxItemQtyInItemPanel.Text = textBoxStoreNameItemsPanel.Text = comboBoxItemId.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Incomplete Item Info");
                    }
                }
                else
                {
                    MessageBox.Show($"Item ID {comboBoxItemId.Text} isn't avilable");
                }
            }
            else
            {
                MessageBox.Show("Insert Item ID");
            }
        }

        #region mesurment Values
        private void buttonAddMesurmentValue_Click(object sender, EventArgs e)
        {
            // make sure all data of item and mesurment value exists
            if (comboBoxItemId.Text != string.Empty && comboBoxMesurmentValues.Text != string.Empty)
            {
                // make sure that this item exists and mesurment vaue doesnt exists befor
                if (ent.Items.Find(int.Parse(comboBoxItemId.Text)) != null)
                {
                    if (ent.Item_MesurementValues.Find(int.Parse(comboBoxItemId.Text), comboBoxMesurmentValues.Text) == null)
                    {
                        Item_MesurementValues value = new Item_MesurementValues();
                        value.Item_ID = int.Parse(comboBoxItemId.Text);
                        value.Mesurment_Value = comboBoxMesurmentValues.Text;
                        ent.Item_MesurementValues.Add(value);
                        ent.SaveChanges();
                        comboBoxMesurmentValues.Text = string.Empty;
                        RerenderMesurmentValues(comboBoxMesurmentValues, value.Item_ID);
                    }
                    else
                    {
                        MessageBox.Show("this Mesurment Value already exists");
                    }
                }
                else
                {
                    MessageBox.Show("this Item doesn't exists");
                }
            }
            else
            {
                MessageBox.Show("Insert Item Id and Mesurment Value");
            }
        }
        private void buttonDeleteMesurmentValue_Click(object sender, EventArgs e)
        {
            if (comboBoxMesurmentValues.Text != string.Empty && comboBoxItemId.Text != string.Empty)
            {
                var mesurmentValue = ent.Item_MesurementValues.Find(int.Parse(comboBoxItemId.Text), comboBoxMesurmentValues.Text);
                if (mesurmentValue != null)
                {
                    ent.Item_MesurementValues.Remove(mesurmentValue);
                    comboBoxMesurmentValues.Text = string.Empty;
                    ent.SaveChanges();
                    RerenderMesurmentValues(comboBoxMesurmentValues, mesurmentValue.Item_ID);
                }
                else
                {
                    MessageBox.Show("This Mesurment value for This Item doesn't exists");
                }
            }
            else
            {
                MessageBox.Show("Insert Mesurment Value and Item ID");
            }
        }

        #endregion

        #endregion

        #region items Store Panel 
        private void comboBoxStoreInChangeItemStorePanel_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            //add other stores in New Store combobox
            var storeNames = (from Store in ent.Stores where Store.Name != comboBoxStoreInChangeItemStorePanel.Text select Store.Name).Distinct().AsEnumerable();
            comboBoxNewStoreInChangeItemStorePanel.Items.Clear();
            foreach (var S in storeNames)
            {
                comboBoxNewStoreInChangeItemStorePanel.Items.Add(S);
            }
            DateTime date = DateTime.Today;

            dataGridViewChange.DataSource = ent.R1(comboBoxStoreInChangeItemStorePanel.Text,null);
            List<int> cols = new List<int>() { 0, 1, 2, 3, 4, 6 };
            disableGridView(cols);

        }
        private void dataGridViewChange_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int res;
            if (int.TryParse(dataGridViewChange.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out res) == true)
            {
                var id = int.Parse(dataGridViewChange.Rows[e.RowIndex].Cells[0].Value.ToString());
                var store = comboBoxStoreInChangeItemStorePanel.Text;
                var item = ent.Item_Store.Find(id, store);
                data.Add(item, int.Parse(dataGridViewChange.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));
            }
        }
        private void buttonChange_Click(object sender, EventArgs e)
        {
            if(comboBoxStoreInChangeItemStorePanel.Text != String.Empty)
            {
                if (comboBoxNewStoreInChangeItemStorePanel.Text != String.Empty)
                {
                    foreach(var item in data.Keys)
                    {
                        data.TryGetValue(item, out int userEnteredQty);
                        var StoreQty = item.Qty;
                        if (userEnteredQty == StoreQty)
                        {
                            //set data of new Item with taking in consideration the new store_name
                            Item_Store New_item_store = new Item_Store();
                            New_item_store.Item_iD = item.Item_iD;
                            New_item_store.Item_Name = item.Item_Name;
                            New_item_store.Qty = userEnteredQty;
                            New_item_store.Date = item.Date;
                            New_item_store.Store_Name = comboBoxNewStoreInChangeItemStorePanel.Text;

                            // delete Item from Item_Store table
                            ent.Item_Store.Remove(item);
                            ent.SaveChanges();

                            // Re-insert Item in Item_Store table
                            var result = checkItemInItemStoreTable(New_item_store);
                            if(result == true)
                            {
                                ent.Item_Store.Add(New_item_store);
                                ent.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("This item already exists in the store, you can Edit its value");

                            }
                        }
                        else if (userEnteredQty < StoreQty)
                        {
                            //set data of new Item with taking in consideration the new store_name
                            Item_Store New_item_store = new Item_Store();
                            New_item_store.Item_iD = item.Item_iD;
                            New_item_store.Item_Name = item.Item_Name;
                            New_item_store.Qty = userEnteredQty;
                            New_item_store.Date = item.Date;
                            New_item_store.Store_Name = comboBoxNewStoreInChangeItemStorePanel.Text;

                            // edit qty of item in old store
                            item.Qty -= userEnteredQty;

                            // Insert Item in Item_Store table
                            var result = checkItemInItemStoreTable(New_item_store);
                            if (result == true)
                            {
                                ent.Item_Store.Add(New_item_store);
                                ent.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("This item already exists in the store, you can Edit its value");
                                /////////////////// 
                                ///this update process can be done automatically 
                                ///by increasing the qty of item in new store by value entered by user ,
                                ///and decreasing qty of item in old store by same value .. 
                                ///but befor that we have to check that old qty won't be less than zero or this update wont take place
                                ///the other thing is, if item in old store equals zero its preferablly to be omitted from the store 
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Item {item.Item_Name} with ID {item.Item_iD} has Qty {StoreQty} in {item.Store_Name}, So Qty of {userEnteredQty} can't be taken out of store .. \n Invalid Process");
                        }
                    }

                    comboBoxNewStoreInChangeItemStorePanel.Text = comboBoxStoreInChangeItemStorePanel.Text = string.Empty;
                    data.Clear();
                    dataGridViewChange.DataSource = ent.R1(comboBoxStoreInChangeItemStorePanel.Text, null);

                   
                }
                else
                {
                    MessageBox.Show("Select the New Store in order to complete change process ");
                }
            }
            else
            {
                MessageBox.Show("Select Store in order to complete change process ");
            }
            }

        #endregion

        #region supplier panel
        private void comboBoxIDInSupplierPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var supplier = ent.Suppliers.Find(int.Parse(comboBoxIDInSupplierPanel.Text));
            if (supplier != null)
            {
                textBoxNameSupplier.Text = supplier.Name;
                textBoxMobileSupplier.Text = supplier.Mobile;
                textBoxPhoneFaxSupplier.Text = supplier.Phone_Fax;
                textBoxEmailSupplier.Text = supplier.Email;
                textBoxWebsiteSupplier.Text = supplier.Web_Site;
            }
            else
            {
                MessageBox.Show("Invalid supplier Data ... ");
            }
            
        }
        private void buttonAddSupplier_Click(object sender, EventArgs e)
        {
            if (comboBoxIDInSupplierPanel.Text != string.Empty && textBoxNameSupplier.Text != string.Empty && textBoxMobileSupplier.Text != string.Empty && textBoxPhoneFaxSupplier.Text != string.Empty && textBoxEmailSupplier.Text != string.Empty && textBoxWebsiteSupplier.Text != string.Empty)
            {
                if (ent.Suppliers.Find(int.Parse(comboBoxIDInSupplierPanel.Text)) == null)
                {
                    bool isValid;
                    Supplier supplier = new Supplier();
                    supplier.ID = int.Parse(comboBoxIDInSupplierPanel.Text) ;
                    supplier.Name = textBoxNameSupplier.Text;
                    #region check supplier data and add it to database if it pass validators
                    //Mobile validation
                    isValid = MobileValidator(textBoxMobileSupplier.Text);
                    if (isValid)
                    {
                        supplier.Mobile = textBoxMobileSupplier.Text;
                        #region check phone, email and website
                        //Phone Validation
                        isValid = PhoneValidator(textBoxPhoneFaxSupplier.Text);
                        if (isValid)
                        {
                            supplier.Phone_Fax = textBoxPhoneFaxSupplier.Text;
                            #region check email and website
                            //Email validation
                            isValid = EmailValidator(textBoxEmailSupplier.Text);
                            if (isValid)
                            {
                                supplier.Email = textBoxEmailSupplier.Text;
                                #region check website
                                //website validation
                                isValid = WebSiteValidator(textBoxWebsiteSupplier.Text);
                                if (isValid)
                                {
                                    supplier.Web_Site = textBoxWebsiteSupplier.Text;
                                    //save supplier to database
                                    ent.Suppliers.Add(supplier);
                                    ent.SaveChanges();
                                    comboBoxIDInSupplierPanel.Text = textBoxNameSupplier.Text = textBoxMobileSupplier.Text = textBoxPhoneFaxSupplier.Text = textBoxEmailSupplier.Text = textBoxWebsiteSupplier.Text = string.Empty;
                                }
                                else
                                {
                                    MessageBox.Show("invalid Website");
                                }
                                #endregion
                            }
                            else
                            {
                                MessageBox.Show("invalid Email");
                            }
                            #endregion
                        }
                        else
                        {
                            MessageBox.Show("invalid Phone number");
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("invalid Mobile number");
                    }
                    #endregion
                    RerenderSuppliersIDs(comboBoxIDInSupplierPanel);
                }
                else
                {
                    MessageBox.Show("this Supplier already exists");
                }
            }
            else
            {
                MessageBox.Show("Incomplete Supplier Info");
            }
        }
        private void buttonUpdateSupplier_Click(object sender, EventArgs e)
        {
            var supplier = ent.Suppliers.Find(int.Parse(comboBoxIDInSupplierPanel.Text));
            if (supplier != null)
            {
                if (comboBoxIDInSupplierPanel.Text != string.Empty && textBoxNameSupplier.Text != string.Empty && textBoxMobileSupplier.Text != string.Empty && textBoxPhoneFaxSupplier.Text != string.Empty && textBoxEmailSupplier.Text != string.Empty && textBoxWebsiteSupplier.Text != string.Empty)
                {
                    bool isValid;
                    supplier.Name = textBoxNameSupplier.Text;
                    #region check supplier data and add it to database if it pass validators
                    //Mobile validation
                    isValid = MobileValidator(textBoxMobileSupplier.Text);
                    if (isValid)
                    {
                        supplier.Mobile = textBoxMobileSupplier.Text;
                        #region check phone, email and website
                        //Phone Validation
                        isValid = PhoneValidator(textBoxPhoneFaxSupplier.Text);
                        if (isValid)
                        {
                            supplier.Phone_Fax = textBoxPhoneFaxSupplier.Text;
                            #region check email and website
                            //Email validation
                            isValid = EmailValidator(textBoxEmailSupplier.Text);
                            if (isValid)
                            {
                                supplier.Email = textBoxEmailSupplier.Text;
                                #region check website and save supplier data
                                //website validation
                                isValid = WebSiteValidator(textBoxWebsiteSupplier.Text);
                                if (isValid)
                                {
                                    supplier.Web_Site = textBoxWebsiteSupplier.Text;
                                    //save supplier to database
                                    ent.SaveChanges();
                                    comboBoxIDInSupplierPanel.Text = textBoxNameSupplier.Text = textBoxMobileSupplier.Text = textBoxPhoneFaxSupplier.Text = textBoxEmailSupplier.Text = textBoxWebsiteSupplier.Text = string.Empty;
                                }
                                else
                                {
                                    MessageBox.Show("invalid Website");
                                }
                                #endregion
                            }
                            else
                            {
                                MessageBox.Show("invalid Email");
                            }
                            #endregion
                        }
                        else
                        {
                            MessageBox.Show("invalid Phone number");
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("invalid Mobile number");
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("Incomplete supplier Info");
                }
            }
            else
            {
                MessageBox.Show("Invalid supplier ID");
            }
        }

        #endregion

        #region consumer panel
        private void comboBoxConsumersIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var consumer = ent.Customers.Find(int.Parse(comboBoxConsumersIDs.Text));
            if (consumer != null)
            {
                textBoxConsumerName.Text = consumer.Name;
                textBoxConsuemrMobile.Text = consumer.Mobile;
                textBoxConsumerPhone.Text = consumer.Phone_Fax;
                textBoxConsumerEmail.Text = consumer.Email;
                textBoxConsumerWebsite.Text = consumer.Web_Site;
            }
            else
            {
                MessageBox.Show("Invalid Consumer Data ... ");
            }
        }
        private void buttonAddConsumer_Click(object sender, EventArgs e)
        {
            if (comboBoxConsumersIDs.Text != string.Empty && textBoxConsumerName.Text != string.Empty && textBoxConsuemrMobile.Text != string.Empty && textBoxConsumerPhone.Text != string.Empty && textBoxConsumerEmail.Text != string.Empty && textBoxConsumerWebsite.Text != string.Empty)
            {
                if (ent.Customers.Find(int.Parse(comboBoxConsumersIDs.Text)) == null)
                {
                    bool isValid;
                    Customer consumer = new Customer();
                    consumer.ID = int.Parse(comboBoxConsumersIDs.Text);
                    consumer.Name = textBoxConsumerName.Text;
                    #region check consumer data and add it to database if it pass validators
                    //Mobile validation
                    isValid = MobileValidator(textBoxConsuemrMobile.Text);
                    if (isValid)
                    {
                        consumer.Mobile = textBoxConsuemrMobile.Text;
                        #region check phone, email and website
                        //Phone Validation
                        isValid = PhoneValidator(textBoxConsumerPhone.Text);
                        if (isValid)
                        {
                            consumer.Phone_Fax = textBoxConsumerPhone.Text;
                            #region check email and website
                            //Email validation
                            isValid = EmailValidator(textBoxConsumerEmail.Text);
                            if (isValid)
                            {
                                consumer.Email = textBoxConsumerEmail.Text;
                                #region check website
                                //website validation
                                isValid = WebSiteValidator(textBoxConsumerWebsite.Text);
                                if (isValid)
                                {
                                    consumer.Web_Site = textBoxConsumerWebsite.Text;
                                    //save supplier to database
                                    ent.Customers.Add(consumer);
                                    ent.SaveChanges();
                                    comboBoxConsumersIDs.Text = textBoxConsuemrMobile.Text = textBoxConsumerEmail.Text = textBoxConsumerName.Text = textBoxConsumerPhone.Text = textBoxConsumerWebsite.Text = string.Empty;
                                }
                                else
                                {
                                    MessageBox.Show("invalid Website");
                                }
                                #endregion
                            }
                            else
                            {
                                MessageBox.Show("invalid Email");
                            }
                            #endregion
                        }
                        else
                        {
                            MessageBox.Show("invalid Phone number");
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("invalid Mobile number");
                    }
                    #endregion
                    RerenderConsumersIDs(comboBoxConsumersIDs);
                }
                else
                {
                    MessageBox.Show("this consumer already exists");
                }
            }
            else
            {
                MessageBox.Show("Incomplete consumer Info");
            }
        }
        private void buttonUpdateConsumer_Click(object sender, EventArgs e)
        {
            var consumer = ent.Customers.Find(int.Parse(comboBoxConsumersIDs.Text));
            if (consumer != null)
            {
                if (comboBoxConsumersIDs.Text != string.Empty && textBoxConsumerName.Text != string.Empty && textBoxConsuemrMobile.Text != string.Empty && textBoxConsumerPhone.Text != string.Empty && textBoxConsumerEmail.Text != string.Empty && textBoxConsumerWebsite.Text != string.Empty)
                {
                    bool isValid;
                    consumer.Name = textBoxConsumerName.Text;
                    #region check consumer data and add it to database if it pass validators
                    //Mobile validation
                    isValid = MobileValidator(textBoxConsuemrMobile.Text);
                    if (isValid)
                    {
                        consumer.Mobile = textBoxConsuemrMobile.Text;
                        #region check phone, email and website
                        //Phone Validation
                        isValid = PhoneValidator(textBoxConsumerPhone.Text);
                        if (isValid)
                        {
                            consumer.Phone_Fax = textBoxConsumerPhone.Text;
                            #region check email and website
                            //Email validation
                            isValid = EmailValidator(textBoxConsumerEmail.Text);
                            if (isValid)
                            {
                                consumer.Email = textBoxConsumerEmail.Text;
                                #region check website and save supplier data
                                //website validation
                                isValid = WebSiteValidator(textBoxConsumerWebsite.Text);
                                if (isValid)
                                {
                                    consumer.Web_Site = textBoxConsumerWebsite.Text;
                                    //save consumer to database
                                    ent.SaveChanges();
                                    comboBoxConsumersIDs.Text = textBoxConsuemrMobile.Text = textBoxConsumerEmail.Text = textBoxConsumerPhone.Text = textBoxConsumerName.Text = textBoxConsumerWebsite.Text = string.Empty;
                                }
                                else
                                {
                                    MessageBox.Show("invalid Website");
                                }
                                #endregion
                            }
                            else
                            {
                                MessageBox.Show("invalid Email");
                            }
                            #endregion
                        }
                        else
                        {
                            MessageBox.Show("invalid Phone number");
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("invalid Mobile number");
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("Incomplete consumer Info");
                }
            }
            else
            {
                MessageBox.Show("Invalid consumer ID");
            }
        }

        #endregion

        #region purchase order panel
        private void buttonAddPurchaseOrder_Click(object sender, EventArgs e)
        {
            // make sure all data of purchase order exists
            if (comboBoxPurchaseOrderIDs.Text != String.Empty && comboBoxStoreNameInPurchaseOrderPanel.Text != string.Empty && comboBoxConsumerIDsInPurchaseOrderPanel.Text != string.Empty && monthCalendarPurchaseOrder.SelectionStart.Date.ToString() != string.Empty)
            {
                if (ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text)) == null)
                {
                    // insert into purchase order table
                    Purchase_Order purchaseOrder = new Purchase_Order();
                    purchaseOrder.ID = int.Parse(comboBoxPurchaseOrderIDs.Text);
                    purchaseOrder.Store_Name = comboBoxStoreNameInPurchaseOrderPanel.Text;
                    purchaseOrder.Customer_ID = int.Parse(comboBoxConsumerIDsInPurchaseOrderPanel.Text);
                    //date time problemaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                    purchaseOrder.Date = monthCalendarPurchaseOrder.SelectionStart;
                    ent.Purchase_Order.Add(purchaseOrder);
                    ent.SaveChanges();
                    MessageBox.Show("Purchase Order Added");
                    textBoxConsumerNameInPurchaseOrderPanel.Text  = comboBoxStoreNameInPurchaseOrderPanel.Text = comboBoxConsumerIDsInPurchaseOrderPanel.Text = string.Empty;
                    RerenderPurchaseOrderIDs(comboBoxPurchaseOrderIDs);
                }
                else
                {
                    MessageBox.Show("this Purchase Order already exists");
                }
            }
            else
            {
                MessageBox.Show("Incomplete Purchase Order Info");
            }
        }
        private void comboBoxPurchaseOrderIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var PurchaseOrder = ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text));
            if (PurchaseOrder != null)
            {
                //add store names to store names combobox in purchase order panel
                RerenderStoreNames(comboBoxStoreNameInPurchaseOrderPanel);

                //add consumers ids to consumer combobox in purchase order panel
                RerenderConsumersIDs(comboBoxConsumerIDsInPurchaseOrderPanel);

                comboBoxStoreNameInPurchaseOrderPanel.Text = PurchaseOrder.Store_Name;
                comboBoxConsumerIDsInPurchaseOrderPanel.Text = PurchaseOrder.Customer_ID.ToString();
                monthCalendarPurchaseOrder.SelectionStart = (DateTime) PurchaseOrder.Date;
                textBoxConsumerNameInPurchaseOrderPanel.Text = PurchaseOrder.Customer.Name;
 
                //add item ids related to purchase oreder to items combobox in purchase order panel
                if (ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text)).Items_PurchaseOrder != null)
                {
                    RerenderPurchaseOrderItems(comboBoxItemIDsInPurchaseOrderPanel);
                }
            }
            else
            {
                MessageBox.Show("Invalid purchase Order ID ... ");
            }
        }
        private void buttonUpdatePurchaseOrder_Click(object sender, EventArgs e)
        {
            if (comboBoxPurchaseOrderIDs.Text != string.Empty)
            {
                var purchaseOrder = ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text));
                if (purchaseOrder != null)
                {
                    if (comboBoxStoreNameInPurchaseOrderPanel.Text != string.Empty && comboBoxConsumerIDsInPurchaseOrderPanel.Text != string.Empty && monthCalendarPurchaseOrder.SelectionStart.Date.ToString() != string.Empty)
                    {
                        if (purchaseOrder.Store_Name == comboBoxStoreNameInPurchaseOrderPanel.Text)
                        {
                            purchaseOrder.Store_Name = comboBoxStoreNameInPurchaseOrderPanel.Text;
                            purchaseOrder.Customer_ID = int.Parse(comboBoxConsumerIDsInPurchaseOrderPanel.Text);
                            //date time problemaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                            purchaseOrder.Date = monthCalendarPurchaseOrder.SelectionStart;
                            MessageBox.Show("Purchase Order Updated");
                            ent.SaveChanges();
                            textBoxConsumerNameInPurchaseOrderPanel.Text = comboBoxStoreNameInPurchaseOrderPanel.Text = comboBoxConsumerIDsInPurchaseOrderPanel.Text = string.Empty;
                        }else
                        {
                            DialogeBox1 warningMsg = new DialogeBox1();
                            DialogResult result;
                            result = warningMsg.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                var ID = int.Parse(comboBoxPurchaseOrderIDs.Text);

                                // find purchase order in purchaseOrder table
                                var purchase_order = ent.Purchase_Order.Find(ID);

                                //update purcahse order with taking in consideration the new store_name
                                Purchase_Order New_purchase_order = new Purchase_Order();
                                purchase_order.ID = int.Parse(comboBoxPurchaseOrderIDs.Text);
                                purchase_order.Customer_ID = int.Parse(comboBoxConsumerIDsInPurchaseOrderPanel.Text);
                                purchase_order.Date = monthCalendarPurchaseOrder.SelectionStart;
                                purchase_order.Store_Name = comboBoxStoreNameInPurchaseOrderPanel.Text;

                                // you should edit qty in items store table befor deleting item from purchase order table
                                if(purchaseOrder.Items_PurchaseOrder.Count > 0)
                                {
                                    foreach (var item in purchaseOrder.Items_PurchaseOrder)
                                    {
                                        DeleteItemFromPurchaseOrder(item.Item_ID,purchase_order.Store_Name,item);
                                    }
                               }

                                MessageBox.Show("Purchase Order Updated");
                                ent.SaveChanges();
                                textBoxConsumerNameInPurchaseOrderPanel.Text = comboBoxStoreNameInPurchaseOrderPanel.Text = comboBoxConsumerIDsInPurchaseOrderPanel.Text = string.Empty;
                                RerenderPurchaseOrderItems(comboBoxItemIDsInPurchaseOrderPanel);
                            }
                            else
                            {
                                textBoxConsumerNameInPurchaseOrderPanel.Text = comboBoxStoreNameInPurchaseOrderPanel.Text = comboBoxConsumerIDsInPurchaseOrderPanel.Text = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incomplete Purchase order Info");
                    }
                }
                else
                {
                    MessageBox.Show($"Purchase Order ID {comboBoxPurchaseOrderIDs.Text} isn't avilable");
                }
            }
            else
            {
                MessageBox.Show("Insert Purchase Order ID");
            }
            
        }
        private void comboBoxConsumerIDsInPurchaseOrderPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var consumers = ent.Customers.Find(int.Parse(comboBoxConsumerIDsInPurchaseOrderPanel.Text));
            if (consumers != null)
            {
                textBoxConsumerNameInPurchaseOrderPanel.Text = consumers.Name;
            }
            else
            {
                MessageBox.Show("Invalid consumer Data ... ");
            }
        }
        private void comboBoxStoreNameInPurchaseOrderPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            RerenderStoreItems(listBoxPurchaseOrder, comboBoxStoreNameInPurchaseOrderPanel.Text);
            //var purchaseID = int.Parse(comboBoxPurchaseOrderIDs.Text);
            //var selectedItems = //(from PurchaseOrderItems in ent.Items_PurchaseOrder where PurchaseOrderItems.PurchaseOrder_ID == purchaseID select PurchaseOrderItems)
            //                    (ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text)).Items_PurchaseOrder).ToString()
            //                    .Intersect
            //                    (from Items in ent.Item_Store where Items.Store_Name == comboBoxStoreNameInPurchaseOrderPanel.Text select Items).ToString();
        }

        #region items within purchase order
        private void buttonAddItemInPurchaseOrder_Click(object sender, EventArgs e)
        {
            // make sure all data of purchaseOrder and items exist
            if (comboBoxPurchaseOrderIDs.Text != string.Empty && comboBoxItemIDsInPurchaseOrderPanel.Text != string.Empty && textBoxQtyInPurchaseOrderPanel.Text != string.Empty)
            {
                // make sure that this purchaseOrder exists and item doesnt exists befor
                if (ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text)) != null)
                {
                    if (ent.Items_PurchaseOrder.Find(int.Parse(comboBoxPurchaseOrderIDs.Text), int.Parse(comboBoxItemIDsInPurchaseOrderPanel.Text)) == null)
                    {
                        // add item to items_purchaseOrder table
                        Items_PurchaseOrder itemsOfPurchaseOrder = new Items_PurchaseOrder();
                        int PurchaseOrderID = itemsOfPurchaseOrder.PurchaseOrder_ID = int.Parse(comboBoxPurchaseOrderIDs.Text);
                        int itemID = itemsOfPurchaseOrder.Item_ID = int.Parse(comboBoxItemIDsInPurchaseOrderPanel.Text);
                        itemsOfPurchaseOrder.Qty = int.Parse(textBoxQtyInPurchaseOrderPanel.Text);

                        //get item from item_store table to check and edit the qty
                        var PurchaseOrder = ent.Purchase_Order.Find(PurchaseOrderID);
                        var storeName = PurchaseOrder.Store_Name;
                        var item_Store = ent.Item_Store.Find(itemID, storeName);
                        if (item_Store != null)
                        {
                            int newQty = int.Parse(item_Store.Qty.ToString()) - int.Parse(textBoxQtyInPurchaseOrderPanel.Text);
                            if (newQty >= 0)
                            {
                                ent.Items_PurchaseOrder.Add(itemsOfPurchaseOrder);
                                item_Store.Qty = newQty;
                                ent.SaveChanges();
                                textBoxQtyInPurchaseOrderPanel.Text = textBoxItemNameInPurchaseOrderPanel.Text = comboBoxItemIDsInPurchaseOrderPanel.Text = string.Empty;
                                RerenderPurchaseOrderItems(comboBoxItemIDsInPurchaseOrderPanel);
                                RerenderStoreItems(listBoxPurchaseOrder, storeName);
                            }
                            else
                            {
                                MessageBox.Show($"Item ID {itemsOfPurchaseOrder.Item_ID} has Qty {item_Store.Qty} in {storeName} Store so Purchase Order Can't contain Qty {textBoxQtyInPurchaseOrderPanel.Text} from this Item");
                            }
                        }
                        else
                        {
                            var stores = (from S in ent.Item_Store where S.Item_iD == itemID select S.Store_Name).AsEnumerable();
                            if (stores.Count() > 0)
                            {
                                string msg = String.Join(", ", stores);
                                MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}. But it may exsists in {msg}");
                            }
                            else
                            {
                                MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}");
                            }
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("this Item already exists");
                    }
                }
                else
                {
                    MessageBox.Show("this Purchase Order doesn't exists");
                }
            }
            else
            {
                MessageBox.Show("Insert Purchase Order Id and Item");
            }
        }
        private void comboBoxItemIDsInPurchaseOrderPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var PurchaseOrderItem = ent.Items_PurchaseOrder.Find(int.Parse(comboBoxPurchaseOrderIDs.Text), int.Parse(comboBoxItemIDsInPurchaseOrderPanel.Text));
            if (PurchaseOrderItem != null)
            {
                textBoxQtyInPurchaseOrderPanel.Text = PurchaseOrderItem.Qty.ToString();
                textBoxItemNameInPurchaseOrderPanel.Text = ent.Items.Find(int.Parse(comboBoxItemIDsInPurchaseOrderPanel.Text)).Name;              
            }
            else
            {
                MessageBox.Show("Invalid Item Data ... ");
            }
        }
        private void buttonUpdateItemInPurchaseOrder_Click(object sender, EventArgs e)
        {
            var PurchaseOrderItem = ent.Items_PurchaseOrder.Find(int.Parse(comboBoxPurchaseOrderIDs.Text), int.Parse(comboBoxItemIDsInPurchaseOrderPanel.Text));
            if (PurchaseOrderItem != null)
            {
                if (textBoxQtyInPurchaseOrderPanel.Text != string.Empty)
                {
                    //add old qty back to the item in store in order to calculate new qty
                    int itemID = int.Parse(comboBoxItemIDsInPurchaseOrderPanel.Text);
                    var PurchaseOrder = ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text));
                    var storeName = PurchaseOrder.Store_Name;
                    var item_Store = ent.Item_Store.Find(itemID, storeName);
                    if (item_Store != null)
                    {
                        item_Store.Qty += PurchaseOrderItem.Qty;

                        //make sure that qty in item store table is a valid qty (not negative value) 
                        int newQty = int.Parse(item_Store.Qty.ToString()) - int.Parse(textBoxQtyInPurchaseOrderPanel.Text);
                        if (newQty >= 0)
                        {
                            item_Store.Qty = newQty;
                            PurchaseOrderItem.Qty = int.Parse(textBoxQtyInPurchaseOrderPanel.Text);
                            ent.SaveChanges();
                            RerenderStoreItems(listBoxPurchaseOrder, storeName);
                            textBoxQtyInPurchaseOrderPanel.Text = textBoxItemNameInPurchaseOrderPanel.Text= comboBoxItemIDsInPurchaseOrderPanel.Text = string.Empty;
                        }
                        else
                        {
                            item_Store.Qty -= PurchaseOrderItem.Qty;
                            ent.SaveChanges();
                            RerenderStoreItems(listBoxPurchaseOrder, storeName);
                            MessageBox.Show($"Item ID {PurchaseOrderItem.Item_ID} has Qty {item_Store.Qty} in {storeName} so Purchase Order Can't contain Qty {textBoxQtyInPurchaseOrderPanel.Text} from this Item");
                        }
                    }
                    else
                    {
                        var stores = (from S in ent.Item_Store where S.Item_iD == itemID select S.Store_Name).AsEnumerable();
                        if (stores.Count() > 0)
                        {
                            string msg = String.Join(", ", stores);
                            MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}. But it may exsists in {msg}");
                        }
                        else
                        {
                            MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}");
                        }
                    }                  
                    
                }
                else
                {
                    MessageBox.Show("Insert new Qty of Item");
                }
            }
            else
            {
                MessageBox.Show("Invalid Item Data ... ");
            }
        }
        private void buttonDeleteItemFromPurchaseOrder_Click(object sender, EventArgs e)
        {
            if (comboBoxPurchaseOrderIDs.Text != string.Empty && comboBoxItemIDsInPurchaseOrderPanel.Text != string.Empty)
            {
                var PurchaseOrderItem = ent.Items_PurchaseOrder.Find(int.Parse(comboBoxPurchaseOrderIDs.Text), int.Parse(comboBoxItemIDsInPurchaseOrderPanel.Text));
                if (PurchaseOrderItem != null)
                {
                    int itemID = int.Parse(comboBoxItemIDsInPurchaseOrderPanel.Text);
                    var PurchaseOrder = ent.Purchase_Order.Find(int.Parse(comboBoxPurchaseOrderIDs.Text));
                    var storeName = PurchaseOrder.Store_Name;
                    //update item qty in item store table in DeleteItemFromPurchaseOrder fn
                    DeleteItemFromPurchaseOrder(itemID, storeName, PurchaseOrderItem);
                    RerenderPurchaseOrderItems(comboBoxItemIDsInPurchaseOrderPanel);
                    RerenderStoreItems(listBoxPurchaseOrder, storeName);
                }
                else
                {
                    MessageBox.Show("This Item for This Purchase Order doesn't exists");
                }
            }
            else
            {
                MessageBox.Show("Insert Purcahse Order and Item ID");
            }
        }

        #endregion

        #endregion

        #region sales order panel
        private void buttonAddSalesOrder_Click(object sender, EventArgs e)
        {
            // make sure all data of sales order exists
            if (comboBoxSalesOrderIDsInSalesOrderPanel.Text != String.Empty && comboBoxStoreNameInSalesOrderPanel.Text != string.Empty && comboBoxSupplierIdInSalesOrderPanel.Text != string.Empty && monthCalendarInSalesOrderpanel.SelectionStart.Date.ToString() != string.Empty)
            {
                if (ent.Pay_Order.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text)) == null)
                {
                    // insert into sales order table
                    Pay_Order salesOrder = new Pay_Order();
                    salesOrder.ID = int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text);
                    salesOrder.Store_Name = comboBoxStoreNameInSalesOrderPanel.Text;
                    salesOrder.Supplier_ID = int.Parse(comboBoxSupplierIdInSalesOrderPanel.Text);
                    //date time problemaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                    salesOrder.Date = monthCalendarInSalesOrderpanel.SelectionStart;
                    ent.Pay_Order.Add(salesOrder);
                    ent.SaveChanges();
                    MessageBox.Show("Sales Order Added");
                    textBoxSupplierNameInSalesOrderPanel.Text = comboBoxStoreNameInSalesOrderPanel.Text = comboBoxSupplierIdInSalesOrderPanel.Text = string.Empty;
                    RerenderSalesOrderIDs(comboBoxSalesOrderIDsInSalesOrderPanel);
                }
                else
                {
                    MessageBox.Show("this Sales Order already exists");
                }
            }
            else
            {
                MessageBox.Show("Incomplete Sales Order Info");
            }
        }
        private void comboBoxSalesOrderIDsInSalesOrderPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var salesOrder = ent.Pay_Order.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text));
            if (salesOrder != null)
            {
                //add store names to store names combobox in sales order panel
                RerenderStoreNames(comboBoxStoreNameInSalesOrderPanel);
                var storeNames = from store in ent.Stores select store;
                
                //add suppliers ids to suppliers combobox in sales order panel
                RerenderSuppliersIDs(comboBoxSupplierIdInSalesOrderPanel);
                
                comboBoxStoreNameInSalesOrderPanel.Text = salesOrder.Store_Name;
                comboBoxSupplierIdInSalesOrderPanel.Text = salesOrder.Supplier_ID.ToString();
                monthCalendarInSalesOrderpanel.SelectionStart = (DateTime)salesOrder.Date;
                textBoxSupplierNameInSalesOrderPanel.Text = salesOrder.Supplier.Name;

                //add item ids to items combobox in sales order panel
                if (ent.Pay_Order.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text)).Items_PayOrder != null)
                {
                    RerenderSalesOrderItems(comboBoxItemIDsInSalesOrderPanel);
                }
            }
            else
            {
                MessageBox.Show("Invalid Sales Order ID ... ");
            }
        }
        private  void buttonUpdateSalesOrder_Click(object sender, EventArgs e)
        {
            if (comboBoxSalesOrderIDsInSalesOrderPanel.Text != string.Empty)
            {
                var salesOrder = ent.Pay_Order.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text));
                if (salesOrder != null)
                {
                    if (comboBoxStoreNameInSalesOrderPanel.Text != string.Empty && comboBoxSupplierIdInSalesOrderPanel.Text != string.Empty && monthCalendarInSalesOrderpanel.SelectionStart.Date.ToString() != string.Empty)
                    {
                        //befor date updated in pay order table .. check if this sales order contain items then these items should be updated in item store table first (store name, date)
                        IList<Items_PayOrder> salesOrderItems = (from Items in ent.Items_PayOrder where Items.PayOrder_ID == salesOrder.ID select Items).ToList();
                        if(salesOrderItems.Count() > 0)
                        {
                            var storeName = salesOrder.Store_Name;
                            foreach(var item in salesOrderItems)
                            {
                                var item_Store = ent.Item_Store.Find(item.Item_ID, storeName);
                                // as store naem is part of pk in item_store table it couldnt be edited directly as:
                                //item_Store.Date = monthCalendarInSalesOrderpanel.SelectionStart;
                                //item_Store.Store_Name = comboBoxStoreNameInSalesOrderPanel.Text;
                                //set data of new Item with taking in consideration the new store_name
                                Item_Store New_item_store = new Item_Store();
                                New_item_store.Item_iD = item_Store.Item_iD;
                                New_item_store.Item_Name = item_Store.Item_Name;
                                New_item_store.Qty = item_Store.Qty;
                                New_item_store.Date = item_Store.Date;
                                New_item_store.Store_Name = comboBoxStoreNameInSalesOrderPanel.Text;

                                // delete Item from Item_Store table
                                ent.Item_Store.Remove(item_Store);
                                ent.SaveChanges();

                                // Re-insert Item in Item_Store table
                                ent.Item_Store.Add(New_item_store);
                                ent.SaveChanges();
                            }
                        }

                        //update in pay order table
                        //date time problemaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                        salesOrder.Date = monthCalendarInSalesOrderpanel.SelectionStart;
                        salesOrder.Store_Name = comboBoxStoreNameInSalesOrderPanel.Text;
                        salesOrder.Supplier_ID = int.Parse(comboBoxSupplierIdInSalesOrderPanel.Text);
                        ent.SaveChanges();
                        MessageBox.Show("Sales Order Updated");
                        textBoxSupplierNameInSalesOrderPanel.Text = comboBoxStoreNameInSalesOrderPanel.Text = comboBoxSupplierIdInSalesOrderPanel.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Incomplete Sales order Info");
                    }
                }
                else
                {
                    MessageBox.Show($"Sales Order ID {comboBoxSalesOrderIDsInSalesOrderPanel.Text} isn't avilable");
                }
            }
            else
            {
                MessageBox.Show("Insert Sales Order ID");
            }

        }
        private void comboBoxSupplierIdInSalesOrderPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var suppliers = ent.Suppliers.Find(int.Parse(comboBoxSupplierIdInSalesOrderPanel.Text));
            if (suppliers != null)
            {
                textBoxSupplierNameInSalesOrderPanel.Text = suppliers.Name;
            }
            else
            {
                MessageBox.Show("Invalid supplier Data ... ");
            }
        }
        private void comboBoxStoreNameInSalesOrderPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            RerenderStoreItems(listBoxSalesOrder, comboBoxStoreNameInSalesOrderPanel.Text);
        }

        #region items within sales order
        private void buttonAddItemToSalesOrder_Click(object sender, EventArgs e)
        {
            // make sure all data of salesOrder and items exist
            if (comboBoxSalesOrderIDsInSalesOrderPanel.Text != string.Empty && comboBoxItemIDsInSalesOrderPanel.Text != string.Empty && textBoxQtyInSalesOrderPanel.Text != string.Empty)
            {
                // make sure that this salesOrder exists and item doesnt exists befor
                if (ent.Pay_Order.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text)) != null)
                {
                    if (ent.Items_PayOrder.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text), int.Parse(comboBoxItemIDsInSalesOrderPanel.Text)) == null)
                    {
                        //insert into items_payOrder table
                        Items_PayOrder itemOfSalesOrder = new Items_PayOrder();
                        itemOfSalesOrder.PayOrder_ID = int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text);
                        itemOfSalesOrder.Item_ID = int.Parse(comboBoxItemIDsInSalesOrderPanel.Text);
                        itemOfSalesOrder.Qty = int.Parse(textBoxQtyInSalesOrderPanel.Text);
                        ent.Items_PayOrder.Add(itemOfSalesOrder);
                        ent.SaveChanges();                  
                        ////------------------------------------------------------------------------------------------------//
                        //insert into store_items table
                        var salesOrder = ent.Pay_Order.Find(itemOfSalesOrder.PayOrder_ID);
                        Item_Store item_store = new Item_Store();
                        item_store.Item_iD = itemOfSalesOrder.Item_ID;
                        var itemName = ent.Items.Find(itemOfSalesOrder.Item_ID).Name;
                        item_store.Item_Name = itemName;
                        item_store.Qty = int.Parse(textBoxQtyInSalesOrderPanel.Text);
                        item_store.Store_Name = salesOrder.Store_Name;
                        item_store.Date = salesOrder.Date;
                        ent.Item_Store.Add(item_store);
                        ent.SaveChanges();
                        textBoxQtyInSalesOrderPanel.Text = textBoxItemNameInSalesOrderPanel.Text = comboBoxItemIDsInSalesOrderPanel.Text = string.Empty;
                        RerenderSalesOrderItems(comboBoxItemIDsInSalesOrderPanel);
                        RerenderStoreItems(listBoxSalesOrder, salesOrder.Store_Name);

                        //////////////////
                        //var item_Store = ent.Item_Store.Find(itemID,storeName);
                        //if (item_Store != null)
                        //{
                        //item_store.Store_Name = salesOrder.Store_Name;
                        //item_store.Date = salesOrder.Date;
                        //    //check qty in item store 
                        //    int newQty = int.Parse(item_Store.Qty.ToString()) + int.Parse(textBoxQtyInSalesOrderPanel.Text);
                        //    if (newQty >= 0)
                        //    {
                        //        item_Store.Qty = newQty;
                        //        ent.SaveChanges();
                        //        textBoxQtyInSalesOrderPanel.Text = comboBoxItemIDsInSalesOrderPanel.Text = string.Empty;
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show($"Item ID {itemsOfSalesOrder.Item_ID} has Qty {item_Store.Qty} in {storeName} Store so Purchase Order Can't contain Qty {itemsOfSalesOrder.Qty} from this Item");
                        //    }
                        //}
                        //else
                        //{
                        //    var stores = (from S in ent.Item_Store where S.Item_iD == itemID select S.Store_Name).AsEnumerable();
                        //    if(stores.Count() > 0)
                        //    {
                        //        string msg = String.Join(", ", stores);
                        //        MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}. But it may exsists in {msg}");
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}");
                        //    }
                        //}

                    }
                    else
                    {
                        MessageBox.Show("this Item already exists");
                    }
                }
                else
                {
                    MessageBox.Show("this Sales Order doesn't exists");
                }
            }
            else
            {
                MessageBox.Show("Select Sales Order Id and Item");
            }
        }
        private void comboBoxItemIDsInSalesOrderPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var salesOrderItem = ent.Items_PayOrder.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text), int.Parse(comboBoxItemIDsInSalesOrderPanel.Text));
            if (salesOrderItem != null)
            {
                textBoxQtyInSalesOrderPanel.Text = salesOrderItem.Qty.ToString();
                textBoxItemNameInSalesOrderPanel.Text = ent.Items.Find(int.Parse(comboBoxItemIDsInSalesOrderPanel.Text)).Name;              
            }
            else
            {
                MessageBox.Show("Invalid Item Data ... ");
            }
        }
        private void buttonUpdateItemInSalesOrder_Click(object sender, EventArgs e)
        {
            var salesOrderItem = ent.Items_PayOrder.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text), int.Parse(comboBoxItemIDsInSalesOrderPanel.Text));
            if (salesOrderItem != null)
            {
                //check that item isn't related to any purchase order 
                int itemID = int.Parse(comboBoxItemIDsInSalesOrderPanel.Text);
                var itemInPurchaseOrder = (from item in ent.Items_PurchaseOrder where item.Item_ID == itemID select item).AsEnumerable();
                if (itemInPurchaseOrder.Count() == 0)
                {
                    if (textBoxQtyInSalesOrderPanel.Text != string.Empty)
                    {
                        //add old qty back to the item in store in order to calculate new qty
                        var salesOrder = ent.Pay_Order.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text));
                        var storeName = salesOrder.Store_Name;
                        var item_Store = ent.Item_Store.Find(itemID, storeName);
                        if (item_Store != null)
                        {
                            item_Store.Qty -= salesOrderItem.Qty;

                            //make sure that qty in item store table is a valid qty (not negative value) 
                            int newQty = int.Parse(item_Store.Qty.ToString()) + int.Parse(textBoxQtyInSalesOrderPanel.Text);
                            if (newQty >= 0)
                            {
                                item_Store.Qty = newQty;
                                salesOrderItem.Qty = int.Parse(textBoxQtyInSalesOrderPanel.Text);
                                ent.SaveChanges();
                                textBoxQtyInSalesOrderPanel.Text = textBoxItemNameInSalesOrderPanel.Text = comboBoxItemIDsInSalesOrderPanel.Text = string.Empty;
                                RerenderStoreItems(listBoxSalesOrder, item_Store.Store_Name);
                            }
                            else
                            {
                                item_Store.Qty += salesOrderItem.Qty;
                                ent.SaveChanges();
                                RerenderStoreItems(listBoxSalesOrder, item_Store.Store_Name);
                                MessageBox.Show($"Item ID {salesOrderItem.Item_ID} has Qty {item_Store.Qty} in {storeName} so Purchase Order Can't contain Qty {textBoxQtyInSalesOrderPanel.Text} from this Item");
                            }
                        }
                        else
                        {
                            var stores = (from S in ent.Item_Store where S.Item_iD == itemID select S.Store_Name).AsEnumerable();
                            if (stores.Count() > 0)
                            {
                                string msg = String.Join(", ", stores);
                                MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}. But it may exsists in {msg}");
                            }
                            else
                            {
                                MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}");
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Insert new Qty of Item");
                    }
                }
                else
                {
                    List<int> purcahseOrderIDs = new List<int>();
                    foreach (var i in itemInPurchaseOrder)
                    {
                        purcahseOrderIDs.Add(i.PurchaseOrder_ID);
                    }
                    MessageBox.Show($"can't update in this Pay Order as it related to purchase Order {String.Join(", ",purcahseOrderIDs)}");
                }
            }
            else
            {
                MessageBox.Show("Invalid Item Data ... ");
            }
        }
        private void buttonDeleteItemFromSalesOrder_Click(object sender, EventArgs e)
        {
            if (comboBoxSalesOrderIDsInSalesOrderPanel.Text != string.Empty && comboBoxItemIDsInSalesOrderPanel.Text != string.Empty)
            {
                var salesOrderItem = ent.Items_PayOrder.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text), int.Parse(comboBoxItemIDsInSalesOrderPanel.Text));
                if (salesOrderItem != null)
                {
                    //check that item isn't related to any purchase order 
                    int itemID = int.Parse(comboBoxItemIDsInSalesOrderPanel.Text);
                    var itemsInPurchaseOrder = (from item in ent.Items_PurchaseOrder where item.Item_ID == itemID select item).AsEnumerable();
                    if (itemsInPurchaseOrder.Count() == 0)
                    {
                        var salesOrder = ent.Pay_Order.Find(int.Parse(comboBoxSalesOrderIDsInSalesOrderPanel.Text));
                        var storeName = salesOrder.Store_Name;
                        var item_Store = ent.Item_Store.Find(itemID, storeName);
                        if (item_Store != null)
                        {
                            //remove item from item store table
                            ent.Item_Store.Remove(item_Store);
                            //remove item from purchase order
                            ent.Items_PayOrder.Remove(salesOrderItem);
                            comboBoxItemIDsInSalesOrderPanel.Text = textBoxQtyInSalesOrderPanel.Text = string.Empty;
                            ent.SaveChanges();
                            RerenderStoreItems(listBoxSalesOrder, storeName);
                            RerenderSalesOrderItems(comboBoxItemIDsInSalesOrderPanel);
                        }
                        else
                        {
                            var stores = (from S in ent.Item_Store where S.Item_iD == itemID select S.Store_Name).AsEnumerable();
                            if (stores.Count() > 0)
                            {
                                string msg = String.Join(", ", stores);
                                MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}. But it may exsists in {msg}");
                            }
                            else
                            {
                                MessageBox.Show($"item ID {itemID} doesn't exists in store {storeName}");
                            }
                        }
                    }
                    else
                    {
                        List<int> PurchaseOrderIDs = new List<int>();
                        foreach (var i in itemsInPurchaseOrder)
                        {
                            PurchaseOrderIDs.Add(i.PurchaseOrder_ID);
                        }
                        MessageBox.Show($"can't update in this Sales Order as it related to Purchase Order {string.Join(" ", PurchaseOrderIDs)}");
                    }
                    
                }
                else
                {
                    MessageBox.Show("This Item for This Sales Order doesn't exists");
                }
            }
            else
            {
                MessageBox.Show("Select Sales Order and Item");
            }
        }

        #endregion

        #endregion

        #region Reports

        #region Report 1 report on selected store (filter by: date)
        private void comboBoxStoreNamesInReport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Store = ent.Stores.Find(comboBoxStoreNamesInReport1.Text);
            if (Store != null)
            {
                labelLocation.Text = Store.Location;
                labelResponsiblePerson.Text = Store.Responsible_person;
                labelLocation.Visible = true;
                labelResponsiblePerson.Visible = true;

                dataGridViewReport1.DataSource = Store.Item_Store.ToList();
                dataGridViewReport1.Columns.RemoveAt(dataGridViewReport1.Columns.Count - 1);
                dataGridViewReport1.Columns.RemoveAt(dataGridViewReport1.Columns.Count - 1);
            }
            else
            {
                MessageBox.Show("invalid Store Name");
            }
        }
        private void dateTimePickerReport1_ValueChanged(object sender, EventArgs e)
        {
            var Store = ent.Stores.Find(comboBoxStoreNamesInReport1.Text);
            var date = dateTimePickerReport1.Value.Date.ToString();
            if (comboBoxStoreNamesInReport1.Text != string.Empty)
            {
                dataGridViewReport1.DataSource = Store.Item_Store.Where(a => a.Date.ToString() == date).ToList();
                if (dataGridViewReport1.Columns.Count > 0)
                {
                    dataGridViewReport1.Columns.RemoveAt(dataGridViewReport1.Columns.Count - 1);
                    dataGridViewReport1.Columns.RemoveAt(dataGridViewReport1.Columns.Count - 1);
                }
            }
            else
            {
                MessageBox.Show("Select the Store First");
            }
        }

        #endregion
        #region Report 2 report on selected item (filter by: store and date)
        private void comboBoxItemIdsReport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get item IDs related to item Name
            var item = (from I in ent.Items where I.Name == comboBoxItemNamesReport2.Text select I).FirstOrDefault();
            // insert store names related to that item in storeNames combobox
            var Stores = (from S in ent.Item_Store where S.Item_iD == item.ID select S.Store_Name).AsEnumerable();
            checkedListBoxStoreNamesInReport2.Items.Clear();
            if (Stores.Count() > 0)
            {
                foreach (var store in Stores)
                {
                    checkedListBoxStoreNamesInReport2.Items.Add(store);
                }
            }

            //show data grid
            dataGridViewReport2.DataSource = ent.R2Date(comboBoxItemNamesReport2.Text, null);

        }
        private void checkedListBoxStoreNamesInReport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (from I in ent.Item_Store where I.Item.Name == comboBoxItemNamesReport2.Text select I).ToList();

            var checkedItems = checkedListBoxStoreNamesInReport2.CheckedItems.OfType<string>();
            if(checkedItems.Count() > 0)
            {
                var list = string.Join(",", checkedItems);
                dataGridViewReport2.DataSource = ent.R2Store(comboBoxItemNamesReport2.Text,list);
            }
        }
        private void dateTimePickerReport2_ValueChanged(object sender, EventArgs e)
        {
            var item = (from I in ent.Item_Store where I.Item.Name == comboBoxItemNamesReport2.Text select I).ToList();
            var date = dateTimePickerReport2.Value.Date;
            if (comboBoxItemNamesReport2.Text != string.Empty)
            {
                var checkedItems = checkedListBoxStoreNamesInReport2.CheckedItems.OfType<string>();
                if (checkedItems.Count() > 0)
                {
                    var list = string.Join(",", checkedItems);
                    dataGridViewReport2.DataSource = ent.R2Both(comboBoxItemNamesReport2.Text, list,date);
                }
                else
                {
                    dataGridViewReport2.DataSource = ent.R2Date(comboBoxItemNamesReport2.Text, date);
                }

                ////code used combobox to select store  instead of using checkedlist
                //if (comboBoxStoreNamesReport2.Text != string.Empty)
                //{
                //    dataGridViewReport2.DataSource = item.Item_Store.Where(a => a.Date.ToString() == date && a.Store_Name == comboBoxStoreNamesReport2.Text).ToList();
                //    RerenderDataInReport2(dataGridViewReport2, item);
                //}
                //else
                //{
                //    dataGridViewReport2.DataSource = item.Item_Store.Where(a => a.Date.ToString() == date).ToList();
                //    RerenderDataInReport2(dataGridViewReport2, item);
                //}
            }
            else
            {
                MessageBox.Show("Select the Item First");
            }
        }

        #endregion
        #region Report 3 report on purchase and sales orders (filter by: 1-date( from: to: ) 2-store )
        private void checkedListBoxReport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var checkedItems = checkedListBoxReport3.CheckedItems.OfType<string>();
            var dateFrom = dateTimePickerReport3From.Value;
            var dateTo = dateTimePickerReport3To.Value;
            if (dateFrom.ToShortDateString() != string.Empty && dateTo.ToShortDateString() != string.Empty && checkedItems.Count() > 0)
            {
                var list = string.Join(",", checkedItems);
                dataGridViewReport3.DataSource = ent.R3New(list, dateFrom, dateTo);
            }
        }

        #endregion
        #region Report 4 report on item (date > dateOFEnteranceToStore.Add(x days))
        private void textBoxReport4_TextChanged(object sender, EventArgs e)
        {
            bool isValid;
            if (textBoxReport4.Text != string.Empty)
            {
                isValid = IntType(textBoxReport4.Text);
                if (isValid)
                {
                    int TimeInterval = int.Parse(textBoxReport4.Text);
                    dataGridViewReport4.DataSource = ent.R4(TimeInterval);
                }
                else
                {
                    MessageBox.Show("Invalid Time Interval");
                }
            }
        }

        #endregion
        #region Report 5 report on item (shelfLIfe <= x days)
        private void textBoxReport5_TextChanged(object sender, EventArgs e)
        {

            bool isValid;
            if (textBoxReport5.Text != string.Empty)
            {
                isValid = IntType(textBoxReport5.Text);
                if (isValid)
                {
                    int TimeInterval = int.Parse(textBoxReport5.Text);
                    dataGridViewReport5.DataSource = ent.R5(TimeInterval);
                }
                else
                {
                    MessageBox.Show("Invalid Time Interval");
                }
            }
        }




        #endregion

        #endregion

       
    }
}
