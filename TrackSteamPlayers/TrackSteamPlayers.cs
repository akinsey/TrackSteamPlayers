using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;

namespace TrackSteamPlayers
{
    public partial class TrackSteamPlayers : Form
    {
        private delegate void UpdateDelegate(ListViewItem param); // Delegate for Invoking method from within thread
        private delegate void SetSteamIdDelegate(string id); // Delegate for Invoking method from within thread
        private delegate void StopProgressDelegate(); // Delegate for Invoking method from within thread
        readonly object steamListLock = new object(); // Lock for modifying steamIdList
        readonly object infoListLock = new object(); // Lock for modifying infoList
        readonly object progressBarLock = new object(); // Lock for modifying progressBar
        readonly object flagListLock = new object(); // Lock for modifying infoList
        Dictionary<string, string[]> infoList = new Dictionary<string, string[]>(); // Stores Player Info
        List<string> steamIdList = new List<string>(); // Stores Player Steam IDs
        List<string> flaggedForDeletion = new List<string>();
        private string dbFile = @"TrackSteamPlayers.db";
        private ListViewColumnSorter lvwColumnSorter;
        private string curSteamId = string.Empty;

        public TrackSteamPlayers()
        {
            InitializeComponent();
            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            playerTable.ListViewItemSorter = lvwColumnSorter;
            lvwColumnSorter.SortColumn = 0;
            lvwColumnSorter.Order = SortOrder.Ascending;
            playerTable.Sort();
            ColumnHeader header = playerTable.Columns[0];
            header.Text = "\u25B2 " + header.Text;
        }

        private void TrackSteamPlayers_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(645, 350);
            try
            {

                // Populate local steamID List from file
                using (TextReader reader = new StreamReader(dbFile))
                {
                    string line = string.Empty;
                    lock (steamListLock)
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            steamIdList.Add(line);
                            lock (progressBarLock)
                            {
                                progressBar.MarqueeAnimationSpeed = 10;
                            }
                        }
                    }
                }

                // Start a worker thread to continuously update the player table
                Thread thread = new Thread(new ThreadStart(updateUserInfo));
                thread.IsBackground = true;
                thread.Name = "updateInfoTable";
                thread.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void trackPlayer_Click(object sender, EventArgs e)
        {
            string url = profileInput.Text.ToString().Trim();
            if (!url.Contains("http://steamcommunity"))
            {
                profileInput.Text = "Please enter a valid Steam Community profile URL";
                return;
            }
            lock (flagListLock)
            {
                flaggedForDeletion.Clear();
            }
            lock (progressBarLock)
            {
                progressBar.MarqueeAnimationSpeed = 10;
            }
            trackPlayer.Enabled = false;
            profileInput.Enabled = false;
            Thread thread = new Thread(new ParameterizedThreadStart(getSteamIdFromUrl));
            thread.IsBackground = true;
            thread.Name = "Get Steam ID Thread";
            thread.Start(url);
            profileInput.Text = string.Empty;

        }

        private void updateDbFile()
        {
            //Clear DB File
            File.WriteAllText(dbFile, string.Empty);
            //Repopulate DB File from steam ID list
            using (TextWriter writer = new StreamWriter(dbFile, true))  // true is for append mode
            {
                lock (steamListLock)
                {
                    foreach (string item in steamIdList)
                    {
                        writer.WriteLine(item);
                    }
                }
            }
        }

        private void updateUserInfo()
        {
            // We want to continously update the users info for the life of the app
            while (true)
            {
                // Need lock even while were iterating through steamId
                lock (steamListLock)
                {
                    // Start a worker thread to pull data from the steam web API
                    foreach (string steamId in steamIdList)
                    {
                        Thread thread = new Thread(new ParameterizedThreadStart(parseDataFromWeb));
                        thread.IsBackground = true;
                        thread.Name = "Worker Thread [" + steamId + "]";
                        thread.Start(steamId);
                    }
                }

                // If the list isn't empty we need to start the 
                if (steamIdList.Count != 0)
                {
                    // Need lock while iterating through infoList
                    lock (infoListLock)
                    {
                        // Updates player list view with contents of infoList
                        foreach (string[] strArray in infoList.Values)
                        {
                            ListViewItem item = new ListViewItem(strArray);
                            item.Name = strArray[3];
                            updatePlayerTable(item);
                        }
                    }
                    Thread.Sleep(10000); // Update in 10 second intervals
                }
            }
        }

        private void updatePlayerTable(ListViewItem param)
        {
            // Method must be invoked if called within worker thread
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateDelegate(updatePlayerTable), new object[] { param });
                return;
            }

            // Param.Name is the steamId and Key for the playerTable
            string steamId = param.Name;

            // Check if playerTable has item already
            if (playerTable.Items.ContainsKey(steamId))
            {
                // Determine if we've made any updates
                bool wasUpdated = false;

                // Check if name needs updating
                string oldName = playerTable.Items.Find(steamId, false)[0].SubItems[0].Text;
                string newName = param.SubItems[0].Text;
                if (!oldName.Equals(newName))
                {
                    playerTable.Items.Find(steamId, false)[0].SubItems[0].Text = newName;
                    wasUpdated = true;
                }

                // Check if server needs updating
                string oldServer = playerTable.Items.Find(steamId, false)[0].SubItems[1].Text;
                string newServer = param.SubItems[1].Text;
                if (!oldServer.Equals(newServer))
                {
                    playerTable.Items.Find(steamId, false)[0].SubItems[1].Text = newServer;
                    wasUpdated = true;
                }

                // Check if the server needs updating
                string oldGame = playerTable.Items.Find(steamId, false)[0].SubItems[2].Text;
                string newGame = param.SubItems[2].Text;
                if (!oldGame.Equals(newGame))
                {
                    playerTable.Items.Find(steamId, false)[0].SubItems[2].Text = newGame;
                    if (!String.IsNullOrEmpty(newGame))
                    {
                        playerTable.Items.Find(steamId, false)[0].BackColor = Color.LightGreen;
                    }
                    else
                    {
                        playerTable.Items.Find(steamId, false)[0].BackColor = Color.White;
                    }
                    wasUpdated = true;
                }
                if (wasUpdated) Console.WriteLine(">>>> Updating row for player [" + steamId + "]");
            }
            else
            {
                playerTable.Items.Add(param);
                stopProgressBar();
                trackPlayer.Enabled = true;
                profileInput.Enabled = true;
                Console.WriteLine(">>>> Adding row for player [" + steamId + "]");
            }
        }

        private void parseDataFromWeb(object steamId) 
        {
            try
            {
                string id = steamId as string;
                WebRequest request = WebRequest.Create("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=050781F5BFF1CD9BAE15AB65E090B6F4&steamids=" + id);
                WebResponse response = request.GetResponse();

                string playerName = "personaname";
                string serverIp = "gameserverip";
                string game = "gameextrainfo";

                string saveName = string.Empty;
                string saveIp = string.Empty;
                string saveGame = string.Empty;

                // Parse data from WebResponse
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line = string.Empty;
                    const string pattern = "\"([^\"]*?)\"";

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.IndexOf(playerName) != -1) // Get player Name
                        {
                            var result = Regex.Matches(line, pattern);
                            saveName = result[1].Groups[1].Value;
                        }
                        else if (line.IndexOf(serverIp) != -1) // Get server IP
                        {
                            var result = Regex.Matches(line, pattern);
                            saveIp = result[1].Groups[1].Value;
                        }
                        else if (line.IndexOf(game) != -1) // Get game Name
                        {
                            var result = Regex.Matches(line, pattern);
                            saveGame = result[1].Groups[1].Value;
                        }
                    }
                }

                // Obtain lock and add user to the infoList
                lock (flagListLock)
                    lock (infoListLock)
                    {
                        if (infoList.ContainsKey(id))
                        {
                            infoList.Remove(id);
                        }
                        if (flaggedForDeletion.IndexOf(id) == -1)
                        {
                            string[] param = new string[] { saveName, saveGame, saveIp, id };
                            infoList.Add(id, param);
                            ListViewItem item = new ListViewItem(param);
                            item.Name = param[3];
                            updatePlayerTable(item);
                            
                        }
                    }
                Console.WriteLine("\n>> Added player [" + id + "] to infoList\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nCannot connect to Steam servers.");
                stopProgressBar();
            }
        }

        private void removePlayer_Click(object sender, EventArgs e)
        {
            lock (flagListLock)
            lock (steamListLock)
            lock (infoList)
            {
                for (int i = 0; i < playerTable.Items.Count; i++)
                {
                    if (playerTable.Items[i].Selected)
                    {
                        string steamId = playerTable.Items[i].SubItems[3].Text;
                        playerTable.Items[i].Remove();
                        steamIdList.Remove(steamId);
                        infoList.Remove(steamId);
                        flaggedForDeletion.Add(steamId);
                        i--;
                    }
                }
            }
            updateDbFile();
        }

        private void playerTable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
                for (int i = 0; i < playerTable.Items.Count; i++)
                {
                    if (playerTable.Items[i].Selected)
                    {
                        string serverIp = playerTable.Items[i].SubItems[2].Text;
                        Clipboard.Clear();
                        if(!string.IsNullOrEmpty(serverIp)) Clipboard.SetText(serverIp);
                        playerTable.SelectedItems.Clear();
                        break;
                    }
                }
        }

        private void getSteamIdFromUrl(object url)
        {
            try
            {
                string fullUrl = (url as string) + "?xml=1";
                WebRequest request = WebRequest.Create(fullUrl);
                WebResponse response = request.GetResponse();

                string srcString = "steamID64";

                // Parse data from WebResponse
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line = string.Empty;
                    string pattern = "<" + srcString + ">([^\"]*?)</" + srcString + ">";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.IndexOf(srcString) != -1) // Get steam id
                        {
                            var result = Regex.Matches(line, pattern);
                            setSteamId(result[0].Groups[1].Value);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nCannot connect to Steam servers.");
                stopProgressBar();
            }
        }

        private void stopProgressBar()
        {
            // Method must be invoked if called within worker thread
            if (this.InvokeRequired)
            {
                this.Invoke(new StopProgressDelegate(stopProgressBar));
                return;
            }
            lock (progressBarLock)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.MarqueeAnimationSpeed = 0;
                progressBar.Style = ProgressBarStyle.Marquee;
            }
        }

        private void setSteamId(string id)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetSteamIdDelegate(setSteamId), new object[] { id });
                return;
            }
            lock (steamListLock)
            {
                if (steamIdList.Contains(id))
                {
                    stopProgressBar();
                    trackPlayer.Enabled = true;
                    profileInput.Enabled = true;
                    return;
                }
                steamIdList.Add(id);
            }
            updateDbFile();
        }

        private void playerTable_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            foreach (ColumnHeader header in playerTable.Columns)
            {
                if (header.Index == e.Column)
                {
                    if (header.Text.Contains("\u25BC")) header.Text = "\u25B2 " + header.Text.Split('\u25BC')[1].Trim(); 
                    else if (header.Text.Contains("\u25B2")) header.Text = "\u25BC " + header.Text.Split('\u25B2')[1].Trim();       
                    else header.Text = ((lvwColumnSorter.Order == SortOrder.Descending) ? "\u25BC " : "\u25B2 ") + header.Text;
                }
                else
                {
                    string headerText = header.Text;
               
                    if(header.Text.Contains("\u25BC")) headerText = header.Text.Split('\u25BC')[1].Trim();
                    else if(header.Text.Contains("\u25B2")) headerText = header.Text.Split('\u25B2')[1].Trim();
               
                    header.Text = headerText;
                }
            }

            // Perform the sort with these new sort options.
            playerTable.Sort();
        }

        private void profileInput_Click(object sender, EventArgs e)
        {
            profileInput.Clear();
        }

        private void playerTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectionArr = playerTable.SelectedItems;
            foreach (ListViewItem selected in selectionArr)
            {
                if(selected.BackColor == Color.LightGreen) selected.BackColor = Color.White;
            }
        }

        private void profileInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                trackPlayer.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
    }
}
