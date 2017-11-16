using AngleSharp.Parser.Html;
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

namespace browser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
        }
        private void navigateToPage()
        {
            
            button1.Enabled = false;
            textBox1.Enabled = false;
            webBrowser1.Navigate(textBox1.Text);

              



        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            navigateToPage();
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)ConsoleKey.Enter)
                navigateToPage();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            button1.Enabled = true;
            textBox1.Enabled = true;
   

        }

        
        private void webBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if (e.MaximumProgress > 0)
            {
                progressBar1.Value = (int)((e.CurrentProgress * 100) / e.MaximumProgress);
               if(((e.CurrentProgress*100)/e.MaximumProgress)>80)
                    SendKeys.Send("{END}");
                }
            }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var html = webBrowser1.Document.Body.OuterHtml;
            File.WriteAllText("c:/data/yahoo.html", html);
        }

        private void scrapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var textResponse = File.ReadAllText("C:/data/yahoo.html");
            var parser = new HtmlParser();
            var html = parser.Parse(textResponse);


            var items =
                html.QuerySelectorAll(".strm-default-clusters")
                .Select(elem => new
                {
                    Title = elem.QuerySelectorAll("span")[2].TextContent.Trim(),
                    Content = elem.QuerySelector(".stream-summary").TextContent.Trim(),
                    Images =
                        elem.QuerySelectorAll("img")
                        .Select(img => img.Attributes["src"].Value)
                        .ToArray()
                })
                .ToArray();


            var output = "";
            foreach (var item in items)
            {

                foreach (var image in item.Images)
                    output += $"<img width=\"10%\" src=\"{image}\"/>";
                output += "<br/>";
                output += $"<h1>{item.Title}</h1>";
                output += $"<p>{item.Content}</p>";
            }

            File.WriteAllText("C:/data/imgs.html", output);
        


    
}
    }
    }

