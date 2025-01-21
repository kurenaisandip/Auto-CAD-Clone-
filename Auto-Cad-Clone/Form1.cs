using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Cad_Clone
{
    public partial class Form1 : Form
    {
        private MenuStrip menuStrip;
        private HubConnection _hubConnection;
        private long userID = 15;
        public Form1()
        {
            InitializeComponent();
            this.Text = "Auto Cad Clone";

            InitializeSignalR();

            // Initialize the MenuStrip
            menuStrip = new MenuStrip();

            // Create Help Menu with Drop-down Items
        ToolStripMenuItem helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDownItems.Add("About", null, OnAboutClick);
            helpMenu.DropDownItems.Add("Documentation", null, OnDocumentationClick);
            helpMenu.DropDownItems.Add("FAQ", null, OnFAQClick);

            // Create Search Menu with a Search Box
            ToolStripMenuItem searchMenu = new ToolStripMenuItem("Search");
            ToolStripTextBox searchBox = new ToolStripTextBox();
            searchBox.ToolTipText = "Type your search here";
            searchBox.KeyDown += OnSearchKeyDown; // Event handler for search
            searchMenu.DropDownItems.Add(searchBox);

            // Add both menus to the MenuStrip
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }


        //private async void InitializeSignalR()
        //{
        //    _hubConnection = new HubConnectionBuilder()
        //        .WithUrl("http://localhost:5221/notifications") 
        //        .WithAutomaticReconnect() 
        //        .Build();

        //    // Listen for messages from the server
        //    _hubConnection.On<string>("ReceiveNotification", (message) =>
        //    {
        //        // Update the UI on the main thread
        //        this.Invoke((MethodInvoker)delegate
        //        {
        //            textBox1.AppendText($"{message}{Environment.NewLine}");
        //        });
        //    });

        //    // Handle connection events
        //    _hubConnection.Closed += async (error) =>
        //    {
        //        MessageBox.Show("Connection closed. Attempting to reconnect...");
        //        await Task.Delay(new Random().Next(0, 5) * 1000); 
        //        await _hubConnection.StartAsync(); 
        //    };

        //    _hubConnection.Reconnected += (connectionId) =>
        //    {
        //        MessageBox.Show("Reconnected to SignalR Hub!");
        //        return Task.CompletedTask;
        //    };

        //    _hubConnection.Reconnecting += (error) =>
        //    {
        //        MessageBox.Show("Reconnecting to SignalR Hub...");
        //        return Task.CompletedTask;
        //    };

        //    try
        //    {
        //        await _hubConnection.StartAsync();
        //        MessageBox.Show("Connected to SignalR Hub!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error: {ex.Message}");
        //    }
        //}
        private async void InitializeSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5221/notifications")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("ReceiveNotification", (message) =>
            {
                Console.WriteLine($"Received message: {message}"); // Log to console
                this.Invoke((MethodInvoker)delegate
                {
                    textBox1.AppendText($"{message}{Environment.NewLine}");
                });
            });

            _hubConnection.Closed += async (error) =>
            {
                MessageBox.Show("Connection closed. Attempting to reconnect...");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hubConnection.StartAsync();
            };

            _hubConnection.Reconnected += (connectionId) =>
            {
                MessageBox.Show("Reconnected to SignalR Hub!");
                return Task.CompletedTask;
            };

            _hubConnection.Reconnecting += (error) =>
            {
                MessageBox.Show("Reconnecting to SignalR Hub...");
                return Task.CompletedTask;
            };

            try
            {
                await _hubConnection.StartAsync();
                MessageBox.Show("Connected to SignalR Hub!");

                // Log the connection state
                Console.WriteLine($"Connection state: {_hubConnection.State}");

                // Register the user ID with the server
                string userId = "desktop"; // Replace this with a unique user ID
                await _hubConnection.InvokeAsync("RegisterUserId", userId);
                MessageBox.Show($"User ID {userId} registered!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        //private async void button1_Click(object sender, EventArgs e)
        //{
        //    // Example: Manually reconnect if needed
        //    if (_hubConnection.State == HubConnectionState.Disconnected)
        //    {
        //        await _hubConnection.StartAsync();
        //    }
        //}

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose of the SignalR connection when the form is closing
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    

        // Event Handlers for Help Menu
        private void OnAboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("About this application...");
        }

        private void OnDocumentationClick(object sender, EventArgs e)
        {
            MessageBox.Show("Open documentation...");
        }

        private void OnFAQClick(object sender, EventArgs e)
        {
            MessageBox.Show("Open FAQ...");
        }

        // Event Handler for Search Box
        private void OnSearchKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ToolStripTextBox searchBox = (ToolStripTextBox)sender;
                string query = searchBox.Text;
                MessageBox.Show("Search query: " + query);
                e.Handled = true;
            }
        }
    }
}