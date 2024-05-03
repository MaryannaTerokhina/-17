using System;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace _17
{
    public partial class Form1 : Form
    {
        private const string DogApiUrl = "https://dog.ceo/api/breeds/image/random";
        private System.Timers.Timer timer;

        public Form1()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(600);
            timer.Elapsed += async (sender, e) => await UpdateDogImage();
        }

        private async Task<string> GetRandomDogImageUrl()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(DogApiUrl);
                var json = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(json);
                return data["message"].ToString();
            }
        }

        private async Task UpdateDogImage()
        {
            try
            {
                var imageUrl = await GetRandomDogImageUrl();
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    pictureBoxDog.ImageLocation = imageUrl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка під час отримання фоточки собачки: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await UpdateDogImage();
            timer.Start();
        }


    }
}
